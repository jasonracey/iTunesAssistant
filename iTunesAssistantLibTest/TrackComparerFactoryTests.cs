using FluentAssertions;
using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class TrackComparerFactoryTests
    {
        private Mock<IITTrack> GetMockTrack(int trackNumber)
        {
            var track = new Mock<IITTrack>();
            track.SetupGet(mock => mock.TrackNumber).Returns(trackNumber);
            return track;
        }

        [TestMethod]
        public void WhenAnyTrackNumberIsZero_ReturnsTrackNameComparer()
        {
            // arrange
            var tracks = new List<IITTrack>
            {
                GetMockTrack(0).Object
            };

            // act
            var comparer = TrackComparerFactory.GetTrackComparer(tracks);

            // assert
            comparer.Should().BeOfType<TrackNameComparer>();
        }

        [TestMethod]
        public void WhenAnyTrackNumbersAreTheSame_ReturnsTrackNameComparer()
        {
            // arrange
            var tracks = new List<IITTrack>
            {
                GetMockTrack(1).Object,
                GetMockTrack(1).Object
            };

            // act
            var comparer = TrackComparerFactory.GetTrackComparer(tracks);

            // assert
            comparer.Should().BeOfType<TrackNameComparer>();
        }

        [TestMethod]
        public void WhenNoTrackNumberIsZero_AndNoTrackNumbersAreTheSame_ReturnsTrackDiscAndNumberComparer()
        {
            // arrange
            var tracks = new List<IITTrack>
            {
                GetMockTrack(9).Object,
                GetMockTrack(3).Object,
                GetMockTrack(7).Object,
            };

            // act
            var comparer = TrackComparerFactory.GetTrackComparer(tracks);

            // assert
            comparer.Should().BeOfType<TrackDiscAndNumberComparer>();
        }
    }

    [TestClass]
    public class TrackDiscAndNumberComparerTests
    {
        private readonly IComparer<IITTrack> comparer = new TrackDiscAndNumberComparer();

        [DataTestMethod]
        [DataRow(0, 0, 0, 0, 0)]
        [DataRow(1, 0, 1, 0, 0)]
        [DataRow(0, 1, 0, 1, 0)]
        [DataRow(1, 1, 1, 1, 0)]
        [DataRow(0, 0, 0, 1, -1)]
        [DataRow(0, 0, 1, 0, -1)]
        [DataRow(0, 1, 1, 1, -1)]
        [DataRow(1, 0, 1, 1, -1)]
        [DataRow(1, 0, 0, 0, 1)]
        [DataRow(0, 1, 0, 0, 1)]
        [DataRow(1, 1, 1, 0, 1)]
        [DataRow(1, 1, 0, 1, 1)]
        public void WhenTracksAreEqual_ReturnsEqual(
            int discNumber1,
            int trackNumber1,
            int discNumber2,
            int trackNumber2,
            int expecteResult)
        {
            // arrange
            var mockTrack1 = new Mock<IITTrack>();
            mockTrack1.SetupGet(t => t.DiscNumber).Returns(discNumber1);
            mockTrack1.SetupGet(t => t.TrackNumber).Returns(trackNumber1);

            var mockTrack2 = new Mock<IITTrack>();
            mockTrack2.SetupGet(t => t.DiscNumber).Returns(discNumber2);
            mockTrack2.SetupGet(t => t.TrackNumber).Returns(trackNumber2);

            // act
            var result = comparer.Compare(mockTrack1.Object, mockTrack2.Object);

            // assert
            result.Should().Be(expecteResult);
        }
    }

    [TestClass]
    public class TrackNameComparerTests
    {
        private readonly IComparer<IITTrack> comparer = new TrackNameComparer();

        [DataTestMethod]
        [DataRow(null, null, 0)]
        [DataRow("", "", 0)]
        [DataRow(" ", " ", 0)]
        [DataRow("a", "a", 0)]
        [DataRow("a", "b", -1)]
        [DataRow("b", "a", 1)]
        public void WhenTracksAreEqual_ReturnsEqual(
            string trackName1,
            string trackName2,
            int expecteResult)
        {
            // arrange
            var mockTrack1 = new Mock<IITTrack>();
            mockTrack1.SetupGet(t => t.Name).Returns(trackName1);

            var mockTrack2 = new Mock<IITTrack>();
            mockTrack2.SetupGet(t => t.Name).Returns(trackName2);

            // act
            var result = comparer.Compare(mockTrack1.Object, mockTrack2.Object);

            // assert
            result.Should().Be(expecteResult);
        }
    }

    [TestClass]
    public class TrackKeyTests
    {
        [DataTestMethod]
        [DataRow(0, 0, "000-000")]
        [DataRow(1, 0, "001-000")]
        [DataRow(0, 1, "000-001")]
        [DataRow(1, 1, "001-001")]
        [DataRow(11, 0, "011-000")]
        [DataRow(0, 11, "000-011")]
        [DataRow(11, 11, "011-011")]
        [DataRow(111, 0, "111-000")]
        [DataRow(0, 111, "000-111")]
        [DataRow(111, 111, "111-111")]
        public void ReturnsExpectedKey(int discNumber, int trackNumber, string expectedKey)
        {
            // arrange
            var mockTrack = new Mock<IITTrack>();
            mockTrack.SetupGet(t => t.DiscNumber).Returns(discNumber);
            mockTrack.SetupGet(t => t.TrackNumber).Returns(trackNumber);

            // act/assert
            mockTrack.Object.GetKey().Should().Be(expectedKey);
        }
    }
}
