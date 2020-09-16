using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class GratefulDeadTrackNameFixerTests
    {
        [DataTestMethod]
        [DataRow("Alligator>", "Alligator >")]
        [DataRow("Alligator >", "Alligator >")]
        [DataRow("Alligator  >", "Alligator >")]
        [DataRow("Alligator->", "Alligator >")]
        [DataRow("Alligator ->", "Alligator >")]
        [DataRow("Alligator - >", "Alligator >")]
        [DataRow("Alligator Jam", "Alligator Jam")]
        [DataRow("Alligator Jam>", "Alligator Jam >")]
        [DataRow("Alligator Jam >", "Alligator Jam >")]
        [DataRow("Alligator Jam->", "Alligator Jam >")]
        [DataRow("Alligator Jam ->", "Alligator Jam >")]
        [DataRow("Alligator Jam - >", "Alligator Jam >")]
        [DataRow("other one jam > cryptical jam >", "The Other One Jam > Cryptical Envelopment Jam >")]
        public void FixTrackName(string before, string after)
        {
            GratefulDeadTrackNameFixer.FixTrackName(before).Should().Be(after);
        }
    }
}
