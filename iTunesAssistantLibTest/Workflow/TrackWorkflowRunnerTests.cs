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
    public class TrackWorkflowRunnerTests
    {
        private Status _status;
        private IList<IITTrack> _tracksToFix;
        private IList<Workflow> _workflows;

        private TrackWorkflowRunner _trackWorkflowRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _status = Status.Create(default);
            _tracksToFix = new List<IITTrack>();
            _workflows = new List<Workflow>();

            _trackWorkflowRunner = new TrackWorkflowRunner();
        }

        [TestMethod]
        public void ValidatesArgs()
        {
            Status nullStatus = null;
            Assert.ThrowsException<ArgumentNullException>(() => _trackWorkflowRunner.Run(ref nullStatus, _tracksToFix, _workflows));
            Assert.ThrowsException<ArgumentNullException>(() => _trackWorkflowRunner.Run(ref _status, null, _workflows));
            Assert.ThrowsException<ArgumentNullException>(() => _trackWorkflowRunner.Run(ref _status, _tracksToFix, null));
        }

        [TestMethod]
        public void WhenNoTracks_SetsStatus_AndReturns()
        {
            // arrange
            _tracksToFix = new List<IITTrack>();

            // act
            _trackWorkflowRunner.Run(ref _status, _tracksToFix, _workflows);

            // assert
            _status.Should().NotBeNull();
            _status.ItemsProcessed.Should().Be(0);
            _status.ItemsTotal.Should().Be(0);
            _status.Message.Should().Be("Running track workflows...");
        }

        [TestMethod]
        public void WhenNoWorkflows_SetsStatus_AndReturns()
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            _tracksToFix.Add(mockTrack.Object);

            // act
            _trackWorkflowRunner.Run(ref _status, _tracksToFix, _workflows);

            // assert
            _status.Should().NotBeNull();
            _status.ItemsProcessed.Should().Be(_tracksToFix.Count());
            _status.ItemsTotal.Should().Be(_tracksToFix.Count());
            _status.Message.Should().Be("Running track workflows...");
            mockTrack.VerifySet(t => t.Album = It.IsAny<string>(), Times.Never);
            mockTrack.VerifySet(t => t.Comment = It.IsAny<string>(), Times.Never);
            mockTrack.VerifySet(t => t.Name = It.IsAny<string>(), Times.Never);
        }

        [DataTestMethod]
        [DataRow("t1", "a1", 0)]
        [DataRow("t1", null, 1)]
        [DataRow("t1", "", 1)]
        [DataRow("t1", " ", 1)]
        public void WhenSetAlbumNames_IfAlbumNameNullOrWhiteSpace_SetsToTrackName(string trackName, string albumName, int times)
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.Setup(t => t.Name).Returns(trackName);
            mockTrack.Setup(t => t.Album).Returns(albumName);
            _tracksToFix.Add(mockTrack.Object);
            _workflows.Add(Workflow.Create(name: WorkflowName.SetAlbumNames));

            // act
            _trackWorkflowRunner.Run(ref _status, _tracksToFix, _workflows);

            // assert
            mockTrack.VerifySet(t => t.Album = trackName, Times.Exactly(times));
        }

        [DataTestMethod]
        [DataRow("x", "y", 1)]
        [DataRow(null, "y", 0)]
        [DataRow("", "y", 0)]
        public void WhenFindAndReplace_IfOldValueNotNullOrEmpty_SetsNewValue(string oldValue, string newValue, int times)
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.Setup(t => t.Name).Returns(Guid.NewGuid().ToString());
            _tracksToFix.Add(mockTrack.Object);
            _workflows.Add(Workflow.Create(name: WorkflowName.FindAndReplace, oldValue: oldValue, newValue: newValue));

            // act
            _trackWorkflowRunner.Run(ref _status, _tracksToFix, _workflows);

            // assert
            mockTrack.VerifySet(t => t.Name = It.IsAny<string>(), Times.Exactly(times));
        }

        [TestMethod]
        public void WhenFixTrackName_SetsTrackName()
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.Setup(t => t.Name).Returns(Guid.NewGuid().ToString());
            _tracksToFix.Add(mockTrack.Object);
            _workflows.Add(Workflow.Create(name: WorkflowName.FixTrackNames));

            // act
            _trackWorkflowRunner.Run(ref _status, _tracksToFix, _workflows);

            // assert
            mockTrack.VerifySet(t => t.Name = It.IsAny<string>(), Times.Once);
        }

        [TestMethod]
        public void WhenFixGratefulDeadTracks_AndCommentIsNotNullOrWhiteSpace_SetsName_AndComment()
        {
            // arrange
            var expectedComment = Guid.NewGuid().ToString();
            var mockTrack = new Mock<IITTrack>();
            mockTrack.Setup(t => t.Name).Returns(Guid.NewGuid().ToString());
            mockTrack.Setup(t => t.Comment).Returns("https://archive.org/details/" + expectedComment);
            _tracksToFix.Add(mockTrack.Object);
            _workflows.Add(Workflow.Create(name: WorkflowName.FixGratefulDeadTracks));

            // act
            _trackWorkflowRunner.Run(ref _status, _tracksToFix, _workflows);

            // assert
            mockTrack.VerifySet(t => t.Name = It.IsAny<string>(), Times.Once);
            mockTrack.VerifySet(t => t.Comment = expectedComment, Times.Once);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void WhenFixGratefulDeadTracks_AndCommentIsNullOrWhiteSpace_ThrowsException(string comment)
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.Setup(t => t.Name).Returns(Guid.NewGuid().ToString());
            mockTrack.Setup(t => t.Comment).Returns(comment);
            _tracksToFix.Add(mockTrack.Object);
            _workflows.Add(Workflow.Create(name: WorkflowName.FixGratefulDeadTracks));

            // act/assert
            Assert.ThrowsException<iTunesAssistantException>(() => _trackWorkflowRunner.Run(ref _status, _tracksToFix, _workflows));
        }
    }
}
