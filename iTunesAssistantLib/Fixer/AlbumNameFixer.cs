namespace iTunesAssistantLib
{
    public static class AlbumNameFixer
    {
        public static string FixAlbumName(this string s)
        {
            return s
                .RemoveDiscNumber()
                .Trim()
                .RemoveDoubleSpaces()
                .ToTitleCase()
                .FixRomanNumerals()
                .FixRegionAbbreviation();
        }
    }
}