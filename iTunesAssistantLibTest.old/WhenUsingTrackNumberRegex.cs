using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WhenUsingTrackNumberRegex
    {
        [TestMethod]
        public void RemovesSingleDigitWithSpace()
        {
            TrackNameFixer
                .FixTrackName("1 Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesSingleDigitWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("1Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitWithSpace()
        {
            TrackNameFixer
                .FixTrackName("01 Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("01Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesSingleDigitDashWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("1-Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesSingleDigitDashWithSpace()
        {
            TrackNameFixer
                .FixTrackName("1 - Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesSingleDigitDashWithLeadingSpace()
        {
            TrackNameFixer
                .FixTrackName("1- Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesSingleDigitDashWithTrailingSpace()
        {
            TrackNameFixer
                .FixTrackName("1 -Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitDashWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("01-Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitDashWithSpace()
        {
            TrackNameFixer
                .FixTrackName("01 - Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitDashWithLeadingSpace()
        {
            TrackNameFixer
                .FixTrackName("01- Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitDashWithTrailingSpace()
        {
            TrackNameFixer
                .FixTrackName("01 -Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultipleDashes()
        {
            TrackNameFixer
                .FixTrackName("01 -- Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesSingleDigitDotWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("1.Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesSingleDigitDotWithSpace()
        {
            TrackNameFixer
                .FixTrackName("1 . Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesSingleDigitDotWithLeadingSpace()
        {
            TrackNameFixer
                .FixTrackName("1. Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesSingleDigitDotWithTrailingSpace()
        {
            TrackNameFixer
                .FixTrackName("1 .Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitDotWithoutSpace()
        {
            TrackNameFixer
                .FixTrackName("01.Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitDotWithSpace()
        {
            TrackNameFixer
                .FixTrackName("01 . Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitDotWithLeadingSpace()
        {
            TrackNameFixer
                .FixTrackName("01. Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultiDigitDotWithTrailingSpace()
        {
            TrackNameFixer
                .FixTrackName("01 .Foo9")
                .Should().Be("Foo9");
        }

        [TestMethod]
        public void RemovesMultipleDots()
        {
            TrackNameFixer
                .FixTrackName("01 .. Foo9")
                .Should().Be("Foo9");
        }
    }
}
