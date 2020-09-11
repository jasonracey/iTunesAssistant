using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WhenFixingTrackName
    {
        [TestMethod]
        public void TrimsWhiteSpace()
        {
            TrackNameFixer
                .FixTrackName(" Foo ")
                .Should().Be("Foo");
        }

        [TestMethod]
        public void RemovesDoubleSpace()
        {
            TrackNameFixer
                .FixTrackName("Foo  Bar")
                .Should().Be("Foo Bar");
        }

        [TestMethod]
        public void ConvertsToTitleCase()
        {
            TrackNameFixer
                .FixTrackName("fOo BaR")
                .Should().Be("Foo Bar");
        }
    }
}
