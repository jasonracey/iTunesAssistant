using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public static class StringExtensions
    {
        public static string RepeatedlyReplace(this string str, string match, string substitution)
        {
            while (str.Contains(match))
            {
                str = str.Replace(match, substitution);
            }
            return str;
        }

        public static string ToLowerAlphaNumeric(this string str)
        {
            const string pattern = "[^a-zA-Z0-9]";
            var replacement = string.Empty;
            return Regex.Replace(str, pattern, replacement, RegexOptions.Compiled).ToLower();
        }
    }
}
