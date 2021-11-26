using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class RegionAbbreviationFixerTests
    {
        [DataTestMethod]
        [DataRow("0000-00-00 alabama arena, alabama al", "0000-00-00 alabama arena, alabama al")]
        [DataRow("0000-00-00 alabama arena, alabama al hambra", "0000-00-00 alabama arena, alabama al hambra")]
        [DataRow("0000-00-00 alabama arena, alabama, al hambra", "0000-00-00 alabama arena, alabama, al hambra")]
        [DataRow("0000-00-00 alabama arena, alabama alhambra", "0000-00-00 alabama arena, alabama alhambra")]
        [DataRow("0000-00-00 alabama arena, alabama, alhambra", "0000-00-00 alabama arena, alabama, alhambra")]
        [DataRow("0000-00-00 alabama arena, alabama,al", "0000-00-00 alabama arena, alabama,AL")]
        [DataRow("0000-00-00 alabama arena, alabama,al ", "0000-00-00 alabama arena, alabama,AL")]
        [DataRow("0000-00-00 alabama arena, alabama, al", "0000-00-00 alabama arena, alabama, AL")]
        [DataRow("0000-00-00 alabama arena, alabama, al ", "0000-00-00 alabama arena, alabama, AL")]
        [DataRow("0000-00-00 alabama arena, alabama , al", "0000-00-00 alabama arena, alabama , AL")]
        [DataRow("0000-00-00 alabama arena, alabama , al ", "0000-00-00 alabama arena, alabama , AL")]
        public void ReturnsExpectedString(string before, string after)
        {
            RegionAbbreviationFixer
                .FixRegionAbbreviation(before)
                .Should()
                .Be(after);
        }
    }
}
