using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class AlbumBuilderTests
    {
        [TestMethod]
        public void WhenTracksIsNull_Throws()
        {
            // arrange
            var status = Status.Create(0);

            // act/assert
            Assert.ThrowsException<ArgumentNullException>(() => AlbumBuilder.BuildAlbums(null, ref status));
        }

        [TestMethod]
        public void WhenTracksToFixIsEmpty_ReturnsExpectedResult_AndStatus()
        {
            // arrange
            var status = Status.Create(0);
            var tracksToFix = new List<IITTrack>();

            // act
            var result = AlbumBuilder.BuildAlbums(tracksToFix, ref status);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            Assert.AreEqual(0, status.ItemsTotal);
            Assert.AreEqual(0, status.ItemsProcessed);
            Assert.AreEqual("Generating album list...", status.Message);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void WhenAlbumNameIsNullOrWhiteSpace_Throws(string albumName)
        {
            // arrange
            var status = Status.Create(0);
            var mockTrack = new Mock<IITTrack>();
            mockTrack.Setup(t => t.Album).Returns(albumName);
            var tracksToFix = new List<IITTrack> { mockTrack.Object };

            // act/assert
            Assert.ThrowsException<iTunesAssistantException>(() => AlbumBuilder.BuildAlbums(tracksToFix, ref status));
        }

        [TestMethod]
        public void WhenTracksToFixIsNotEmpty_GroupsTracksByAlbumName()
        {
            // arrange
            const int albumCount = 7;
            var status = Status.Create(0);
            var tracksToFix = TestData.BuildMockAlbums(albumCount).SelectMany(t => t).ToList();

            // act
            var result = AlbumBuilder.BuildAlbums(tracksToFix, ref status);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(albumCount, result.Count);
            Assert.AreEqual(tracksToFix.Count, status.ItemsTotal);
            Assert.AreEqual(tracksToFix.Count, status.ItemsProcessed);
            Assert.AreEqual("Generating album list...", status.Message);
        }
    }
}
