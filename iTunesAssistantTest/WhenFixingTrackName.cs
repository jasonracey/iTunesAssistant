using FluentAssertions;
using iTunesAssistantLib;
using NUnit.Framework;

namespace iTunesAssistantLibTest
{
    [TestFixture]
    public class WhenFixingTrackName
    {
        [Test]
        public void RemovesSingleDigitWithSpace()
        {
            TrackNameFixer
                .FixTrackName("1 Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesSingleDigitWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("1Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitWithSpace()
        {
            TrackNameFixer
                .FixTrackName("01 Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("01Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesSingleDigitDashWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("1-Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesSingleDigitDashWithSpace()
        {
            TrackNameFixer
                .FixTrackName("1 - Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesSingleDigitDashWithLeadingSpace()
        {
            TrackNameFixer
                .FixTrackName("1- Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesSingleDigitDashWithTrailingSpace()
        {
            TrackNameFixer
                .FixTrackName("1 -Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitDashWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("01-Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitDashWithSpace()
        {
            TrackNameFixer
                .FixTrackName("01 - Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitDashWithLeadingSpace()
        {
            TrackNameFixer
                .FixTrackName("01- Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitDashWithTrailingSpace()
        {
            TrackNameFixer
                .FixTrackName("01 -Foo9")
                .Should().Be("Foo9");
        }

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
