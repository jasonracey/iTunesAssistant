using System;

namespace iTunesAssistantLib
{
    public static class RomanNumeralFixer
    {
        public static string FixRomanNumerals(this string s)
        {
            s = s.Replace("Ii.", "II.");
            s = s.Replace("Iii.", "III.");
            s = s.Replace("Iv.", "IV.");
            return s;
        }
    }
}
