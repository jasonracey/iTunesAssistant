using iTunesLib;
using System;
using System.Collections.Generic;

namespace iTunesAssistantLib
{
    public static class AlbumBuilder
    {
        public static IDictionary<string, IList<IITTrack>> BuildAlbums(ref Status status, IList<IITTrack> tracksToFix)
        {
            if (tracksToFix == null) throw new ArgumentNullException(nameof(tracksToFix));

            status = Status.Create(tracksToFix.Count, "Generating album list...");

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

                status.ItemProcessed();
            }

            return albums;
        }
    }
}
