using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public class TrackNameFixer
    {
        private static readonly IEnumerable<string> Labels = new HashSet<string> { "track" };

        /// <summary>
        /// Matches track number labels commonly found on shared music files.
        /// </summary>
        public static readonly Regex TrackNumberRegex = new Regex(@"^(\d+\s*[-.]*\s*)", RegexOptions.Compiled);

        /// <summary>
        /// Matches track number labels commonly found on shared music files ripped from vinyl.
        /// </summary>
        public static readonly Regex VinylTrackNumberRegex = new Regex(@"^(\w\s*[-.]*\s*\d+\s*[-.]*\s*)", RegexOptions.Compiled);

        /// <summary>
        /// Matches track number labels commonly found on shared Grateful Dead music files.
        /// </summary>
        public static readonly Regex GratefulDeadTrackNumberRegex = new Regex(@"^gd\d*\s*t?\d*\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Returns a regex that matches track number labels where the specified string might be embedded inside any of variety of common formats.
        /// </summary>
        /// <param name="label">The label to embed.</param>
        /// <returns>A regex that matches track number labels where the specified string might be embedded inside any of variety of common formats.</returns>
        public static Regex GetTrackLabelRegex(string label)
        {
            return new Regex($@"^(\d*\s*\-?\s*{label}\s*\-?\s*\d*\s*\-?\s*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static string FixTrackName(string trackName)
        {
            // Run regexes in order of most specific to least.
            trackName = Labels
                .Select(label => GetTrackLabelRegex(label))
                .Concat(new[] { GratefulDeadTrackNumberRegex, VinylTrackNumberRegex, TrackNumberRegex })
                .Aggregate(trackName, (trackName, regex) => regex.Replace(trackName ?? string.Empty, string.Empty));

            trackName = trackName
                .RepeatedlyReplace(StringExtensions.DoubleSpace, StringExtensions.SingleSpace)
                .Trim()
                .ToTitleCase();

            return trackName;
        }
    }
}