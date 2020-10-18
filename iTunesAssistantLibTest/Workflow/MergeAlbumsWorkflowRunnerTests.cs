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
    public class MergeAlbumsWorkflowRunnerTests
    {
        private IList<IITTrack> _testTracks;
        private Mock<IWorkflowRunnerInfo> _mockWorkflowData;
        private Status _status;
        private MergeAlbumsWorkflowRunner _mergeAlbumsWorkflowRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockWorkflowData = new Mock<IWorkflowRunnerInfo>();
            _status = Status.Create(default);
            _mergeAlbumsWorkflowRunner = new MergeAlbumsWorkflowRunner();
        }

        [TestMethod]
        public void WhenWorkflowInfoNull_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _mergeAlbumsWorkflowRunner.Run(null, ref _status));
        }

        [TestMethod]
        public void WhenTracksNull_Throws()
        {
            // arrange
            _mockWorkflowData.Setup(m => m.Tracks).Returns((IList<IITTrack>)null);

            // act/assert
            Assert.ThrowsException<ArgumentNullException>(() => _mergeAlbumsWorkflowRunner.Run(_mockWorkflowData.Object, ref _status));
        }

        [TestMethod]
        public void WhenTracksIsEmpty_DoesNotThrow()
        {
            // arrange
            _mockWorkflowData.Setup(m => m.Tracks).Returns(new List<IITTrack>());

            // act
            _mergeAlbumsWorkflowRunner.Run(_mockWorkflowData.Object, ref _status);

            // assert
            Assert.AreEqual(0, _status.ItemsProcessed);
            Assert.AreEqual(0, _status.ItemsTotal);
            Assert.AreEqual("Running merge albums workflow...", _status.Message);
        }

        [TestMethod]
        public void WhenTracksIsNotEmpty_CanMergeSingleMultiDiscAlbums()
        {
            // arrange
            const int DiscCount = 2;
            const string AlbumName = "Mock Album";
            var albums = TestData.BuildMockMultiDiscAlbum(DiscCount, AlbumName);
            _testTracks = albums.SelectMany(album => album).ToList();
            _mockWorkflowData.Setup(m => m.Tracks).Returns(_testTracks);

            // act
            _mergeAlbumsWorkflowRunner.Run(_mockWorkflowData.Object, ref _status);

            // assert
            Assert.AreEqual(1, _status.ItemsProcessed);
            Assert.AreEqual(1, _status.ItemsTotal);
            Assert.AreEqual("Running merge albums workflow...", _status.Message);

            var processedTracks = _mockWorkflowData.Object.Tracks.ToList();

            Assert.IsTrue(processedTracks.All(track => track.Album == AlbumName));
            Assert.IsTrue(processedTracks.All(track => track.DiscCount == 1));
            Assert.IsTrue(processedTracks.All(track => track.DiscNumber == 1));
            Assert.IsTrue(processedTracks.All(track => track.TrackCount == _testTracks.Count()));
            AssertTrackNumbersAreCorrect(processedTracks, _testTracks.Count);
        }

        [TestMethod]
        public void WhenTracksIsNotEmpty_CanMergeMultipleMultiDiscAlbums()
        {
            // arrange
            const int FirstDiscCount = 4;
            const string FirstAlbumName = "First Mock Album";
            var firstSetOfDiscs = TestData.BuildMockMultiDiscAlbum(FirstDiscCount, FirstAlbumName);

            const int SecondDiscCount = 3;
            const string SecondAlbumName = "Second Mock Album";
            var secondSetOfDiscs = TestData.BuildMockMultiDiscAlbum(SecondDiscCount, SecondAlbumName);

            _testTracks = firstSetOfDiscs.Concat(secondSetOfDiscs).SelectMany(album => album).ToList();
            _mockWorkflowData.Setup(m => m.Tracks).Returns(_testTracks);

            // act
            _mergeAlbumsWorkflowRunner.Run(_mockWorkflowData.Object, ref _status);

            // assert
            Assert.AreEqual(2, _status.ItemsProcessed);
            Assert.AreEqual(2, _status.ItemsTotal);
            Assert.AreEqual("Running merge albums workflow...", _status.Message);

            var processedTracks = _mockWorkflowData.Object.Tracks.ToList();

            Assert.IsTrue(processedTracks.All(track => track.DiscCount == 1));
            Assert.IsTrue(processedTracks.All(track => track.DiscNumber == 1));

            var expectedFirstAlbumTrackCount = firstSetOfDiscs.SelectMany(track => track).Count();
            var firstAlbumTracks = processedTracks.Where(track => track.Album == FirstAlbumName).ToList();
            Assert.AreEqual(expectedFirstAlbumTrackCount, firstAlbumTracks.Count);
            Assert.IsTrue(firstAlbumTracks.All(track => track.TrackCount == expectedFirstAlbumTrackCount));
            AssertTrackNumbersAreCorrect(firstAlbumTracks, expectedFirstAlbumTrackCount);

            var expectedSecondAlbumTrackCount = secondSetOfDiscs.SelectMany(track => track).Count();
            var secondAlbumTracks = processedTracks.Where(track => track.Album == SecondAlbumName).ToList();
            Assert.AreEqual(expectedSecondAlbumTrackCount, secondAlbumTracks.Count);
            Assert.IsTrue(secondAlbumTracks.All(track => track.TrackCount == expectedSecondAlbumTrackCount));
            AssertTrackNumbersAreCorrect(secondAlbumTracks, expectedSecondAlbumTrackCount);
        }

        private void AssertTrackNumbersAreCorrect(IEnumerable<IITTrack> tracks, int expectedTrackCount)
        {
            for (var i = 0; i < expectedTrackCount; i++)
            {
                Assert.IsNotNull(tracks.FirstOrDefault(track => track.TrackNumber == i + 1));
            }
        }
    }
}