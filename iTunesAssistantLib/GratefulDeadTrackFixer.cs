using System;

namespace iTunesAssistantLib
{
    public static class GratefulDeadTrackFixer
    {
        public static string FixTrackName(string trackName)
        {
            var trackNames = trackName.Split(new[] {">"}, StringSplitOptions.RemoveEmptyEntries);

            if (trackNames.Length == 1)
            {
                trackName = GetCanonicalName(trackName);
            }
            else
            {
                var tempName = string.Empty;
                for (var i = 0; i < trackNames.Length; i++)
                {
                    var currentTrack = trackNames[i].Trim();
                    if (i < trackNames.Length - 1)
                    {
                        tempName += GetCanonicalName(currentTrack + " >") + " ";
                    }
                    else
                    {
                        if (trackName.EndsWith(">"))
                        {
                            currentTrack += " >";
                        }
                        tempName += GetCanonicalName(currentTrack);
                    }
                }
                trackName = tempName;
            }

            return trackName;
        }

        private static string GetCanonicalName(string trackName)
        {
            var endsWithSegue = trackName.TrimEnd().EndsWith(">");
            if (endsWithSegue)
            {
                trackName = trackName.Replace(">", string.Empty);
            }

            var endsWithJam = trackName.TrimEnd().ToLower().EndsWith(" jam");
            if (endsWithJam)
            {
                trackName = trackName.Replace(" Jam", string.Empty).Replace(" jam", string.Empty);
            }

            var result = GratefulDeadTrackNameData.Search(trackName);

            trackName = string.IsNullOrWhiteSpace(result)
                ? "***** UNKNOWN - " + trackName
                : result;

            if (endsWithJam)
            {
                trackName += " Jam";
            }

            if (endsWithSegue)
            {
                trackName += " >";
            }

            return trackName;
        }
    }
}
