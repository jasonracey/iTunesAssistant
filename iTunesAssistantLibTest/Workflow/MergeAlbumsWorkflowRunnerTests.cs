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
        private MergeAlbumsWorkflowRunner _mergeAlbumsWorkflowRunner;
        private Status _status;

        private IList<IITTrack> _testTracks;

        private Mock<IWorkflowRunnerInfo> _mockWorkflowData;

        [TestInitialize]
        public void TestInitialize()
        {
            _mergeAlbumsWorkflowRunner = new MergeAlbumsWorkflowRunner();
            _status = Status.Create(default);

            _testTracks = TestData.BuildMockAlbum().ToList();

            _mockWorkflowData = new Mock<IWorkflowRunnerInfo>();
            _mockWorkflowData.Setup(m => m.Tracks).Returns(_testTracks);
        }

        [TestMethod]
        public void WhenWorkflowInfoNull_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _mergeAlbumsWorkflowRunner.Run(null, ref _status));
        }

        [TestMethod]
        public void WhenTracksNull_Throws()
        {
            _mockWorkflowData.Setup(m => m.Tracks).Returns((IList<IITTrack>)null);
            Assert.ThrowsException<ArgumentNullException>(() => _mergeAlbumsWorkflowRunner.Run(_mockWorkflowData.Object, ref _status));
        }

        // todo
    }
}
