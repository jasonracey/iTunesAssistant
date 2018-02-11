using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class WorkflowRunner
    {
        private readonly iTunesAppClass _app;

        public WorkflowRunner()
        {
            _app = new iTunesAppClass();
        }

        public string State { get; private set; }
        public int ItemsProcessed { get; private set; }
        public int ItemsTotal { get; private set; }

        public void Run(HashSet<Workflow> workflows)
        {
            SetNewState(0, _app.LibraryPlaylist.Tracks.Count, "Loading tracks...");

            if (_app.SelectedTracks == null)
            {
                return;
            }

            var tracksToFix = new List<IITTrack>();

            foreach (IITTrack track in _app.SelectedTracks)
            {
                tracksToFix.Add(track);
                ItemsProcessed++;
            }

            if (tracksToFix.Count == 0)
            {
                return;
            }

            if (workflows.Any(item => item.Name == WorkflowName.MergeAlbums))
            {
                RunMergeAlbumsWorkflow(tracksToFix);
            }

            if (workflows.Any(item => item.Name == WorkflowName.ImportTrackNames))
            {
                var inputFilePath = workflows.First(item => item.Name == WorkflowName.ImportTrackNames).Data;
                RunImportTrackNamesWorkflow(tracksToFix, inputFilePath);
            }

            var albumWorkflows = workflows.Where(item => 
                item.Name == WorkflowName.FixCountOfTracksOnAlbum ||
                item.Name == WorkflowName.FixTrackNumbers).ToList();

            RunAlbumWorkflows(tracksToFix, albumWorkflows);

            var trackWorkflows = workflows.Where(item => 
                item.Name == WorkflowName.FixGratefulDeadTracks || 
                item.Name == WorkflowName.FixTrackNames ||
                item.Name == WorkflowName.SetAlbumNames).ToList();

            RunTrackWorkflows(tracksToFix, trackWorkflows);
        }

        private void SetNewState(int itemsProcessed, int itemsTotal, string state)
        {
            ItemsProcessed = itemsProcessed;
            ItemsTotal = itemsTotal;
            State = state;
        }

        private void RunMergeAlbumsWorkflow(IReadOnlyCollection<IITTrack> tracksToFix)
        {
            SetNewState(0, tracksToFix.Count, "Generating album groups...");

            var albumGroups = new Dictionary<string, SortedDictionary<string,List<IITTrack>>>();

            foreach (var track in tracksToFix)
            {
                var newAlbumName = AlbumNameFixer.FixAlbumName(track.Album);

                if (!albumGroups.ContainsKey(newAlbumName))
                {
                    albumGroups.Add(newAlbumName, new SortedDictionary<string, List<IITTrack>>());
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

            foreach (var albumGroup in albumGroups.Values)
            {
                foreach (var album in albumGroup.Values)
                {
                    var comparer = TrackComparerFactory.GetTrackComparer(album);
                    album.Sort(comparer);
                }
            }

            SetNewState(0, albumGroups.Count, "Running merge albums workflow...");

            foreach (var albumGroup in albumGroups)
            {
                var newAlbumName = albumGroup.Key;
                var trackCount = albumGroup.Value.Sum(album => album.Value.Count);
                var trackNumber = 1;

                foreach (var album in albumGroup.Value)
                {
                    foreach (IITTrack track in album.Value)
                    {
                        track.Album = newAlbumName;
                        track.DiscCount = 1;
                        track.DiscNumber = 1;

                        // have to set number before count in case old number is higher than count
                        track.TrackNumber = trackNumber++;
                        track.TrackCount = trackCount;
                    }
                }

                ItemsProcessed++;
            }
        }

        private void RunImportTrackNamesWorkflow(IReadOnlyCollection<IITTrack> tracksToFix, string inputFilePath)
        {
            SetNewState(0, tracksToFix.Count, "Reading track names to import...");

            var newTrackNames = System.IO.File.ReadAllLines(inputFilePath);
            
            var newTrackNameGroup = new List<string>();
            foreach (var newTrackName in newTrackNames)
            {
                var cleanName = newTrackName.Trim();
                if (cleanName != string.Empty)
                {
                    newTrackNameGroup.Add(newTrackName);
                }

                ItemsProcessed++;
            }

            var albums = GetAlbums(tracksToFix);

            SetNewState(0, tracksToFix.Count, "Assigning new track names...");

            for (var i = 0; i < albums.Count; i++)
            {
                var currentAlbum = albums.ElementAt(i).Value;

                for (var j = 0; j < currentAlbum.Count; j++)
                {
                    currentAlbum[j].Name = newTrackNameGroup[j];
                    ItemsProcessed++;
                }
            }

            return;
        }

        private void RunAlbumWorkflows(
            IReadOnlyCollection<IITTrack> tracksToFix,
            IReadOnlyCollection<Workflow> albumWorkflows)
        {
            if (albumWorkflows.Count == 0)
            {
                return;
            }

            var albums = GetAlbums(tracksToFix);

            SetNewState(0, albums.Count, "Running album workflows...");

            const string trackMissingErrorCode = "0xA0040203";
            const string trackMissingErrorMessage = "One or more tracks could not be found in your file system.";

            foreach (var album in albums)
            {
                // have to set number before count in case old number is higher than count
                if (albumWorkflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNumbers))
                {
                    var trackComparer = TrackComparerFactory.GetTrackComparer(album.Value);
                    album.Value.Sort(trackComparer);
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
                                throw new System.Exception(trackMissingErrorMessage);
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
                                throw new System.Exception(trackMissingErrorMessage);
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

        private void RunTrackWorkflows(
            IReadOnlyCollection<IITTrack> tracksToFix,
            IReadOnlyCollection<Workflow> trackWorkflows)
        {
            if (trackWorkflows.Count == 0)
            {
                return;
            }

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

                if (trackWorkflows.Any(workflow => workflow.Name == WorkflowName.FixTrackNames))
                {
                    track.Name = TrackNameFixer.FixTrackName(track.Name);
                }

                if (trackWorkflows.Any(workflow => workflow.Name == WorkflowName.FixGratefulDeadTracks))
                {
                    track.Name = GratefulDeadTrackFixer.FixTrackName(track.Name);
                    track.Comment = track.Comment.Replace("https://archive.org/details/", string.Empty);
                }

                ItemsProcessed++;
            }
        }

        private SortedDictionary<string, List<IITTrack>> GetAlbums(IReadOnlyCollection<IITTrack> tracksToFix)
        {
            SetNewState(0, tracksToFix.Count, "Generating album list...");

            var albums = new SortedDictionary<string, List<IITTrack>>();

            foreach (var track in tracksToFix)
            {
                if (string.IsNullOrWhiteSpace(track.Album))
                {
                    throw new System.Exception("One or more tracks is missing an album name.");
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
