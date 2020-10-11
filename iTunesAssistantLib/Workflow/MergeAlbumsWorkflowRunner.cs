using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class MergeAlbumsWorkflowRunner : IWorkflowRunner
    {
        public void Run(IWorkflowData workflowData, ref Status status)
        {
            status = Status.Create(workflowData.Tracks.Count, "Generating album groups...");

            var albumGroups = new Dictionary<string, IDictionary<string, IList<IITTrack>>>();

            foreach (var track in workflowData.Tracks)
            {
                var newAlbumName = AlbumNameFixer.FixAlbumName(track.Album);

                if (!albumGroups.ContainsKey(newAlbumName))
                {
                    albumGroups.Add(newAlbumName, new SortedDictionary<string, IList<IITTrack>>());
                    albumGroups[newAlbumName].Add(track.Album, new List<IITTrack> { track });
                }
                else if (!albumGroups[newAlbumName].ContainsKey(track.Album))
                {
                    albumGroups[newAlbumName].Add(track.Album, new List<IITTrack> { track });
                }
                else
                {
                    albumGroups[newAlbumName][track.Album].Add(track);
                }

                status.ItemProcessed();
            }

            foreach (var album in albumGroups.SelectMany(albumGroup => albumGroup.Value.Values))
            {
                var comparer = TrackComparerFactory.GetTrackComparer(album);
                album.ToList().Sort(comparer);
            }

            status = Status.Create(albumGroups.Count, "Running merge albums workflow...");

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

                status.ItemProcessed();
            }
        }
    }
}
