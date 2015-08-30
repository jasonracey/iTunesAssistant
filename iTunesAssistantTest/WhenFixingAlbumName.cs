﻿using FluentAssertions;
using iTunesAssistantLib;
using NUnit.Framework;

namespace iTunesAssistantLibTest
{
    [TestFixture]
    public class WhenFixingAlbumName
    {
        [Test]
        public void RemovesDiscAndNumberFromName()
        {
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   disc1")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   disc01")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   disc 1")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   disc 01")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   [disc1]")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   [disc01]")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   [disc 1]")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   [disc 01]")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   (disc1)")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   (disc01)")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   (disc 1)")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   (disc 01)")
                .Should().Be("Foo Bar1");
        }

        [Test]
        public void RemovesCdAndNumberFromName()
        {
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   cd1")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   cd01")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   cd 1")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   cd 01")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   [cd1]")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   [cd01]")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   [cd 1]")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   [cd 01]")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   (cd1)")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   (cd01)")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   (cd 1)")
                .Should().Be("Foo Bar1");
            AlbumNameFixer
                .FixAlbumName(" FoO   bAr1   (cd 01)")
                .Should().Be("Foo Bar1");
        }
    }
}