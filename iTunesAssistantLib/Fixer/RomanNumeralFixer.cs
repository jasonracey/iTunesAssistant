using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public static class RomanNumeralFixer
    {
        private static readonly Regex RomanNumeralRegex = new Regex(@"\b(X|XX|XXX|XL|L|LX|LXX|LXXX|XC|C)?(I|II|III|IV|V|VI|VII|VIII|IX)?\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string FixRomanNumerals(this string s)
        {
            return RomanNumeralRegex.Replace(s, match => match.Value.ToUpperInvariant());
        }
    }
}
