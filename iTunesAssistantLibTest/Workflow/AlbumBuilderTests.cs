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
            Assert.ThrowsException<ArgumentNullException>(() => AlbumBuilder.BuildAlbums(ref status, null));
        }

        [TestMethod]
        public void WhenTracksToFixIsEmpty_ReturnsExpectedResult_AndStatus()
        {
            // arrange
            var status = Status.Create(0);
            var tracksToFix = new List<IITTrack>();

            // act
            var result = AlbumBuilder.BuildAlbums(ref status, tracksToFix);

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
            Assert.ThrowsException<iTunesAssistantException>(() => AlbumBuilder.BuildAlbums(ref status, tracksToFix));
        }

        [TestMethod]
        public void WhenTracksToFixIsNotEmpty_GroupsTracksByAlbumName()
        {
            // arrange
            const int albumCount = 7;
            var status = Status.Create(0);
            var tracksToFix = BuildMockAlbums(albumCount).SelectMany(t => t).ToList();

            // act
            var result = AlbumBuilder.BuildAlbums(ref status, tracksToFix);

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(albumCount, result.Count);
            Assert.AreEqual(tracksToFix.Count, status.ItemsTotal);
            Assert.AreEqual(tracksToFix.Count, status.ItemsProcessed);
            Assert.AreEqual("Generating album list...", status.Message);
        }

        private IEnumerable<IEnumerable<IITTrack>> BuildMockAlbums(int albumCount)
        {
            var albums = new List<IEnumerable<IITTrack>>();

            var random = new Random();
            for (var i = 0; i < albumCount; i++)
            {
                var album = BuildMockAlbum(random.Next(11, 17));
                albums.Add(album);
            }

            return albums;
        }

        private IEnumerable<IITTrack> BuildMockAlbum(int trackCount)
        {
            var albumName = Guid.NewGuid().ToString();

            var tracks = new List<IITTrack>();

            for (var i = 0; i < trackCount; i++)
            {
                var track = new Mock<IITTrack>();
                track.Setup(t => t.Album).Returns(albumName);
                tracks.Add(track.Object);
            }

            return tracks;
        }
    }
}
