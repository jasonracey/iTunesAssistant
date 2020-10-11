using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iTunesAssistantLibTest.Fixer
{
    [TestClass]
    public class TrackCountFixerTests
    {
        [TestMethod]
        public void WhenTracksIsNull_ThrowsExpectedException()
        {
            // act/assert
            Assert.ThrowsException<ArgumentNullException>(() => TrackCountFixer.FixTrackCounts(null));
        }

        [TestMethod]
        public void WhenTracksIsEmpty_DoesNotThrow()
        {
            // arrange
            var tracks = new List<IITTrack>();

            // act/assert
            TrackCountFixer.FixTrackCounts(tracks);
        }

        [TestMethod]
        public void WhenTrackNotFound_ThrowsExpectedException()
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.SetupSet(t => t.TrackCount = It.IsAny<int>()).Throws(new System.Runtime.InteropServices.COMException(TrackMissingError.Code));
            var tracks = new List<IITTrack> { mockTrack.Object };

            // act/assert
            var exception = Assert.ThrowsException<iTunesAssistantException>(() => TrackCountFixer.FixTrackCounts(tracks));
            Assert.AreEqual(TrackMissingError.Message, exception.Message);
        }

        [TestMethod]
        public void WhenErrorIsNotTrackNotFound_ThrowsExpectedException()
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.SetupSet(t => t.TrackCount = It.IsAny<int>()).Throws(new NotImplementedException());
            var tracks = new List<IITTrack> { mockTrack.Object };

            // act/assert
            Assert.ThrowsException<NotImplementedException>(() => TrackCountFixer.FixTrackCounts(tracks));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(100)]
        public void SetsExpectedTrackCount(int trackCount)
        {
            // arrange
            var tracks = TestData.BuildMockAlbum(trackCount).ToList();

            // act
            TrackCountFixer.FixTrackCounts(tracks);

            // assert
            Assert.IsTrue(tracks.All(t => t.TrackCount == trackCount));
        }
    }
}
