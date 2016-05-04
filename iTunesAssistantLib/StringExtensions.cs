namespace iTunesAssistantLib
{
    public static class StringExtensions
    {
        public static string ReplaceAll(this string str, string match, string substitution)
        {
            while (str.Contains(match))
            {
                str = str.Replace(match, substitution);
            }
            return str;
        }
    }
}
