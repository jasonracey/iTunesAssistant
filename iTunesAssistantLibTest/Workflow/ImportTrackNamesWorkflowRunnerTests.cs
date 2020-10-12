using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class ImportTrackNamesWorkflowRunnerTests
    {
        private const string TestInputFilePath = "TestInputFile.txt";

        private ImportTrackNamesWorkflowRunner _importTrackNamesWorkflowRunner;
        private Status _status;

        private IList<IITTrack> _testTracks;

        private Mock<IWorkflowRunnerInfo> _mockWorkflowData;

        [TestInitialize]
        public void TestInitialize()
        {
            _importTrackNamesWorkflowRunner = new ImportTrackNamesWorkflowRunner();
            _status = Status.Create(default);

            _testTracks = TestData.BuildMockAlbum().ToList();

            _mockWorkflowData = new Mock<IWorkflowRunnerInfo>();
            _mockWorkflowData.Setup(m => m.Tracks).Returns(_testTracks);
            _mockWorkflowData.Setup(m => m.InputFilePath).Returns(TestInputFilePath);

            File.WriteAllLines(TestInputFilePath, _testTracks.Select(t => t.Name));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(TestInputFilePath)) 
                File.Delete(TestInputFilePath);
        }

        [TestMethod]
        public void WhenWorkflowInfoNull_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _importTrackNamesWorkflowRunner.Run(null, ref _status));
        }

        [TestMethod]
        public void WhenTracksNull_Throws()
        {
            _mockWorkflowData.Setup(m => m.Tracks).Returns((IList<IITTrack>)null);
            Assert.ThrowsException<ArgumentNullException>(() => _importTrackNamesWorkflowRunner.Run(_mockWorkflowData.Object, ref _status));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void WhenInputFilePathNullOrWhiteSpace_Throws(string inputFilePath)
        {
            _mockWorkflowData.Setup(m => m.InputFilePath).Returns(inputFilePath);
            Assert.ThrowsException<ArgumentNullException>(() => _importTrackNamesWorkflowRunner.Run(_mockWorkflowData.Object, ref _status));
        }

        [TestMethod]
        public void WhenAnyTrackNumberZero_Throws()
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.SetupGet(t => t.TrackNumber).Returns(0);
            _testTracks.Add(mockTrack.Object);

            // act/assert
            Assert.ThrowsException<iTunesAssistantException>(() => _importTrackNamesWorkflowRunner.Run(_mockWorkflowData.Object, ref _status));
        }

        [TestMethod]
        public void WhenFileTrackCount_DoesntMatchListTrackCount_Throws()
        {
            // arrange
            File.WriteAllLines(TestInputFilePath, _testTracks.Skip(1).Select(t => t.Name));

            // act/assert
            Assert.ThrowsException<iTunesAssistantException>(() => _importTrackNamesWorkflowRunner.Run(_mockWorkflowData.Object, ref _status));
        }

        [TestMethod]
        public void WhenParsingFile_SkipsEmptyLines()
        {
            // arrange
            var lines = _testTracks.Select(t => t.Name)
                .Prepend(" ")
                .Append("  ");
            var expectedCount = lines.Count() - 2;
            File.WriteAllLines(TestInputFilePath, lines);

            // act
            _importTrackNamesWorkflowRunner.Run(_mockWorkflowData.Object, ref _status);

            // assert
            Assert.AreEqual(expectedCount, _status.ItemsProcessed);
            Assert.AreEqual(expectedCount, _status.ItemsTotal);
        }

        [TestMethod]
        public void WhenParsingFile_TrimsLines()
        {
            // arrange
            var lines = _testTracks.Select(t => $"   {t.Name}  ").ToList();
            File.WriteAllLines(TestInputFilePath, lines);

            // act
            _importTrackNamesWorkflowRunner.Run(_mockWorkflowData.Object, ref _status);

            // assert
            for (var i = 0; i < lines.Count(); i++)
            {
                Assert.AreEqual(lines[i].Trim(), _mockWorkflowData.Object.Tracks[i].Name);
            }
        }
    }
}
