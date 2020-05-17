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

            var isMasterNow = false;
                
            _provider.Setup(provider => provider.Lock(_clusterInformation.ClusterName, _clusterInformation.HostName))
                .Returns(true);

            //Act
            _sut.ClusterStatusChanged += (sender, args) => isMasterNow = args.IsMaster; 
            _sut.Start();


            //Assert
            isMasterNow.Should().BeTrue();
        }

        [Test]
        public void It_Should_Remain_SlaveRole_When_Lock_Is_Not_Available()
        {
            //Arrange
            var isMasterNow = false;
            
            _provider.Setup(provider => provider.Lock(_clusterInformation.ClusterName, _clusterInformation.HostName))
                .Returns(false);

            //Act
            _sut.ClusterStatusChanged += (sender, args) => isMasterNow = args.IsMaster;
            _sut.Start();

            //Assert
            isMasterNow.Should().BeFalse();
        }

        [Test]
        public void It_Should_Try_To_Claim_MasterRole_When_Lock_Is_Released_By_AnotherHost()
        {
            //Arrange
            var eventArgs = new LockReleasedEventArgs(_clusterInformation.ClusterName);

            //Act
            _sut.Start();

            _provider.Raise(provider => provider.LockReleased += null, eventArgs);

            //Assert
            _provider.Verify(provider => provider.Lock(_clusterInformation.ClusterName, _clusterInformation.HostName));
        }

        [Test]
        public void It_Should_Handover_When_MasterRoleExists_And_Closed_Gracefully()
        {
            //Arrange

            _provider.Setup(provider => provider.Lock(_clusterInformation.ClusterName, _clusterInformation.HostName))
                .Returns(true);
            
            _sut.Start();

            //Act
            _sut.Close();

            //Assert
            _provider.Verify(provider =>
                provider.Unlock(_clusterInformation.ClusterName, _clusterInformation.HostName));
        }
    }
}