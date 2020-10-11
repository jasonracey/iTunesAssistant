using FluentAssertions;
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
    public class AlbumWorkflowRunnerTests
    {
        private readonly int AlbumCount = TestData.Random.Next(1, 10);

        private IList<IITTrack> _tracksToFix;
        private IList<Workflow> _workflows;

        private WorkflowData _workflowData;
        private Status _status;

        private AlbumWorkflowRunner _albumWorkflowRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _tracksToFix = TestData.BuildMockAlbums(AlbumCount).SelectMany(t => t).ToList();
            _workflows = new List<Workflow>
            {
                Workflow.Create(WorkflowName.FixTrackNumbers),
                Workflow.Create(WorkflowName.FixCountOfTracksOnAlbum)
            };

            _workflowData = new WorkflowData(_tracksToFix, _workflows);
            _status = Status.Create(default);

            _albumWorkflowRunner = new AlbumWorkflowRunner();
        }

        [TestMethod]
        public void WhenWorkflowDataNull_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _albumWorkflowRunner.Run(null, ref _status));
        }

        [TestMethod]
        public void WhenTracksNull_Throws()
        {
            var mockWorkflowData = new Mock<IWorkflowData>();
            mockWorkflowData.Setup(m => m.Tracks).Returns((IList<IITTrack>)null);
            Assert.ThrowsException<ArgumentNullException>(() => _albumWorkflowRunner.Run(mockWorkflowData.Object, ref _status));
        }

        [TestMethod]
        public void WhenWorkflowsNull_Throws()
        {
            var mockWorkflowData = new Mock<IWorkflowData>();
            mockWorkflowData.Setup(m => m.Tracks).Returns(new List<IITTrack>());
            mockWorkflowData.Setup(m => m.Workflows).Returns((IEnumerable<Workflow>)null);
            Assert.ThrowsException<ArgumentNullException>(() => _albumWorkflowRunner.Run(mockWorkflowData.Object, ref _status));
        }

        [TestMethod]
        public void WhenNoTracks_SetsStatus_AndReturns()
        {
            // arrange
            _tracksToFix = new List<IITTrack>();
            _workflowData = new WorkflowData(_tracksToFix, _workflows);

            // act
            _albumWorkflowRunner.Run(_workflowData, ref _status);

            // assert
            _status.Should().NotBeNull();
            _status.ItemsProcessed.Should().Be(0);
            _status.ItemsTotal.Should().Be(0);
            _status.Message.Should().Be("Running album workflows...");
        }

        [TestMethod]
        public void WhenNoWorkflows_SetsStatus_AndReturns()
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.SetupGet(t => t.Album).Returns(Guid.NewGuid().ToString());
            _tracksToFix = new List<IITTrack> { mockTrack.Object };
            _workflows = new List<Workflow>();
            _workflowData = new WorkflowData(_tracksToFix, _workflows);

            // act
            _albumWorkflowRunner.Run(_workflowData, ref _status);

            // assert
            _status.Should().NotBeNull();
            _status.ItemsProcessed.Should().Be(_tracksToFix.Count());
            _status.ItemsTotal.Should().Be(_tracksToFix.Count());
            _status.Message.Should().Be("Running album workflows...");
            mockTrack.VerifySet(t => t.TrackCount = It.IsAny<int>(), Times.Never);
            mockTrack.VerifySet(t => t.TrackNumber = It.IsAny<int>(), Times.Never);
        }

        [TestMethod]
        public void FixesNumbersBeforeCounts()
        {
            // act
            _albumWorkflowRunner.Run(_workflowData, ref _status);

            // assert
            _status.Should().NotBeNull();
            _status.ItemsProcessed.Should().Be(AlbumCount);
            _status.ItemsTotal.Should().Be(AlbumCount);
            _status.Message.Should().Be("Running album workflows...");
            foreach (var album in _workflowData.Tracks.GroupBy(t => t.Album))
            {
                foreach (var track in album)
                {
                    track.TrackCount.Should().Be(album.Count());
                }
            }
        }
    }
}
