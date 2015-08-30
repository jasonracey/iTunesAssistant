using System.Text.RegularExpressions;

namespace iTunesAssistantLib
{
    public class AlbumNameFixer
    {
        public static string FixAlbumName(string albumName)
        {
            var albumNameRegex = new Regex(@"\s*\[*\(*(disc|cd)\s*\d*\)*\]*\s*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            albumName = albumNameRegex.Replace(albumName, string.Empty);

            while (albumName.Contains("  "))
            {
                albumName = albumName.Replace("  ", " ");
            }

            albumName = albumName.Trim();

            return albumName.ToTitleCase();
        }
    }
}