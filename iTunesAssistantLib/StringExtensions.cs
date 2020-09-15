using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public static class StringExtensions
    {
        public const string DoubleSpace = "  ";
        public const string SingleSpace = " ";

        private const string PatternForAnyAlphaNumeric = "[^a-zA-Z0-9]";

        public static string RemoveDoubleSpaces(this string s)
        {
            return RepeatedlyReplace(s, DoubleSpace, SingleSpace);
        }

        public static string RepeatedlyReplace(this string s, string match, string substitution)
        {
            while (s.Contains(match))
            {
                s = s.Replace(match, substitution);
            }
            return s;
        }

        public static string ToLowerAlphaNumeric(this string s)
        {
            return Regex.Replace(
                input: s, 
                pattern: PatternForAnyAlphaNumeric, 
                replacement: string.Empty, 
                options: RegexOptions.Compiled).ToLower();
        }
    }
}
