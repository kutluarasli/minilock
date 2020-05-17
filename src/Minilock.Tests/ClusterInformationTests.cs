using System;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;

namespace Minilock.Tests
{
    public class ClusterInformationTests
    {
        private Fixture _fixture;
        private ClusterInformation _sut;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void It_Should_Assign_ClusterInformation_When_Initialized_Successfully()
        {
            //Arrange
            var clusterName = _fixture.Create<string>();
            var hostName = _fixture.Create<string>();

            //Act
            _sut = new ClusterInformation(clusterName, hostName);

            //Assert
            _sut.ClusterName.Should().Be(clusterName);
            _sut.HostName.Should().Be(hostName);
        }
        
        [Test]
        public void It_Should_Throw_ArgumentNullException_When_ClusterName_Is_Null()
        {
            //Arrange
            var clusterName = string.Empty;
            var hostName = _fixture.Create<string>();

            //Act
            Assert.Throws<ArgumentNullException>(() => _sut = new ClusterInformation(clusterName, hostName))
                .ParamName.Should().Be("clusterName");
        }
        
        [Test]
        public void It_Should_Throw_ArgumentNullException_When_HostName_Is_Null()
        {
            //Arrange
            var clusterName = _fixture.Create<string>();
            var hostName = string.Empty;

            //Act
            Assert.Throws<ArgumentNullException>(() => _sut = new ClusterInformation(clusterName, hostName))
                .ParamName.Should().Be("hostName");
        }
    }
}