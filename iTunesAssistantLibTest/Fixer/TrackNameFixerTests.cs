using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class TrackNameFixerTests
    {
        [DataTestMethod]
        [DataRow(" alligator jam", "Alligator Jam")]
        [DataRow("alligator jam ", "Alligator Jam")]
        [DataRow(" alligator jam ", "Alligator Jam")]
        public void TrimsWhiteSpace(string before, string after)
        {
            TrackNameFixer.FixTrackName(before).Should().Be(after);
        }

        [TestMethod]
        public void CollapsesInternalSpaces()
        {
            TrackNameFixer.FixTrackName("alligator      jam").Should().Be("Alligator Jam");
        }

        [TestMethod]
        public void ConvertsToTitleCase()
        {
            TrackNameFixer.FixTrackName("aLlIgAtOr jAm").Should().Be("Alligator Jam");
        }

        [TestMethod]
        public void FixesRomanNumerals()
        {
            TrackNameFixer.FixTrackName("iii. allegro").Should().Be("III. Allegro");
        }
    }

    [TestClass]
    public class TrackNumberRegexTests
    {
        [DataTestMethod]
        [DataRow("riot in cell block 9", "riot in cell block 9")]
        [DataRow("01riot in cell block 9", "riot in cell block 9")]
        [DataRow("01  riot in cell block 9", "riot in cell block 9")]
        [DataRow("01-riot in cell block 9", "riot in cell block 9")]
        [DataRow("01 - riot in cell block 9", "riot in cell block 9")]
        [DataRow("01 -- riot in cell block 9", "riot in cell block 9")]
        [DataRow("01.riot in cell block 9", "riot in cell block 9")]
        [DataRow("01 . riot in cell block 9", "riot in cell block 9")]
        [DataRow("01 .. riot in cell block 9", "riot in cell block 9")]
        public void ReturnsExpectedString(string before, string after)
        {
            TrackNameFixer.TrackNumberRegex
                .Replace(before, string.Empty)
                .Should()
                .Be(after);
        }
    }

    [TestClass]
    public class VinylTrackNumberRegexTests
    {
        [DataTestMethod]
        [DataRow("riot in cell block 9", "riot in cell block 9")]
        [DataRow("a-1-riot in cell block 9", "riot in cell block 9")]
        [DataRow("a - 1 - riot in cell block 9", "riot in cell block 9")]
        [DataRow("A-1-riot in cell block 9", "riot in cell block 9")]
        [DataRow("a-10-riot in cell block 9", "riot in cell block 9")]
        [DataRow("a - 10 - riot in cell block 9", "riot in cell block 9")]
        [DataRow("A-10-riot in cell block 9", "riot in cell block 9")]
        [DataRow("A - 10 - riot in cell block 9", "riot in cell block 9")]
        public void ReturnsExpectedString(string before, string after)
        {
            TrackNameFixer.VinylTrackNumberRegex
                .Replace(before, string.Empty)
                .Should()
                .Be(after);
        }
    }

    [TestClass]
    public class GratefulDeadTrackNumberRegexTests
    {
        [DataTestMethod]
        [DataRow("riot in cell block 9", "riot in cell block 9")]
        [DataRow("gdriot in cell block 9", "gdriot in cell block 9")]
        [DataRow("g riot in cell block 9", "g riot in cell block 9")]
        [DataRow("gd riot in cell block 9", "riot in cell block 9")]
        [DataRow("gd1 riot in cell block 9", "riot in cell block 9")]
        [DataRow("gd1  riot in cell block 9", "riot in cell block 9")]
        [DataRow("gd11 riot in cell block 9", "riot in cell block 9")]
        [DataRow("gd1t riot in cell block 9", "riot in cell block 9")]
        [DataRow("gd11t riot in cell block 9", "riot in cell block 9")]
        [DataRow("gd1t1 riot in cell block 9", "riot in cell block 9")]
        [DataRow("gd11t1 riot in cell block 9", "riot in cell block 9")]
        [DataRow("gd1t11 riot in cell block 9", "riot in cell block 9")]
        [DataRow("gd11t11 riot in cell block 9", "riot in cell block 9")]
        public void ReturnsExpectedString(string before, string after)
        {
            TrackNameFixer.GratefulDeadTrackNumberRegex
                .Replace(before, string.Empty)
                .Should()
                .Be(after);
        }
    }

    [TestClass]
    public class LabeledTrackNumberRegexTests
    {
        private readonly Regex trackLabelRegex = TrackNameFixer.GetTrackLabelRegex("track");

        [DataTestMethod]
        [DataRow("riot in cell block 9", "riot in cell block 9")]
        [DataRow("tRaCk10riot in cell block 9", "riot in cell block 9")]
        [DataRow("tRaCk10-riot in cell block 9", "riot in cell block 9")]
        [DataRow("tRaCk10 riot in cell block 9", "riot in cell block 9")]
        [DataRow("tRaCk10 - riot in cell block 9", "riot in cell block 9")]
        [DataRow("tRaCk 10riot in cell block 9", "riot in cell block 9")]
        [DataRow("tRaCk 10-riot in cell block 9", "riot in cell block 9")]
        [DataRow("tRaCk 10 riot in cell block 9", "riot in cell block 9")]
        [DataRow("tRaCk 10 - riot in cell block 9", "riot in cell block 9")]
        public void ReturnsExpectedString(string before, string after)
        {
            trackLabelRegex
                .Replace(before, string.Empty)
                .Should()
                .Be(after);
        }
    }
}
