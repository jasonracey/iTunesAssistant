using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class GratefulDeadTrackFixerTests
    {
        [TestMethod]
        public void PreservesJam()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Alligator Jam")
                .Should().Be("Alligator Jam");
        }

        [TestMethod]
        public void FixesSegueArrow()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Alligator ->")
                .Should().Be("Alligator >");
        }

        [TestMethod]
        public void FixesSegueSpaceArrow()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Alligator - >")
                .Should().Be("Alligator >");
        }

        [TestMethod]
        public void PreservesSegue()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Alligator >")
                .Should().Be("Alligator >");
        }

        [TestMethod]
        public void PreservesJamSegue()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Alligator Jam >")
                .Should().Be("Alligator Jam >");
        }

        [TestMethod]
        public void CreatesSegueSpacing()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Alligator>")
                .Should().Be("Alligator >");
        }

        [TestMethod]
        public void FixesSegueSpacing()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Alligator  >")
                .Should().Be("Alligator >");
        }

        [TestMethod]
        public void CreatesJamSegueSpacing()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Alligator Jam>")
                .Should().Be("Alligator Jam >");
        }

        [TestMethod]
        public void FixesJamSegueSpacing()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Alligator Jam  >")
                .Should().Be("Alligator Jam >");
        }

        [TestMethod]
        public void FixesCompoundTracks()
        {
            GratefulDeadTrackFixer
                .FixTrackName("other one jam > cryptical jam >")
                .Should().Be("The Other One Jam > Cryptical Envelopment Jam >");
        }
    }
}
