using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public class TrackNameFixer
    {
        private static readonly Regex TrackNumberRegex = new Regex(@"^(\d+\s*-*\s*)", RegexOptions.Compiled);

        public static string FixTrackName(string trackName)
        {
            trackName = TrackNumberRegex.Replace(trackName, string.Empty);
            trackName = trackName.ReplaceAll("  ", " ");
            trackName = trackName.Trim();
            return trackName.ToTitleCase();
        }
    }
}