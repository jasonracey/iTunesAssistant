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
    public class TrackNumberFixerTests
    {
        [TestMethod]
        public void WhenTracksIsNull_ThrowsExpectedException()
        {
            // act/assert
            Assert.ThrowsException<ArgumentNullException>(() => TrackNumberFixer.FixTrackNumbers(null));
        }

        [TestMethod]
        public void WhenTracksIsEmpty_DoesNotThrow()
        {
            // arrange
            var tracks = new List<IITTrack>();

            // act/assert
            TrackNumberFixer.FixTrackNumbers(tracks);
        }

        [TestMethod]
        public void WhenTrackNotFound_ThrowsExpectedException()
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.SetupSet(t => t.TrackNumber = It.IsAny<int>()).Throws(new System.Runtime.InteropServices.COMException(TrackMissingError.Code));
            var tracks = new List<IITTrack> { mockTrack.Object };

            // act/assert
            var exception = Assert.ThrowsException<iTunesAssistantException>(() => TrackNumberFixer.FixTrackNumbers(tracks));
            Assert.AreEqual(TrackMissingError.Message, exception.Message);
        }

        [TestMethod]
        public void WhenErrorIsNotTrackNotFound_ThrowsExpectedException()
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.SetupSet(t => t.TrackNumber = It.IsAny<int>()).Throws(new NotImplementedException());
            var tracks = new List<IITTrack> { mockTrack.Object };

            // act/assert
            Assert.ThrowsException<NotImplementedException>(() => TrackNumberFixer.FixTrackNumbers(tracks));
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(100)]
        public void SetsSequentialTrackNumbers(int trackCount)
        {
            // arrange
            var tracks = TestData.BuildMockAlbum(trackCount).ToList();

            // act
            TrackNumberFixer.FixTrackNumbers(tracks);

            // assert
            var trackList = tracks.ToList();
            trackList.Sort(new TrackDiscAndNumberComparer());
            for (var i = 0; i < trackList.Count; i++)
            {
                Assert.AreEqual(i + 1, trackList[i].TrackNumber);
            }
        }

        [TestMethod]
        public void CanSortTracksByName()
        {
            // arrange
            var tracks = TestData.BuildMockAlbum(trackCount: 10, setupTrackNumbers: false).ToList();

            // act
            TrackNumberFixer.FixTrackNumbers(tracks);

            // assert
            for (var i = 0; i < tracks.Count - 2; i++)
            {
                var curr = tracks.Single(t => t.TrackNumber == (i + 1));
                var next = tracks.Single(t => t.TrackNumber == (i + 2));
                Assert.AreEqual(-1, curr.Name.CompareTo(next.Name));
            }
        }
    }
}
