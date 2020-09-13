using System.Globalization;

namespace iTunesAssistantLib
{
    public static class TitleCaser
    {
        private static readonly TextInfo TextInfoEnUs = new CultureInfo("en-US", false).TextInfo;

        public static string ToTitleCase(this string s)
        {
            return TextInfoEnUs.ToTitleCase(TextInfoEnUs.ToLower(s));
        }
    }
}
