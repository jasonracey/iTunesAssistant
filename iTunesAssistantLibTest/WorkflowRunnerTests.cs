using FluentAssertions;
using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WorkflowRunnerTests
    {
        private const int itemsTotal = 42;

        private readonly Mock<IAppClassWrapper> _mockAppClass = new Mock<IAppClassWrapper>();

        private IEnumerable<Workflow> _workflows;

        private WorkflowRunner _workflowRunner;

        [TestInitialize]
        public void TestInitialize()
        {
            _workflows = new List<Workflow>();

            _mockAppClass.Setup(appClass => appClass.LibraryPlaylist.Tracks.Count).Returns(itemsTotal);
            _mockAppClass.Setup(appClass => appClass.SelectedTracks).Returns((IITTrackCollection)null);

            _workflowRunner = new WorkflowRunner(_mockAppClass.Object);
        }

        [TestMethod]
        public void SetsExpectedInitialState()
        {
            // act
            _workflowRunner.Run(_workflows);

            // assert
            _workflowRunner.ItemsProcessed.Should().Be(0);
            _workflowRunner.ItemsTotal.Should().Be(itemsTotal);
            _workflowRunner.State.Should().Be("Loading tracks...");
        }
    }
}
