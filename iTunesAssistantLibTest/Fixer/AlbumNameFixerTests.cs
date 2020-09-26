using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class AlbumNameFixerTests
    {
        [DataTestMethod]
        [DataRow(" FoO   bAr1   disc1", "Foo Bar1")]
        [DataRow(" FoO   bAr1   disc01", "Foo Bar1")]
        [DataRow(" FoO   bAr1   disc 1", "Foo Bar1")]
        [DataRow(" FoO   bAr1   disc 01", "Foo Bar1")]
        [DataRow(" FoO   bAr1   [disc1]", "Foo Bar1")]
        [DataRow(" FoO   bAr1   [disc01]", "Foo Bar1")]
        [DataRow(" FoO   bAr1   [disc 1]", "Foo Bar1")]
        [DataRow(" FoO   bAr1   [disc 01]", "Foo Bar1")]
        [DataRow(" FoO   bAr1   (disc1)", "Foo Bar1")]
        [DataRow(" FoO   bAr1   (disc01)", "Foo Bar1")]
        [DataRow(" FoO   bAr1   (disc 1)", "Foo Bar1")]
        [DataRow(" FoO   bAr1   (disc 01)", "Foo Bar1")]
        public void RemovesDiscAndNumberFromName(string before, string after)
        {
            AlbumNameFixer.FixAlbumName(before).Should().Be(after);
        }

        [DataTestMethod]
        [DataRow(" FoO   bAr1   cd1", "Foo Bar1")]
        [DataRow(" FoO   bAr1   cd01", "Foo Bar1")]
        [DataRow(" FoO   bAr1   cd 1", "Foo Bar1")]
        [DataRow(" FoO   bAr1   cd 01", "Foo Bar1")]
        [DataRow(" FoO   bAr1   [cd1]", "Foo Bar1")]
        [DataRow(" FoO   bAr1   [cd01]", "Foo Bar1")]
        [DataRow(" FoO   bAr1   [cd 1]", "Foo Bar1")]
        [DataRow(" FoO   bAr1   [cd 01]", "Foo Bar1")]
        [DataRow(" FoO   bAr1   (cd1)", "Foo Bar1")]
        [DataRow(" FoO   bAr1   (cd01)", "Foo Bar1")]
        [DataRow(" FoO   bAr1   (cd 1)", "Foo Bar1")]
        [DataRow(" FoO   bAr1   (cd 01)", "Foo Bar1")]
        public void RemovesCdAndNumberFromName(string before, string after)
        {
            AlbumNameFixer.FixAlbumName(before).Should().Be(after);
        }
    }
}
