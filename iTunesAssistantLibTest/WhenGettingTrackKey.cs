using FluentAssertions;
using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WhenGettingTrackKey
    {
        [DataTestMethod]
        [DataRow(0, 0, "000-000")]
        [DataRow(1, 0, "001-000")]
        [DataRow(0, 1, "000-001")]
        [DataRow(1, 1, "001-001")]
        [DataRow(11, 0, "011-000")]
        [DataRow(0, 11, "000-011")]
        [DataRow(11, 11, "011-011")]
        [DataRow(111, 0, "111-000")]
        [DataRow(0, 111, "000-111")]
        [DataRow(111, 111, "111-111")]
        public void ReturnsExpectedKey(int discNumber, int trackNumber, string expectedKey)
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.SetupGet(t => t.DiscNumber).Returns(discNumber);
            mockTrack.SetupGet(t => t.TrackNumber).Returns(trackNumber);

            // act/assert
            mockTrack.Object.GetKey().Should().Be(expectedKey);
        }
    }
}
