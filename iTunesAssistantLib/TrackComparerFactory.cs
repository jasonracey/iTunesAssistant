using System.Collections.Generic;
using iTunesLib;

namespace iTunesAssistantLib
{
    public class TrackComparerFactory
    {
        public static IComparer<IITTrack> GetTrackComparer(List<IITTrack> tracks)
        {
            var trackNumbers = new Dictionary<string, int>();

            foreach (var track in tracks)
            {
                var key = track.GetKey();
                if (track.TrackNumber != 0 && !trackNumbers.ContainsKey(key))
                {
                    trackNumbers.Add(key, track.TrackNumber);
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
        public int Compare(IITTrack t1, IITTrack t2)
        {
            return string.CompareOrdinal(t1.Name, t2.Name);
        }
    }

    public class TrackDiscAndNumberComparer : IComparer<IITTrack>
    {
        public int Compare(IITTrack t1, IITTrack t2)
        {
            return t1.GetKey().CompareTo(t2.GetKey());
        }
    }

    public static class TrackKey
    {
        public static string GetKey(this IITTrack track)
        {
            var discNumberString = GetPaddedNumberString(track.DiscNumber);
            var trackNumberString = GetPaddedNumberString(track.TrackNumber);
            return $"{discNumberString}-{trackNumberString}";
        }

        private static string GetPaddedNumberString(int number)
        {
            // assumes range of 1-99
            return number < 10
                ? $"0{number}"
                : number.ToString();
        }
    }
}