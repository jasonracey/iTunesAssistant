using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public class TrackNameFixer
    {
        private static readonly List<string> Labels = new List<string>
        {
            "Track"
        };

        private static readonly Regex GratefulDeadTrackNumberRegex = new Regex(@"^gd\d+\s?t?\d+\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex TrackNumberRegex = new Regex(@"^(\d+\s*[-.]*\s*)", RegexOptions.Compiled);
        private static readonly Regex VinylTrackNumberRegex = new Regex(@"^(\w\s*[-.]*\s*\d+\s*[-.]*\s*)", RegexOptions.Compiled);

        public static string FixTrackName(string trackName)
        {
            // run more specific first
            foreach (var label in Labels)
            {
                // todo: what is this doing? what is a track label?
                var trackLabelRegex = new Regex($@"^(\d*\s*\-?\s*{label}\s*\-?\s*\d*\s*\-?\s*)");
                trackName = trackLabelRegex.Replace(trackName, string.Empty);
            }
            trackName = GratefulDeadTrackNumberRegex.Replace(trackName, string.Empty);
            trackName = VinylTrackNumberRegex.Replace(trackName, string.Empty);
            trackName = TrackNumberRegex.Replace(trackName, string.Empty);

            trackName = trackName.RepeatedlyReplace("  ", " ");
            trackName = trackName.Trim();
            trackName = trackName.ToTitleCase(); // this messes up Roman numerals
            trackName = RomanNumeralFixer.FixRomanNumerals(trackName);

            return trackName;
        }
    }
}