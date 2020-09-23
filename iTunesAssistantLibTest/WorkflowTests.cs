using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WorkflowTests
    {
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void NameIsRequired(string name)
        {
            Assert.ThrowsException<ArgumentNullException>(() => Workflow.Create(name: name));
        }

        [TestMethod]
        public void MergeAlbums()
        {
            // arrange
            var name = WorkflowName.MergeAlbums;

            // act
            var workflow = Workflow.Create(name);

            // assert
            workflow.Should().NotBeNull();
            workflow.Name.Should().Be(name);
            workflow.FileName.Should().BeNull();
            workflow.OldValue.Should().BeNull();
            workflow.NewValue.Should().BeNull();
            workflow.Type.Should().Be(WorkflowType.None);
        }

        [TestMethod]
        public void ImportTrackNames()
        {
            // arrange
            var name = WorkflowName.ImportTrackNames;
            var fileName = Guid.NewGuid().ToString();

            // act
            var workflow = Workflow.Create(name, fileName: fileName);

            // assert
            workflow.Should().NotBeNull();
            workflow.Name.Should().Be(name);
            workflow.FileName.Should().Be(fileName);
            workflow.OldValue.Should().BeNull();
            workflow.NewValue.Should().BeNull();
            workflow.Type.Should().Be(WorkflowType.None);
        }

        [TestMethod]
        public void FindAndReplace()
        {
            // arrange
            var name = WorkflowName.FindAndReplace;
            var oldValue = Guid.NewGuid().ToString();
            var newValue = Guid.NewGuid().ToString();

            // act
            var workflow = Workflow.Create(name, oldValue: oldValue, newValue: newValue);

            // assert
            workflow.Should().NotBeNull();
            workflow.Name.Should().Be(name);
            workflow.FileName.Should().BeNull();
            workflow.OldValue.Should().Be(oldValue);
            workflow.NewValue.Should().Be(newValue);
            workflow.Type.Should().Be(WorkflowType.Track);
        }

        [DataTestMethod]
        [DataRow(WorkflowName.FixCountOfTracksOnAlbum)]
        [DataRow(WorkflowName.FixTrackNumbers)]
        public void AlbumWorkflows(string name)
        {
            // act
            var workflow = Workflow.Create(name);

            // assert
            workflow.Should().NotBeNull();
            workflow.Name.Should().Be(name);
            workflow.FileName.Should().BeNull();
            workflow.OldValue.Should().BeNull();
            workflow.NewValue.Should().BeNull();
            workflow.Type.Should().Be(WorkflowType.Album);
        }

        [DataTestMethod]
        [DataRow(WorkflowName.FixGratefulDeadTracks)]
        [DataRow(WorkflowName.FixTrackNames)]
        [DataRow(WorkflowName.SetAlbumNames)]
        public void TrackWorkflows(string name)
        {
            // act
            var workflow = Workflow.Create(name);

            // assert
            workflow.Should().NotBeNull();
            workflow.Name.Should().Be(name);
            workflow.FileName.Should().BeNull();
            workflow.OldValue.Should().BeNull();
            workflow.NewValue.Should().BeNull();
            workflow.Type.Should().Be(WorkflowType.Track);
        }
    }
}
