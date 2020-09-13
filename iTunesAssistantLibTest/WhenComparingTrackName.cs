using FluentAssertions;
using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WhenComparingTrackName
    {
        private readonly IComparer<IITTrack> comparer = new TrackNameComparer();

        [DataTestMethod]
        [DataRow(null, null, 0)]
        [DataRow("", "", 0)]
        [DataRow(" ", " ", 0)]
        [DataRow("a", "a", 0)]
        [DataRow("a", "b", -1)]
        [DataRow("b", "a", 1)]
        public void WhenTracksAreEqual_ReturnsEqual(
            string trackName1,
            string trackName2,
            int expecteResult)
        {
            // arrange
            var mockTrack1 = new Mock<IITTrack>();
            mockTrack1.SetupGet(t => t.Name).Returns(trackName1);

            var mockTrack2 = new Mock<IITTrack>();
            mockTrack2.SetupGet(t => t.Name).Returns(trackName2);

            // act
            var result = comparer.Compare(mockTrack1.Object, mockTrack2.Object);

            // assert
            result.Should().Be(expecteResult);
        }
    }
}
