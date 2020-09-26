using System.Collections.Generic;
using iTunesLib;

namespace iTunesAssistantLib
{
    public static class TrackComparerFactory
    {
        public static IComparer<IITTrack> GetTrackComparer(IEnumerable<IITTrack> tracks)
        {
            var trackNumbers = new HashSet<string>();

            foreach (var track in tracks)
            {
                var key = track.GetKey();
                if (track.TrackNumber != 0 && !trackNumbers.Contains(key))
                {
                    trackNumbers.Add(key);
                }
                else
                {
                    return new TrackNameComparer();
                }
            }

            return new TrackDiscAndNumberComparer();
        }
    }

    public class TrackNameComparer : IComparer<IITTrack>
    {
        public int Compare(IITTrack? t1, IITTrack? t2)
        {
            return string.CompareOrdinal(t1?.Name, t2?.Name);
        }
    }

    public class TrackDiscAndNumberComparer : IComparer<IITTrack>
    {
        public int Compare(IITTrack? t1, IITTrack? t2)
        {
            return t1?.GetKey()?.CompareTo(t2?.GetKey()) ?? 0;
        }
    }

    public static class TrackKey
    {
        public static string GetKey(this IITTrack track)
        {
            return $"{track.DiscNumber:D3}-{track.TrackNumber:D3}";
        }
    }
}