using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class GratefulDeadTrackNameDataTest
    {
        [DataTestMethod]
        [DataRow("", null)]
        [DataRow(" ", null)]
        [DataRow("blah", null)]
        [DataRow("alligator", "Alligator")]
        [DataRow(" penelope  alligator armadillo  ", "Alligator")]
        public void Test(string term, string expected)
        {
            GratefulDeadTrackNameData.Search(term).Should().Be(expected);
        }
    }
}
