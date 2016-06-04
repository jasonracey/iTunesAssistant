using FluentAssertions;
using iTunesAssistantLib;
using NUnit.Framework;

namespace iTunesAssistantLibTest
{
    [TestFixture]
    public class WhenUsingVinylTrackNumberRegex
    {
        [Test]
        public void RemovesSingleDigitTrackNumbers()
        {
            TrackNameFixer
                .FixTrackName("A-1-foo")
                .Should().Be("Foo");
        }

        [Test]
        public void IgnoresCase()
        {
            TrackNameFixer
                .FixTrackName("a-1-foo")
                .Should().Be("Foo");
        }

        [Test]
        public void IgnoresSpaces()
        {
            TrackNameFixer
                .FixTrackName("A - 1 - foo")
                .Should().Be("Foo");
        }

        [Test]
        public void RemovesDoubleDigitTrackNumbers()
        {
            TrackNameFixer
                .FixTrackName("A-10-foo")
                .Should().Be("Foo");
        }
    }
}