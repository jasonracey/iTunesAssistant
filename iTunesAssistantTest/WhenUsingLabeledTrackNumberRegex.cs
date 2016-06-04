using FluentAssertions;
using iTunesAssistantLib;
using NUnit.Framework;

namespace iTunesAssistantLibTest
{
    [TestFixture]
    public class WhenUsingLabeledTrackNumberRegex
    {
        [Test]
        public void IgnoresWhiteSpace()
        {
            TrackNameFixer
                .FixTrackName("10 Track 10 foo")
                .Should().Be("Foo");
        }

        [Test]
        public void RemovesLabelNumber()
        {
            TrackNameFixer
                .FixTrackName("Track10 foo")
                .Should().Be("Foo");
        }

        [Test]
        public void RemovesNumberLabelNumber()
        {
            TrackNameFixer
                .FixTrackName("10Track10 foo")
                .Should().Be("Foo");
        }

        [Test]
        public void RemovesNumberDashLabelDashNumberDash()
        {
            TrackNameFixer
                .FixTrackName("01 - Track - 01 - foo")
                .Should().Be("Foo");
        }
    }
}