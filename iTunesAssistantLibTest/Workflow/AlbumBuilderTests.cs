using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class AlbumBuilderTests
    {
        [TestMethod]
        public void WhenTracksIsNull_ThrowsArgumentNullException()
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
    }
}
