using System;
using System.Collections.Generic;
using iTunesLib;

namespace iTunesAssistantLib
{
    public static class TrackCountFixer
    {
        public static void FixTrackCounts(IList<IITTrack> tracks)
        {
            if (tracks == null) throw new ArgumentNullException(nameof(tracks));

            foreach (var track in tracks)
            {
                try
                {
                    track.TrackCount = tracks.Count;
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
