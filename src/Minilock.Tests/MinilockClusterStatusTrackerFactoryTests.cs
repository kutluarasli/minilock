using System;
using AutoFixture;
using FluentAssertions;
using Minilock.Providers.Core;
using Moq;
using NUnit.Framework;

namespace Minilock.Tests
{
    public class MinilockClusterStatusTrackerFactoryTests
    {

        private Fixture _fixture;
        private Mock<IMinilockProvider> _provider;
        private MinilockClusterStatusTrackerFactory _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _provider = new Mock<IMinilockProvider>();
            _sut = new MinilockClusterStatusTrackerFactory(_provider.Object);
        }

        [Test]
        public void Given_ClusterInformation_It_Should_Create_Cluster_StatusTracker()
        {
            //Arrange

            var clusterName = _fixture.Create<string>();
            var hostName = _fixture.Create<string>();
            var clusterInformation = new ClusterInformation(clusterName, hostName);

            //Act
            var result =  _sut.CreateStatusTracker(clusterInformation);

            //Assert
            result.Should().NotBeNull();
            result.ClusterInformation.Should().BeEquivalentTo(clusterInformation);
            result.ClusterInformation.Should().NotBeSameAs(clusterInformation);
        }
        
        [Test]
        public void It_Should_Throw_ArgumentNullException_When_GivenClusterInformationIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.CreateStatusTracker(null));  
        }
    }
}