using iTunesLib;
using Moq;
using System;
using System.Collections.Generic;

namespace iTunesAssistantLibTest
{
    public static class TestData
    {
        public const int MinAlbumSize = 11;
        public const int MaxAlbumSize = 17;

        public static readonly Random Random = new Random();

        public static IEnumerable<IEnumerable<IITTrack>> BuildMockAlbums(int albumCount)
        {
            var albums = new List<IEnumerable<IITTrack>>();

            for (var i = 0; i < albumCount; i++)
            {
                var album = BuildMockAlbum();
                albums.Add(album);
            }

            return albums;
        }

        public static IEnumerable<IEnumerable<IITTrack>> BuildMockMultiDiscAlbum(int discCount, string albumName)
        {
            var albums = new List<IEnumerable<IITTrack>>();

            for (var i = 0; i < discCount; i++)
            {
                var albumNameWithDiscNumber = $"{albumName} cd{i + 1}";
                var album = BuildMockAlbum(albumName: albumNameWithDiscNumber);
                albums.Add(album);
            }

            return albums;
        }

        public static IEnumerable<IITTrack> BuildMockAlbum(int trackCount = 0, string albumName = null, bool setupTrackNumbers = true)
        {
            if (trackCount == 0) trackCount = Random.Next(MinAlbumSize, MaxAlbumSize);

            if (string.IsNullOrWhiteSpace(albumName)) albumName = Guid.NewGuid().ToString();

            var tracks = new List<IITTrack>();

            for (var i = 0; i < trackCount; i++)
            {
                var track = new Mock<IITTrack>();

                track.SetupGet(t => t.Album).Returns(albumName);
                track.SetupGet(t => t.Name).Returns(Guid.NewGuid().ToString());

                if (setupTrackNumbers)
                {
                    // make at least one track have a number that's higher than count
                    // of tracks on album to verify numbers are set before counts
                    track.SetupGet(t => t.TrackNumber).Returns(i == 0
                        ? int.MaxValue
                        : Random.Next());
                }
                else
                {
                    track.SetupGet(t => t.TrackNumber).Returns(1);
                }

                // keep track of the value assigned for verification
                track.SetupSet(t => t.Album = It.IsAny<string>())
                    .Callback<string>(name => track.SetupGet(t => t.Album)
                    .Returns(name));

                // keep track of the value assigned for verification
                track.SetupSet(t => t.DiscCount = It.IsAny<int>())
                    .Callback<int>(count => track.SetupGet(t => t.DiscCount)
                    .Returns(count));

                // keep track of the value assigned for verification
                track.SetupSet(t => t.DiscNumber = It.IsAny<int>())
                    .Callback<int>(number => track.SetupGet(t => t.DiscNumber)
                    .Returns(number));

                // keep track of the value assigned for verification
                track.SetupSet(t => t.Name = It.IsAny<string>())
                    .Callback<string>(name => track.SetupGet(t => t.Name)
                    .Returns(name));

                // keep track of the value assigned for verification
                track.SetupSet(t => t.TrackCount = It.IsAny<int>())
                    .Callback<int>(count => track.SetupGet(t => t.TrackCount)
                    .Returns(count));

                // keep track of the value assigned for verification
                track.SetupSet(t => t.TrackNumber = It.IsAny<int>())
                    .Callback<int>(number => track.SetupGet(t => t.TrackNumber)
                    .Returns(number));

                tracks.Add(track.Object);
            }

            return tracks;
        }
    }
}
