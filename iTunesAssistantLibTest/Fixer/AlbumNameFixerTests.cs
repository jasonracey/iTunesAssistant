using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class AlbumNameFixerTests
    {
        [DataTestMethod]
        [DataRow("In Utero Disc1", "In Utero")]
        public void RemovesDiscAndNumberFromName(string before, string after)
        {
            AlbumNameFixer.FixAlbumName(before).Should().Be(after);
        }

        [DataTestMethod]
        [DataRow("In Utero CD1", "In Utero")]
        public void RemovesCdAndNumberFromName(string before, string after)
        {
            AlbumNameFixer.FixAlbumName(before).Should().Be(after);
        }

        [TestMethod]
        public void Trims()
        {
            AlbumNameFixer
                .FixAlbumName("    Supernaut           ")
                .Should()
                .Be("Supernaut");
        }

        [TestMethod]
        public void RemovesDoubleSpaces()
        {
            AlbumNameFixer
                .FixAlbumName("My  Cool   Album")
                .Should()
                .Be("My Cool Album");
        }

        [TestMethod]
        public void ConvertsToTitleCase()
        {
            AlbumNameFixer
                .FixAlbumName("tOwEr")
                .Should()
                .Be("Tower");
        }

        [TestMethod]
        public void FixesRomanNumerals()
        {
            AlbumNameFixer
                .FixAlbumName("Symphony i, Symphony ii, Symphony iii")
                .Should()
                .Be("Symphony I, Symphony II, Symphony III");
        }

        [TestMethod]
        public void FixesRegionAbbreviations()
        {
            AlbumNameFixer
                .FixAlbumName("2021-11-26 Issaquah Arena, Issaquah, wa")
                .Should()
                .Be("2021-11-26 Issaquah Arena, Issaquah, WA");
        }
    }
}
