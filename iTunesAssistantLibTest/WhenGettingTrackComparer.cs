using FluentAssertions;
using iTunesAssistantLib;
using iTunesLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class WhenGettingTrackComparer
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
}
