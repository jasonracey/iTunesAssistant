using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class StringExtensionTests
    {
        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("  ", " ")]
        [DataRow("   ", " ")]
        [DataRow("alligator  jam", "alligator jam")]
        [DataRow("alligator  jam  > strawberry   jam", "alligator jam > strawberry jam")]
        public void RemoveDoubleSpaces(string before, string after)
        {
            before.RemoveDoubleSpaces().Should().Be(after);
        }

        [DataTestMethod]
        [DataRow("alligator    jam", "  ", " ", "alligator jam")]
        [DataRow("alligator  jam", "  ", ".", "alligator.jam")]
        public void RepeatedlyReplace(string before, string match, string substitution, string after)
        {
            before.RepeatedlyReplace(match, substitution).Should().Be(after);
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow(".", "")]
        [DataRow("a", "a")]
        [DataRow("ab", "ab")]
        [DataRow("!a@b#", "ab")]
        [DataRow("1", "1")]
        [DataRow("12", "12")]
        [DataRow("!1@2#", "12")]
        [DataRow("1a", "1a")]
        [DataRow("1a2b", "1a2b")]
        [DataRow("!1a@2b#", "1a2b")]
        [DataRow("A", "a")]
        [DataRow("AB", "ab")]
        [DataRow("!A@B#", "ab")]
        [DataRow("1A", "1a")]
        [DataRow("1A2B", "1a2b")]
        [DataRow("!1A@2B#", "1a2b")]
        public void ToLowerAlphaNumeric(string before, string after)
        {
            before.ToLowerAlphaNumeric().Should().Be(after);
        }
    }
}