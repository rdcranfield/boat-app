//using boat_app_v2.Models;

using boat_app_v2.BusinessLogic.Repository;
using boat_app_v2.Entities.Models;
using Moq;

namespace boat_app_v2.tests.Mocks;

internal abstract class MockIBoatRepository
{
    public static Mock<IBoatRepository> GetMock()
    {
        var mock = new Mock<IBoatRepository>();
        var boats = new List<Boat>()
        {
            new Boat()
            {
                Code = "ABCD-1234-C1",
                Name = "Sir Jon's Cabin Cruiser",
                Length = 13.716,
                Width = 2.5908,
            },
            new Boat()
            {
                Code = "ABCD-1234-B1",
                Name = "Sir cAN's Cabin Cruiser",
                Length = 13.716,
                Width = 2.5908,
            },
            new Boat()
            {
                Code = "ABCD-1234-B2",
                Name = "Sir jAMIE's Cabin Cruiser",
                Length = 13.716,
                Width = 2.5908,
            },
            new Boat()
            {
                Code = "AACD-1234-B2",
                Name = "Sir's Cabin Cruiser",
                Length = 13.716,
                Width = 2.5908,
            },
            new Boat()
            {
                Code = "AACD-1134-C2",
                Name = "Sir jAMIE's Cabin Cruiser",
                Length = 13.716,
                Width = 2.5908,
            }
        };
        mock.Setup(m => m.GetAllBoats()).Returns(() => boats);

        mock.Setup(m => m.GetBoatById(It.IsAny<string>()))
            .Returns((string code) => boats.FirstOrDefault(o => o.Code == code));

        mock.Setup(m => m.CreateBoat(It.IsAny<Boat>()))
            .Callback(() => { return; });

        mock.Setup(m => m.UpdateBoat(It.IsAny<Boat>()))
            .Callback(() => { return; });

        mock.Setup(m => m.DeleteBoat(It.IsAny<Boat>()))
            .Callback(() => { return; });
        return mock;
    }
}