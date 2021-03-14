using System;
using System.Collections.Generic;
using System.Linq;
using iTunesLib;

namespace iTunesAssistantLib
{
    public static class TrackNumberFixer
    {
        public static void FixTrackNumbers(IList<IITTrack> tracks)
        {
            if (tracks == null) throw new ArgumentNullException(nameof(tracks));

            var trackComparer = TrackComparerFactory.GetTrackComparer(tracks);

            var trackList = tracks.ToList();
            trackList.Sort(trackComparer);

            for (var i = 0; i < trackList.Count; i++)
            {
                try
                {
                    trackList[i].TrackNumber = i + 1;
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    if (e.Message.Contains(TrackMissingError.Code))
                    {
                        throw new iTunesAssistantException(TrackMissingError.Message);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
