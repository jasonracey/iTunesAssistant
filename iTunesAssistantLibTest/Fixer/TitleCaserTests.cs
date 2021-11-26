using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class TitleCaserTests
    {
        [DataTestMethod]
        [DataRow("hello world", "Hello World")]
        [DataRow("HELLO WORLD", "Hello World")]
        [DataRow("Hello World", "Hello World")]
        public void ReturnsExpectedString(string before, string after)
        {
            TitleCaser
                .ToTitleCase(before)
                .Should()
                .Be(after);
        }
    }
}
