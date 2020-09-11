using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WhenUsingVinylTrackNumberRegex
    {
        [TestMethod]
        public void RemovesSingleDigitTrackNumbers()
        {
            TrackNameFixer
                .FixTrackName("A-1-foo")
                .Should().Be("Foo");
        }

        [TestMethod]
        public void IgnoresCase()
        {
            TrackNameFixer
                .FixTrackName("a-1-foo")
                .Should().Be("Foo");
        }

        [TestMethod]
        public void IgnoresSpaces()
        {
            TrackNameFixer
                .FixTrackName("A - 1 - foo")
                .Should().Be("Foo");
        }

        [TestMethod]
        public void RemovesDoubleDigitTrackNumbers()
        {
            TrackNameFixer
                .FixTrackName("A-10-foo")
                .Should().Be("Foo");
        }
    }
}