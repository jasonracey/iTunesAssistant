using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public static class DiscNumberRemover
    {
        private static readonly Regex DiscNumberRegex = new Regex(@"\s*\[*\(*(disc|cd)\s*\d*\)*\]*\s*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string RemoveDiscNumber(this string s)
        {
            return DiscNumberRegex.Replace(s, string.Empty);
        }
    }
}