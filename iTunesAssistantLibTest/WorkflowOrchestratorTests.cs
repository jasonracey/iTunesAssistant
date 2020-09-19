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

        private IEnumerable<IITTrack> _testTracks;
        private IEnumerable<Workflow> _testWorkflows;

        private WorkflowOrchestrator _workflowRunner;

        [TestInitialize]
        public void TestInitialize()
        {
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

            _workflowRunner = new WorkflowOrchestrator(_mockAppClass.Object);
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
            _workflowRunner.Message.Should().Be(default);
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
            var countOfNonNullTracks = _testTracks.Count(t => t != null);
            _workflowRunner.ItemsProcessed.Should().Be(countOfNonNullTracks);
            _workflowRunner.ItemsTotal.Should().Be(countOfNonNullTracks);
            _workflowRunner.Message.Should().Be("Running track workflows...");
        }
    }
}
