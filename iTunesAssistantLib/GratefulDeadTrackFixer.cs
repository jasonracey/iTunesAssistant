using System;

namespace iTunesAssistantLib
{
    public static class GratefulDeadTrackFixer
    {
        public static string FixTrackName(string trackName)
        {
            trackName = trackName.Replace("/", string.Empty);
            trackName = trackName.Replace("-", string.Empty);
            trackName = trackName.Replace("!", string.Empty);

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
            var lower = trackName.ToLower();

            var endsWithSegue = lower.EndsWith(">");
            var endsWithJam = lower.EndsWith(" jam") || lower.EndsWith(" jam>") || lower.EndsWith(" jam >");

            if (endsWithSegue)
            {
                trackName = trackName.Replace(">", string.Empty);
            }
            if (endsWithJam)
            {
                trackName = trackName.Replace(" Jam", string.Empty).Replace(" jam", string.Empty);
            }

            if (lower.Contains("the rub"))
            {
                trackName = "Ain't It Crazy (The Rub)";
            }
            else if (lower.Contains("aligator"))
            {
                trackName = "Alligator";
            }
            else if (lower.Contains("around"))
            {
                trackName = "Around And Around";
            }
            else if (lower.Contains("biodtl"))
            {
                trackName = "Beat It On Down The Line";
            }
            else if (lower.Contains("bill graham"))
            {
                trackName = "Bill Graham";
            }
            else if (lower.Contains("throated"))
            {
                trackName = "Black-Throated Wind";
            }
            else if (lower.Contains("eyed women"))
            {
                trackName = "Brown-Eyed Women";
            }
            else if (lower.Contains("c. c."))
            {
                trackName = "C.C. Rider";
            }
            else if (lower.Contains("candyman"))
            {
                trackName = "Candyman";
            }
            else if (lower.Contains("cold rain"))
            {
                trackName = "Cold Rain And Snow";
            }
            else if (lower.Contains("crowd noise"))
            {
                trackName = "Crowd";
            }
            else if (lower.Contains("cryptical"))
            {
                trackName = "Cryptical Envelopment";
            }
            else if (lower.Contains("dancin"))
            {
                trackName = "Dancing in the Street";
            }
            else if (lower.Contains("dont ease"))
            {
                trackName = "Don't Ease Me In";
            }
            else if (lower.Contains("duprees"))
            {
                trackName = "Dupree's Diamond Blues";
            }
            else if (lower.Contains("easy tolove"))
            {
                trackName = "Easy To Love You";
            }
            else if (lower.Contains("equipment issues"))
            {
                trackName = "Equipment Problems";
            }
            else if (lower.Contains("estimated"))
            {
                trackName = "Estimated Prophet";
            }
            else if (lower.Contains("franklins"))
            {
                trackName = "Franklin's Tower";
            }
            else if (lower.Contains("frozen logger"))
            {
                trackName = "The Frozen Logger";
            }
            else if (lower.Contains("gdtrfb") || lower.Contains("raod feelin"))
            {
                trackName = "Goin' Down The Road Feeling Bad";
            }
            else if (lower.Contains("good lovin"))
            {
                trackName = "Good Lovin'";
            }
            else if (lower.Contains("mojo work"))
            {
                trackName = "Got My Mojo Working";
            }
            else if (lower.Contains("your rider"))
            {
                trackName = "I Know You Rider";
            }
            else if (lower.Contains("mans world") || lower.Contains("man's world"))
            {
                trackName = "It's A Man's, Man's, Man's World";
            }
            else if (lower.Contains("baby blue"))
            {
                trackName = "It's All Over Now, Baby Blue";
            }
            else if (lower.Contains("jack a roe") || lower.Contains("jackaroe"))
            {
                trackName = "Jack-A-Roe";
            }
            else if (lower.Contains("johnny"))
            {
                trackName = "Johnny B. Goode";
            }
            else if (lower.Contains("kingbee"))
            {
                trackName = "I'm A King Bee";
            }
            else if (lower.Contains("lazy lightning"))
            {
                trackName = "Lazy Lightnin'";
            }
            else if (lower.Contains("women are smarter"))
            {
                trackName = "Man Smart (Woman Smarter)";
            }
            else if (lower.Contains("bobby mcgee"))
            {
                trackName = "Me And Bobby McGee";
            }
            else if (lower.Contains("my uncle"))
            {
                trackName = "Me And My Uncle";
            }
            else if (lower.Contains("mexical") || lower.Contains("mexacal"))
            {
                trackName = "Mexicali Blues";
            }
            else if (lower.Contains("minglewood"))
            {
                trackName = "New Minglewood Blues";
            }
            else if (lower.Contains("mississippi") || lower.Contains("halfstep"))
            {
                trackName = "Mississippi Half-Step, Uptown Toodeloo";
            }
            else if (lower.Contains("new speedway"))
            {
                trackName = "New Speedway Boogie";
            }
            else if (lower.Contains("ollin arageed"))
            {
                trackName = "Ollin Arrageed";
            }
            else if (lower.Contains("in the band"))
            {
                trackName = "Playing In The Band";
            }
            else if (lower.Contains("promised land"))
            {
                trackName = "The Promised Land";
            }
            else if (lower.Contains("ramble"))
            {
                trackName = "Ramble On Rose";
            }
            else if (lower.Contains("slipknot"))
            {
                trackName = "Slipknot!";
            }
            else if (lower.Contains("stephen"))
            {
                trackName = "St. Stephen";
            }
            else if (lower.Contains("announcements"))
            {
                trackName = "Stage Announcements";
            }
            else if (
                lower.Contains(" stage") ||
                lower.Contains("stage ") ||
                lower.Equals("stage"))
            {
                trackName = "Stage Banter";
            }
            else if (lower.Contains("peggy o") || lower.Contains("peggyo"))
            {
                trackName = "Peggy-O";
            }
            else if (lower.Contains("mcfall"))
            {
                trackName = "Rosalie McFall";
            }
            else if (lower.Contains("jimmy row"))
            {
                trackName = "Row Jimmy";
            }
            else if (lower.Contains("samson"))
            {
                trackName = "Samson And Delilah";
            }
            else if (lower.Contains("silver theads"))
            {
                trackName = "Silver Threads And Golden Needles";
            }
            else if (lower.Contains("forever tuning"))
            {
                trackName = "Stars And Stripes Forever";
            }
            else if (lower.Contains("sugaee"))
            {
                trackName = "Sugaree";
            }
            else if (lower.Contains("sweet chariot"))
            {
                trackName = "Swing Low, Sweet Chariot";
            }
            else if (lower.Contains("tennessee fred") || lower.Contains("tennesee") || lower.Contains("tennesse"))
            {
                trackName = "Tennessee Jed";
            }
            else if (
                lower.Contains("for the other one") ||
                lower.Contains("for the the other one"))
            {
                trackName = "That's It For The Other One";
            }
            else if (
                lower.Contains("other one") ||
                lower.Contains("otherone"))
            {
                trackName = "The Other One";
            }
            else if (lower.Contains("truckin"))
            {
                trackName = "Truckin'";
            }
            else if (lower.Contains("tuning"))
            {
                trackName = "Tuning";
            }
            else if (lower.Contains("lovelight"))
            {
                trackName = "Turn On Your Love Light";
            }
            else if (
                lower.Contains("u. s. blues") ||
                lower.Contains("u.s.blues") ||
                lower.Contains("us blues"))
            {
                trackName = "U.S. Blues";
            }
            else if (lower.Contains("wake up"))
            {
                trackName = "Wake Up Little Susie";
            }
            else if (lower.Contains("werewolves in london"))
            {
                trackName = "Werewolves Of London";
            }
            else if (lower.Contains("warf rat"))
            {
                trackName = "Wharf Rat";
            }

            trackName = trackName.Replace("*", string.Empty);
            trackName = trackName.Replace("(Encore)", string.Empty);

            trackName = trackName.TrimEnd();

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

        private static string ReplaceAll(string trackName, string pattern, string substitution)
        {
            while (trackName.Contains(pattern))
            {
                trackName = trackName.Replace(pattern, substitution);
            }
            return trackName;
        }
    }
}
