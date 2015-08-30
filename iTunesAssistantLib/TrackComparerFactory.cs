using System.Collections.Generic;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class TrackComparerFactory
    {
        public static IComparer<IITTrack> GetTrackComparer(List<IITTrack> tracks)
        {
            var trackNumbers = new Dictionary<int, int>();

            foreach (var track in tracks)
            {
                if (track.TrackNumber != 0 && !trackNumbers.ContainsKey(track.TrackNumber))
                {
                    trackNumbers.Add(track.TrackNumber, track.TrackNumber);
                }
                else
                {
                    return new TrackNameComparer();
                }
            }

            return new TrackNumberComparer();
        }
    }

    public class TrackNameComparer : IComparer<IITTrack>
    {
        public int Compare(IITTrack x, IITTrack y)
        {
            return string.CompareOrdinal(x.Name, y.Name);
        }
    }

    public class TrackNumberComparer : IComparer<IITTrack>
    {
        public int Compare(IITTrack x, IITTrack y)
        {
            return x.TrackNumber.CompareTo(y.TrackNumber);
        }
    }
}