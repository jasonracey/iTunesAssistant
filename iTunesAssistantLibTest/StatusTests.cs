using FluentAssertions;
using iTunesAssistantLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace iTunesAssistantLibTest
{
    [TestClass]
    public class StatusTests
    {
        private readonly Random _random = new Random();

        [TestMethod]
        public void ItemsTotal_MustBeGreaterThanOrEqualToZero()
        {
            // arrange
            var itemsTotal = _random.Next(-100, -1);

            // act/assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Status.Create(itemsTotal));
        }

        [TestMethod]
        public void Create_WithoutMessage()
        {
            // arrange
            var itemsTotal = _random.Next(1, 100);

            // act
            var status = Status.Create(itemsTotal);

            // assert
            status.ItemsProcessed.Should().Be(0);
            status.ItemsTotal.Should().Be(itemsTotal);
            status.Message.Should().Be(string.Empty);
        }

        [TestMethod]
        public void Create_WithMessage()
        {
            // arrange
            var itemsTotal = _random.Next(1, 100);
            var message = Guid.NewGuid().ToString();

            // act
            var status = Status.Create(itemsTotal, message);

            // assert
            status.ItemsProcessed.Should().Be(0);
            status.ItemsTotal.Should().Be(itemsTotal);
            status.Message.Should().Be(message);
        }

        [TestMethod]
        public void ItemsProcessed()
        {
            // arrange
            var iterations = _random.Next(1, 100);
            var status = Status.Create(default);

            // act
            for (var i = 0; i < iterations; i++) status.ItemProcessed();

            // assert
            status.ItemsProcessed.Should().Be(iterations);
        }
    }
}
