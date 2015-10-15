using FluentAssertions;
using iTunesAssistantLib;
using NUnit.Framework;

namespace iTunesAssistantLibTest
{
    [TestFixture]
    public class WhenFixingGratefulDeadTrackName
    {
        [Test]
        public void PreservesJam()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Foo Jam")
                .Should().Be("Foo Jam");
        }

        [Test]
        public void FixesSegueArrow()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Foo ->")
                .Should().Be("Foo >");
        }

        [Test]
        public void FixesSegueSpaceArrow()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Foo - >")
                .Should().Be("Foo >");
        }

        [Test]
        public void PreservesSegue()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Foo >")
                .Should().Be("Foo >");
        }

        [Test]
        public void PreservesJamSegue()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Foo Jam >")
                .Should().Be("Foo Jam >");
        }

        [Test]
        public void CreatesSegueSpacing()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Foo>")
                .Should().Be("Foo >");
        }

        [Test]
        public void FixesSegueSpacing()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Foo  >")
                .Should().Be("Foo >");
        }

        [Test]
        public void CreatesJamSegueSpacing()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Foo Jam>")
                .Should().Be("Foo Jam >");
        }

        [Test]
        public void FixesJamSegueSpacing()
        {
            GratefulDeadTrackFixer
                .FixTrackName("Foo Jam  >")
                .Should().Be("Foo Jam >");
        }

        [Test]
        public void FixesCompoundTracks()
        {
            GratefulDeadTrackFixer
                .FixTrackName("other one jam > cryptical jam >")
                .Should().Be("The Other One Jam > Cryptical Envelopment Jam >");
        }
    }
}
