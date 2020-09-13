using FluentAssertions;
using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WhenComparingTrackAndDiscNumber
    {
        private readonly IComparer<IITTrack> comparer = new TrackDiscAndNumberComparer();

        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0)]
        [DataRow(1, 0, 1, 0, 0)]
        [DataRow(0, 1, 0, 1, 0)]
        [DataRow(1, 1, 1, 1, 0)]
        [DataRow(0, 0, 0, 1, -1)]
        [DataRow(0, 0, 1, 0, -1)]
        [DataRow(0, 1, 1, 1, -1)]
        [DataRow(1, 0, 1, 1, -1)]
        [DataRow(1, 0, 0, 0, 1)]
        [DataRow(0, 1, 0, 0, 1)]
        [DataRow(1, 1, 1, 0, 1)]
        [DataRow(1, 1, 0, 1, 1)]
        public void WhenTracksAreEqual_ReturnsEqual(
            int discNumber1, 
            int trackNumber1,
            int discNumber2,
            int trackNumber2,
            int expecteResult)
        {
            // arrange
            var mockTrack1 = new Mock<IITTrack>();
            mockTrack1.SetupGet(t => t.DiscNumber).Returns(discNumber1);
            mockTrack1.SetupGet(t => t.TrackNumber).Returns(trackNumber1);

            var mockTrack2 = new Mock<IITTrack>();
            mockTrack2.SetupGet(t => t.DiscNumber).Returns(discNumber2);
            mockTrack2.SetupGet(t => t.TrackNumber).Returns(trackNumber2);

            // act
            var result = comparer.Compare(mockTrack1.Object, mockTrack2.Object);

            // assert
            result.Should().Be(expecteResult);
        }
    }
}
