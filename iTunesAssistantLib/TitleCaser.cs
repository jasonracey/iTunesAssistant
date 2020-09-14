using System.Globalization;

namespace iTunesAssistantLib
{
    public static class TitleCaser
    {
        private static readonly TextInfo TextInfoEnUs = new CultureInfo("en-US", false).TextInfo;

        public static string ToTitleCase(this string s)
        {
            // because Roman numerals are modified by ToTitleCase
            return RomanNumeralFixer.FixRomanNumerals(TextInfoEnUs.ToTitleCase(TextInfoEnUs.ToLower(s)));
        }
    }
}