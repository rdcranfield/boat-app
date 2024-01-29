using boat_app_v2.BusinessLogic.Repository;
using Moq;

namespace boat_app_v2.tests.Mocks
{
    internal class MockIRepositoryController
    {
        public static Mock<IRepositoryController> GetMock()
        {
            var mock = new Mock<IRepositoryController>();
            var boatRepoMock = MockIBoatRepository.GetMock();

            mock.Setup(m => m.BoatRepository).Returns(() => boatRepoMock.Object);
            mock.Setup(m => m.Save()).Callback(() => { return; });

            return mock;
        }
    }
}

