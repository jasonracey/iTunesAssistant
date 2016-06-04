using System.Globalization;

namespace iTunesAssistantLib
{
    public static class TitleCaser
    {
        public static string ToTitleCase(this string s)
        {
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(textInfo.ToLower(s));
        }
    }
}
