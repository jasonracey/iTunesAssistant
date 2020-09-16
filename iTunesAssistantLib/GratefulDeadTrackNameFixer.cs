using System;

namespace iTunesAssistantLib
{
    public static class GratefulDeadTrackNameFixer
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
            // so we don't have to do this for each conditional below
            trackName = trackName.TrimEnd();

            var endsWithSegue = trackName.EndsWith(">");
            if (endsWithSegue)
            {
                trackName = trackName.Substring(0, trackName.Length - 1).TrimEnd();
            }

            if (trackName.EndsWith("-"))
            {
                trackName = trackName.Substring(0, trackName.Length - 1).TrimEnd();
            }

            var endsWithJam = trackName.ToLower().EndsWith(" jam");
            if (endsWithJam)
            {
                trackName = trackName.Substring(0, trackName.Length - 4);
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
