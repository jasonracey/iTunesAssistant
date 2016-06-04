using FluentAssertions;
using iTunesAssistantLib;
using NUnit.Framework;

namespace iTunesAssistantLibTest
{
    [TestFixture]
    public class WhenUsingTrackNumberRegex
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
        public void RemovesMultipleDashes()
        {
            TrackNameFixer
                .FixTrackName("01 -- Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesSingleDigitDotWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("1.Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesSingleDigitDotWithSpace()
        {
            TrackNameFixer
                .FixTrackName("1 . Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesSingleDigitDotWithLeadingSpace()
        {
            TrackNameFixer
                .FixTrackName("1. Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesSingleDigitDotWithTrailingSpace()
        {
            TrackNameFixer
                .FixTrackName("1 .Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitDotWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("01.Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitDotWithSpace()
        {
            TrackNameFixer
                .FixTrackName("01 . Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitDotWithLeadingSpace()
        {
            TrackNameFixer
                .FixTrackName("01. Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultiDigitDotWithTrailingSpace()
        {
            TrackNameFixer
                .FixTrackName("01 .Foo9")
                .Should().Be("Foo9");
        }

        [Test]
        public void RemovesMultipleDots()
        {
            TrackNameFixer
                .FixTrackName("01 .. Foo9")
                .Should().Be("Foo9");
        }
    }
}
