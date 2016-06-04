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

        public void Run(List<Workflow> workflows)
        {
            NewState(0, _app.LibraryPlaylist.Tracks.Count, "Loading tracks...");

            var tracksToFix = new List<IITTrack>();

            foreach (IITTrack track in _app.LibraryPlaylist.Tracks)
            {
                if (track.Genre == "Fix")
                {
                    tracksToFix.Add(track);
                }
                ItemsProcessed++;
            }

            if (tracksToFix.Count == 0)
            {
                return;
            }

            var albumGroupWorkflows = workflows.Where(item => 
                item == Workflow.MergeAlbums).ToList();

            RunAlbumGroupWorkflows(tracksToFix, albumGroupWorkflows);

            var albumWorkflows = workflows.Where(item => 
                item == Workflow.FixCountOfTracksOnAlbum ||
                item == Workflow.FixTrackNumbers).ToList();

            RunAlbumWorkflows(tracksToFix, albumWorkflows);

            var trackWorkflows = workflows.Where(item => 
                item == Workflow.FixGratefulDeadTracks || 
                item == Workflow.FixTrackNames ||
                item == Workflow.SetAlbumNames).ToList();

            RunTrackWorkflows(tracksToFix, trackWorkflows);
        }

        private void NewState(int itemsProcessed, int itemsTotal, string state)
        {
            ItemsProcessed = itemsProcessed;
            ItemsTotal = itemsTotal;
            State = state;
        }

        private void RunAlbumGroupWorkflows(
            IReadOnlyCollection<IITTrack> tracksToFix,
            IReadOnlyCollection<Workflow> albumGroupWorkflows)
        {
            if (albumGroupWorkflows.Count == 0)
            {
                return;
            }

            NewState(0, tracksToFix.Count, "Generating album groups...");

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

            NewState(0, albumGroups.Count, "Running album group workflows...");

            foreach (var albumGroup in albumGroups)
            {
                var newAlbumName = albumGroup.Key;
                var trackCount = albumGroup.Value.Sum(x => x.Value.Count);
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

        private void RunAlbumWorkflows(
            IReadOnlyCollection<IITTrack> tracksToFix,
            IReadOnlyCollection<Workflow> albumWorkflows)
        {
            if (albumWorkflows.Count == 0)
            {
                return;
            }

            NewState(0, tracksToFix.Count, "Generating album list...");

            var albums = new Dictionary<string,List<IITTrack>>();

            foreach (var track in tracksToFix)
            {
                if (!albums.ContainsKey(track.Album))
                {
                    albums.Add(track.Album, new List<IITTrack> {track});
                }
                else
                {
                    albums[track.Album].Add(track);
                }

                ItemsProcessed++;
            }

            NewState(0, albums.Count, "Running album workflows...");

            foreach (var album in albums)
            {
                // have to set number before count in case old number is higher than count
                if (albumWorkflows.Contains(Workflow.FixTrackNumbers))
                {
                    var trackComparer = TrackComparerFactory.GetTrackComparer(album.Value);
                    album.Value.Sort(trackComparer);
                    for (var i = 0; i < album.Value.Count; i++)
                    {
                        album.Value[i].TrackNumber = i + 1;
                    }
                }

                if (albumWorkflows.Contains(Workflow.FixCountOfTracksOnAlbum))
                {
                    foreach (var track in album.Value)
                    {
                        track.TrackCount = album.Value.Count;
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

            NewState(0, tracksToFix.Count, "Running track workflows...");

            foreach (var track in tracksToFix)
            {
                if (trackWorkflows.Contains(Workflow.SetAlbumNames))
                {
                    if (string.IsNullOrWhiteSpace(track.Album))
                    {
                        track.Album = track.Name;
                    }
                }

                if (trackWorkflows.Contains(Workflow.FixTrackNames))
                {
                    track.Name = TrackNameFixer.FixTrackName(track.Name);
                }

                if (trackWorkflows.Contains(Workflow.FixGratefulDeadTracks))
                {
                    const string gratefulDead = "Grateful Dead";
                    if (track.Artist == gratefulDead)
                    {
                        track.Composer = gratefulDead;
                        track.Comment = track.Comment.Replace("https://archive.org/details/", string.Empty);
                        track.Name = GratefulDeadTrackFixer.FixTrackName(track.Name);
                    }
                }

                ItemsProcessed++;
            }
        }
    }
}
