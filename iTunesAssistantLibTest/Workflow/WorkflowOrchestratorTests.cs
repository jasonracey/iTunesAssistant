using FluentAssertions;
using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WorkflowOrchestratorTests
    {
        private const int itemsTotal = 42;
        private const string trackName1 = "mock name 1";
        private const string trackName2 = "mock name 2";

        private readonly Mock<IAppClassWrapper> _mockAppClass = new Mock<IAppClassWrapper>();

        private Mock<IITTrack> mockTrack1;
        private Mock<IITTrack> mockTrack2;
        private Mock<IITTrackCollection> _mockTrackCollection;

        private Mock<IWorkflowRunner> _mockMergeAlbumsWorkflowRunner;
        private Mock<IWorkflowRunner> _mockImportTrackNamesWorkflowRunner;
        private Mock<IWorkflowRunner> _mockAlbumWorkflowRunner;
        private Mock<IWorkflowRunner> _mockTrackWorkflowRunner;

        private IEnumerable<IITTrack> _testTracks;
        private IEnumerable<Workflow> _testWorkflows;

        private WorkflowOrchestrator _workflowRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMergeAlbumsWorkflowRunner = new Mock<IWorkflowRunner>();
            _mockImportTrackNamesWorkflowRunner = new Mock<IWorkflowRunner>();
            _mockAlbumWorkflowRunner = new Mock<IWorkflowRunner>();
            _mockTrackWorkflowRunner = new Mock<IWorkflowRunner>();

            mockTrack1 = new Mock<IITTrack>();
            mockTrack2 = new Mock<IITTrack>();

            mockTrack1.SetupGet(t => t.Name).Returns(trackName1);
            mockTrack2.SetupGet(t => t.Name).Returns(trackName2);

            _testTracks = new IITTrack[]
            {
                mockTrack1.Object,
                mockTrack2.Object
            };

            _mockTrackCollection = new Mock<IITTrackCollection>();
            _mockTrackCollection.Setup(m => m.GetEnumerator()).Returns(_testTracks.GetEnumerator());

            _testWorkflows = new List<Workflow>
            {
                Workflow.Create(name: "mock workflow"),
            };

            _mockAppClass.Setup(appClass => appClass.SelectedTracks).Returns(_mockTrackCollection.Object);

            _mockAppClass.Setup(appClass => appClass.LibraryPlaylist.Tracks.Count).Returns(itemsTotal);
            _mockAppClass.Setup(appClass => appClass.SelectedTracks).Returns(_mockTrackCollection.Object);

            _workflowRunner = new WorkflowOrchestrator(
                _mockAppClass.Object,
                _mockMergeAlbumsWorkflowRunner.Object,
                _mockImportTrackNamesWorkflowRunner.Object,
                _mockAlbumWorkflowRunner.Object,
                _mockTrackWorkflowRunner.Object);
        }

        [TestMethod]
        public void WhenWorkflowsIsEmpty_DoesNotUpdateState_AndExits()
        {
            // arrange
            _testWorkflows = Enumerable.Empty<Workflow>();

            // act
            _workflowRunner.Run(_testWorkflows);

            // assert
            _workflowRunner.ItemsProcessed.Should().Be(default);
            _workflowRunner.ItemsTotal.Should().Be(default);
            _workflowRunner.Message.Should().Be(string.Empty);
            VerifyTimes(_mockMergeAlbumsWorkflowRunner, Times.Never());
            VerifyTimes(_mockImportTrackNamesWorkflowRunner, Times.Never());
            VerifyTimes(_mockAlbumWorkflowRunner, Times.Never());
            VerifyTimes(_mockTrackWorkflowRunner, Times.Never());
        }

        [TestMethod]
        public void WhenTrackCollectionIsNull_SetsExpectedInitialState_AndExits()
        {
            // arrange
            _mockAppClass.Setup(appClass => appClass.SelectedTracks).Returns((IITTrackCollection)null);

            // act
            _workflowRunner.Run(_testWorkflows);

            // assert
            _workflowRunner.ItemsProcessed.Should().Be(0);
            _workflowRunner.ItemsTotal.Should().Be(itemsTotal);
            _workflowRunner.Message.Should().Be("Loading selected tracks...");
            VerifyTimes(_mockMergeAlbumsWorkflowRunner, Times.Never());
            VerifyTimes(_mockImportTrackNamesWorkflowRunner, Times.Never());
            VerifyTimes(_mockAlbumWorkflowRunner, Times.Never());
            VerifyTimes(_mockTrackWorkflowRunner, Times.Never());
        }

        [TestMethod]
        public void WhenTrackCollectionIsEmpty_SetsExpectedInitialState_AndExits()
        {
            // arrange
            _mockTrackCollection.Setup(m => m.GetEnumerator()).Returns(new IITTrack[]{ }.GetEnumerator());

            // act
            _workflowRunner.Run(_testWorkflows);

            // assert
            _workflowRunner.ItemsProcessed.Should().Be(0);
            _workflowRunner.ItemsTotal.Should().Be(itemsTotal);
            _workflowRunner.Message.Should().Be("Loading selected tracks...");
            VerifyTimes(_mockMergeAlbumsWorkflowRunner, Times.Never());
            VerifyTimes(_mockImportTrackNamesWorkflowRunner, Times.Never());
            VerifyTimes(_mockAlbumWorkflowRunner, Times.Never());
            VerifyTimes(_mockTrackWorkflowRunner, Times.Never());
        }

        [TestMethod]
        public void WhenLoadingSelectedTracks_SkipsNullTracks_AndIncrementsItemsProcessed()
        {
            // arrange
            _testTracks = new IITTrack[] { mockTrack1.Object, null, mockTrack2.Object };
            _mockTrackCollection.Setup(m => m.GetEnumerator()).Returns(_testTracks.GetEnumerator());
            _mockAppClass.Setup(appClass => appClass.SelectedTracks).Returns(_mockTrackCollection.Object);

            // act
            _workflowRunner.Run(_testWorkflows);

            // assert
            _workflowRunner.ItemsProcessed.Should().Be(_testTracks.Count(t => t != null));
            _workflowRunner.ItemsTotal.Should().Be(itemsTotal);
            _workflowRunner.Message.Should().Be("Loading selected tracks...");
            VerifyTimes(_mockMergeAlbumsWorkflowRunner, Times.Never());
            VerifyTimes(_mockImportTrackNamesWorkflowRunner, Times.Never());
            VerifyTimes(_mockAlbumWorkflowRunner, Times.Never());
            VerifyTimes(_mockTrackWorkflowRunner, Times.Once());
        }

        [DataTestMethod]
        [DataRow(new[] { WorkflowName.FindAndReplace }, 0, 0, 0, 1)]
        [DataRow(new[] { WorkflowName.FixCountOfTracksOnAlbum }, 0, 0, 1, 0)]
        [DataRow(new[] { WorkflowName.FixGratefulDeadTracks }, 0, 0, 0, 1)]
        [DataRow(new[] { WorkflowName.FixTrackNames }, 0, 0, 0, 1)]
        [DataRow(new[] { WorkflowName.FixTrackNumbers }, 0, 0, 1, 0)]
        [DataRow(new[] { WorkflowName.ImportTrackNames }, 0, 1, 0, 0)]
        [DataRow(new[] { WorkflowName.MergeAlbums }, 1, 0, 0, 0)]
        [DataRow(new[] { WorkflowName.SetAlbumNames }, 0, 0, 0, 1)]
        [DataRow( new[] { WorkflowName.FindAndReplace, WorkflowName.FixCountOfTracksOnAlbum, WorkflowName.ImportTrackNames, WorkflowName.MergeAlbums }, 1, 1, 1, 1)]
        public void WhenRunningWorkflows_UsesTheCorrectRunners(string[] workflowNames, int timesMerge, int timesImport, int timesAlbum, int timesTrack)
        {
            // arrange
            _testWorkflows = workflowNames.Select(workflowName => 
            {
                return workflowName switch
                {
                    WorkflowName.ImportTrackNames => Workflow.Create(workflowName, fileName: "mock file name"),
                    WorkflowName.FindAndReplace => Workflow.Create(workflowName, oldValue: "mock old value", newValue: "mock new value"),
                    _ => Workflow.Create(workflowName)
                };
            });

            // act
            _workflowRunner.Run(_testWorkflows);

            // assert
            VerifyTimes(_mockMergeAlbumsWorkflowRunner, Times.Exactly(timesMerge));
            VerifyTimes(_mockImportTrackNamesWorkflowRunner, Times.Exactly(timesImport));
            VerifyTimes(_mockAlbumWorkflowRunner, Times.Exactly(timesAlbum));
            VerifyTimes(_mockTrackWorkflowRunner, Times.Exactly(timesTrack));
        }

        private void VerifyTimes(Mock<IWorkflowRunner> mockWorkflowRunner, Times times)
        {
            mockWorkflowRunner.Verify(mock => mock.Run(
                ref It.Ref<Status>.IsAny,
                It.IsAny<IList<IITTrack>>(),
                It.IsAny<IEnumerable<Workflow>>(),
                It.IsAny<string>()), times);
        }
    }
}
