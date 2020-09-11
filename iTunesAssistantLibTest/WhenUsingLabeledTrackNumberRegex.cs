using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WhenUsingLabeledTrackNumberRegex
    {
        [TestMethod]
        public void IgnoresWhiteSpace()
        {
            TrackNameFixer
                .FixTrackName("10 Track 10 foo")
                .Should().Be("Foo");
        }

        [TestMethod]
        public void RemovesLabelNumber()
        {
            TrackNameFixer
                .FixTrackName("Track10 foo")
                .Should().Be("Foo");
        }

        [TestMethod]
        public void RemovesNumberLabelNumber()
        {
            TrackNameFixer
                .FixTrackName("10Track10 foo")
                .Should().Be("Foo");
        }

        [TestMethod]
        public void RemovesNumberDashLabelDashNumberDash()
        {
            TrackNameFixer
                .FixTrackName("01 - Track - 01 - foo")
                .Should().Be("Foo");
        }
    }
}