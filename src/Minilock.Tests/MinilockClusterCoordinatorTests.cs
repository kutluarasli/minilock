using AutoFixture;
using FluentAssertions;
using Minilock.Providers.Core;
using Moq;
using NUnit.Framework;

namespace Minilock.Tests
{
    public class MinilockClusterCoordinatorTests
    {
        private MinilockClusterCoordinator _sut;
        private Mock<IMinilockProvider> _provider;
        private Fixture _fixture;
        private ClusterInformation _clusterInformation;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _provider = new Mock<IMinilockProvider>();
            _clusterInformation = _fixture.Create<ClusterInformation>();
            _sut = new MinilockClusterCoordinator(_provider.Object, _clusterInformation);
        }

        [Test]
        public void It_Should_Acquire_MasterRole_When_Lock_Is_Free()
        {
            //Arrange

            _provider.Setup(provider => provider.LockAsync(_clusterInformation.ClusterName))
                .ReturnsAsync(new LockReference(true));

            //Act
            _sut.Start();

            //Assert
            _sut.IsMaster.Should().BeTrue();
        }
        
        [Test]
        public void It_Should_Notify_When_ClusterStatusChanged()
        {
            //Arrange

            var isNotified = false;

            _provider.Setup(provider => provider.LockAsync(_clusterInformation.ClusterName))
                .ReturnsAsync(new LockReference(true));

            //Act
            _sut.ClusterStatusChanged += (sender, args) => isNotified = true;
            _sut.Start();

            //Assert
            isNotified.Should().BeTrue();
        }

        [Test]
        public void It_Should_Remain_SlaveRole_When_Lock_Is_Not_Available()
        {
            //Arrange
            _provider.Setup(provider => provider.LockAsync(_clusterInformation.ClusterName))
                .ReturnsAsync(new LockReference(false));

            //Act
            _sut.Start();

            //Assert
            _sut.IsMaster.Should().BeFalse();
        }

        [Test]
        public void It_Should_Handover_When_MasterRoleExists_And_Closed_Gracefully()
        {
            //Arrange

            var lockReference = new LockReference(true); 

            _provider.Setup(provider => provider.LockAsync(_clusterInformation.ClusterName))
                .ReturnsAsync(lockReference);
            
            _sut.Start();

            //Act
            _sut.Close();

            //Assert
            _provider.Verify(provider =>
                provider.Unlock(lockReference));
        }
    }
}