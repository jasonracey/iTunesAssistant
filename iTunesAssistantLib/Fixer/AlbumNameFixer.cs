using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public static class AlbumNameFixer
    {
        private static readonly Regex DiscNumberRegex = new Regex(@"\s*\[*\(*(disc|cd)\s*\d*\)*\]*\s*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string FixAlbumName(this string s)
        {
            return s.RemoveDiscNumber().RemoveDoubleSpaces().Trim().ToTitleCase();
        }

        private static string RemoveDiscNumber(this string s)
        {
            return DiscNumberRegex.Replace(s, string.Empty);
        }
    }
}