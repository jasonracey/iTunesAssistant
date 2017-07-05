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

        private static readonly Regex TrackNumberRegex = new Regex(@"^(\d+\s*[-.]*\s*)", RegexOptions.Compiled);
        private static readonly Regex VinylTrackNumberRegex = new Regex(@"^(\w\s*[-.]*\s*\d+\s*[-.]*\s*)", RegexOptions.Compiled);

        public static string FixTrackName(string trackName)
        {
            // run more specific first
            foreach (var label in Labels)
            {
                var regex = new Regex($@"^(\d*\s*\-?\s*{label}\s*\-?\s*\d*\s*\-?\s*)");
                trackName = regex.Replace(trackName, string.Empty);
            }
            trackName = VinylTrackNumberRegex.Replace(trackName, string.Empty);
            trackName = TrackNumberRegex.Replace(trackName, string.Empty);

            trackName = trackName.RepeatedlyReplace("  ", " ");
            trackName = trackName.Trim();
            return trackName.ToTitleCase();
        }
    }
}