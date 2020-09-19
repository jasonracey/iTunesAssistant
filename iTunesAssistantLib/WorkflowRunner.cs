﻿using System;
using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class WorkflowRunner
    {
        private readonly IAppClassWrapper _appClassWrapper;

        public WorkflowRunner(IAppClassWrapper appClassWrapper)
        {
            _appClassWrapper = appClassWrapper ?? 
                throw new ArgumentNullException(nameof(appClassWrapper));
        }

        public string? State { get; private set; }
        public int ItemsProcessed { get; private set; }
        public int ItemsTotal { get; private set; }

        public void Run(IEnumerable<Workflow> workflows)
        {
            if (workflows?.Count() == 0)
            {
                return;
            }

            SetNewState(0, _appClassWrapper.LibraryPlaylist.Tracks.Count, "Loading selected tracks...");

            if (_appClassWrapper.SelectedTracks == null)
            {
                return;
            }

            var tracksToFix = new List<IITTrack>();

            foreach (IITTrack? track in _appClassWrapper.SelectedTracks)
            {
                if (track != null)
                {
                    tracksToFix.Add(track);
                    ItemsProcessed++;
                }
            }

            if (tracksToFix.Count == 0)
            {
                return;
            }

            if (workflows.Any(workflow => workflow.Name == WorkflowName.MergeAlbums))
            {
                RunMergeAlbumsWorkflow(tracksToFix);
            }

            var inputFilePath = workflows
                .FirstOrDefault(workflow => workflow.Name == WorkflowName.ImportTrackNames)?
                .FileName;

            if (!string.IsNullOrWhiteSpace(inputFilePath))
            {
                RunImportTrackNamesWorkflow(tracksToFix, inputFilePath);
            }

            var albumWorkflows = workflows.Where(workflow => workflow.Type == WorkflowType.Album);

            if (albumWorkflows.Any())
            {
                RunAlbumWorkflows(tracksToFix, albumWorkflows);
            }

            var trackWorkflows = workflows.Where(workflow => workflow.Type == WorkflowType.Track);

            if (trackWorkflows.Any())
            {
                RunTrackWorkflows(tracksToFix, trackWorkflows);
            }
        }

        private void SetNewState(int itemsProcessed, int itemsTotal, string state)
        {
            ItemsProcessed = itemsProcessed;
            ItemsTotal = itemsTotal;
            State = state;
        }

        private void RunMergeAlbumsWorkflow(IList<IITTrack> tracksToFix)
        {
            SetNewState(0, tracksToFix.Count, "Generating album groups...");

            var albumGroups = new Dictionary<string, IDictionary<string, IList<IITTrack>>>();

            foreach (var track in tracksToFix)
            {
                var newAlbumName = AlbumNameFixer.FixAlbumName(track.Album);

                if (!albumGroups.ContainsKey(newAlbumName))
                {
                    albumGroups.Add(newAlbumName, new SortedDictionary<string, IList<IITTrack>>());
                    albumGroups[newAlbumName].Add(track.Album, new List<IITTrack> { track });
                }
                else if (!albumGroups[newAlbumName].ContainsKey(track.Album))
                {
                    albumGroups[newAlbumName].Add(track.Album, new List<IITTrack> {track});
                }
                else
                {
                    albumGroups[newAlbumName][track.Album].Add(track);
                }

                ItemsProcessed++;
            }

            foreach (var album in albumGroups.SelectMany(albumGroup => albumGroup.Value.Values))
            {
                var comparer = TrackComparerFactory.GetTrackComparer(album);
                album.ToList().Sort(comparer);
            }

            SetNewState(0, albumGroups.Count, "Running merge albums workflow...");

            foreach (var albumGroup in albumGroups)
            {
                var newAlbumName = albumGroup.Key;
                var trackCount = albumGroup.Value.Sum(album => album.Value.Count);
                var trackNumber = 1;

                foreach (var track in albumGroup.Value.SelectMany(album => album.Value))
                {
                    track.Album = newAlbumName;
                    track.DiscCount = 1;
                    track.DiscNumber = 1;

                    // must set number before count in case old number is higher than count
                    track.TrackNumber = trackNumber++;
                    track.TrackCount = trackCount;
                }

                ItemsProcessed++;
            }
        }

        private void RunImportTrackNamesWorkflow(IList<IITTrack> tracksToFix, string inputFilePath)
        {
            SetNewState(0, tracksToFix.Count, "Reading track names to import...");

            var newTrackNames = System.IO.File.ReadAllLines(inputFilePath);
            
            var cleanedNewTrackNames = new List<string>();
            foreach (var newTrackName in newTrackNames)
            {
                var cleanName = newTrackName.Trim();
                if (cleanName != string.Empty)
                {
                    cleanedNewTrackNames.Add(cleanName);
                    ItemsProcessed++;
                }
            }

            if (tracksToFix.Any(track => track.TrackNumber == 0))
            {
                throw new iTunesAssistantException("One or more tracks does not have a track number");
            }

            if (tracksToFix.Count != cleanedNewTrackNames.Count)
            {
                throw new iTunesAssistantException("The number of names to import must match the number of tracks selected");
            }

            SetNewState(0, tracksToFix.Count, "Assigning new track names...");

            for (var i = 0; i < tracksToFix.Count; i++)
            {
                tracksToFix[i].Name = cleanedNewTrackNames[i];
                ItemsProcessed++;
            }

            return;
        }

        private void RunAlbumWorkflows(IList<IITTrack> tracksToFix, IEnumerable<Workflow> albumWorkflows)
        {
            var albums = GetAlbums(tracksToFix);

            SetNewState(0, albums.Count, "Running album workflows...");

            const string trackMissingErrorCode = "0xA0040203";
            const string trackMissingErrorMessage = "One or more tracks could not be found in your file system";

            foreach (var album in albums)
            {
                // must set number before count in case old number is higher than count
                if (albumWorkflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNumbers))
                {
                    var trackComparer = TrackComparerFactory.GetTrackComparer(album.Value);
                    album.Value.ToList().Sort(trackComparer);
                    for (var i = 0; i < album.Value.Count; i++)
                    {
                        try
                        {
                            album.Value[i].TrackNumber = i + 1;
                        }
                        catch (System.Runtime.InteropServices.COMException e)
                        {
                            if (e.Message.Contains(trackMissingErrorCode))
                            {
                                throw new iTunesAssistantException(trackMissingErrorMessage);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }

                if (albumWorkflows.Any(workflow => workflow.Name == WorkflowName.FixCountOfTracksOnAlbum))
                {
                    foreach (var track in album.Value)
                    {
                        try
                        {
                            track.TrackCount = album.Value.Count;
                        }
                        catch (System.Runtime.InteropServices.COMException e)
                        {
                            if (e.Message.Contains(trackMissingErrorCode))
                            {
                                throw new iTunesAssistantException(trackMissingErrorMessage);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }

                ItemsProcessed++;
            }
        }

        private void RunTrackWorkflows(IList<IITTrack> tracksToFix, IEnumerable<Workflow> trackWorkflows)
        {
            SetNewState(0, tracksToFix.Count, "Running track workflows...");

            foreach (var track in tracksToFix)
            {
                if (trackWorkflows.Any(workflow => workflow.Name == WorkflowName.SetAlbumNames))
                {
                    if (string.IsNullOrWhiteSpace(track.Album))
                    {
                        track.Album = track.Name;
                    }
                }

                if (trackWorkflows.Any(workflow => workflow.Name == WorkflowName.FindAndReplace))
                {
                    var findAndReplace = trackWorkflows.First(item => item.Name == WorkflowName.FindAndReplace);
                    if (!string.IsNullOrEmpty(findAndReplace.OldValue))
                    {
                        track.Name = track.Name.Replace(findAndReplace.OldValue, findAndReplace.NewValue);
                    }
                }

                if (trackWorkflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNames))
                {
                    track.Name = TrackNameFixer.FixTrackName(track.Name);
                }

                if (trackWorkflows.Any(workflow => workflow.Name == WorkflowName.FixGratefulDeadTracks))
                {
                    track.Name = GratefulDeadTrackNameFixer.FixTrackName(track.Name);
                    if (string.IsNullOrWhiteSpace(track.Comment))
                    {
                        throw new iTunesAssistantException("One or more Grateful Dead tracks is missing a comment");
                    }
                    else
                    {
                        track.Comment = track.Comment.Replace("https://archive.org/details/", string.Empty);
                    }
                }

                ItemsProcessed++;
            }
        }

        private IDictionary<string, IList<IITTrack>> GetAlbums(IList<IITTrack> tracksToFix)
        {
            SetNewState(0, tracksToFix.Count, "Generating album list...");

            var albums = new SortedDictionary<string, IList<IITTrack>>();

            foreach (var track in tracksToFix)
            {
                if (string.IsNullOrWhiteSpace(track.Album))
                {
                    throw new iTunesAssistantException("One or more tracks is missing an album name");
                }

                if (!albums.ContainsKey(track.Album))
                {
                    albums.Add(track.Album, new List<IITTrack> { track });
                }
                else
                {
                    albums[track.Album].Add(track);
                }

                ItemsProcessed++;
            }

            return albums;
        }
    }
}
