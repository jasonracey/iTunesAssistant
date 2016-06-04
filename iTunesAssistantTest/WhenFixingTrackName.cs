using FluentAssertions;
using iTunesAssistantLib;
using NUnit.Framework;

namespace iTunesAssistantLibTest
{
    public class WhenFixingTrackName
    {
        [Test]
        public void TrimsWhiteSpace()
        {
            TrackNameFixer
                .FixTrackName(" Foo ")
                .Should().Be("Foo");
        }

        [Test]
        public void RemovesDoubleSpace()
        {
            TrackNameFixer
                .FixTrackName("Foo  Bar")
                .Should().Be("Foo Bar");
        }

        [Test]
        public void ConvertsToTitleCase()
        {
            TrackNameFixer
                .FixTrackName("fOo BaR")
                .Should().Be("Foo Bar");
        }
    }
}
