using AutoMapper;
using boat_app_v2.Controllers;
using boat_app_v2.Entities.DataTransferObjects;
using boat_app_v2.Entities.Models;
using boat_app_v2.Models;
using boat_app_v2.Services;
using boat_app_v2.tests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace boat_app_v2.tests.UnitTests;

public class Tests
{
    private BoatRepositoryController _boatRepositoryController;

    private string inactiveBoatCode = "ABCD-1234-A1";
    private string activeBoatCode = "ABCD-1234-C1";
    private IMapper mapper;

    public IMapper GetMapper()
    {
        var mappingProfile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
        return new Mapper(configuration);
    }
    
    [SetUp]
    public void Setup()
    {
        var repositoryControllerMock = MockIRepositoryController.GetMock();

        var logger = new Mock<ILogger<BoatRepositoryController>>().Object;
        mapper = GetMapper();

        _boatRepositoryController = new BoatRepositoryController(logger, repositoryControllerMock.Object, mapper);
    }

    [Test]
    public void Test_GetAllBoats()
    {
        var result = _boatRepositoryController.GetAllBoats() as ObjectResult;
        Assert.NotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        Assert.IsAssignableFrom<List<BoatObject>>(result.Value);
    }
    
    
    [Test]
    public void Test_GetBoatById()
    {
        //test inactive code
        var result = _boatRepositoryController.GetBoatById(inactiveBoatCode) as ObjectResult;
        Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);
        Boat? boat = result.Value as Boat;
        Assert.AreEqual(null, boat);

        //test active code
        result = _boatRepositoryController.GetBoatById(activeBoatCode) as ObjectResult;
        Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        Assert.NotNull(result);
        boat = result.Value as Boat;
        Assert.AreEqual("Sir Jon's Cabin Cruiser", boat.Name);
    }
    
    [Test]
    public void Test_CreateBoat()
    {
        //not really needed, considering user never defines the code, the back-end does, but just to be extra safe
        var incorrectCode = "123177&&";
        Boat createBoat = new Boat
        {
            Code = incorrectCode
        };
        
        //test incorrect code
        var result = _boatRepositoryController.CreateBoat(createBoat) as ObjectResult;
        Assert.AreEqual(StatusCodes.Status400BadRequest, result!.StatusCode);

        //test unique code, try create boat with an existing code
        createBoat = new Boat
        {
            Code = activeBoatCode
        };
        result = _boatRepositoryController.CreateBoat(createBoat) as ObjectResult;
        Assert.AreEqual(StatusCodes.Status409Conflict, result!.StatusCode);
        
        //test new code, ´
        createBoat = new Boat
        {
            Code = inactiveBoatCode
        };
        result = _boatRepositoryController.CreateBoat(createBoat) as ObjectResult;
        Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        Assert.NotNull(result);
        Boat boat = result.Value as Boat;
        Assert.AreNotEqual(activeBoatCode, boat.Code);
        Assert.AreEqual(inactiveBoatCode, boat.Code);
    }
    
    
    [Test]
    public void Test_UpdateBoat()
    {
        // test update on existing boat
        var result = _boatRepositoryController.GetBoatById(activeBoatCode) as ObjectResult;
        Boat? boat = result.Value as Boat;
        boat.Name = "test";
        
        result = _boatRepositoryController.UpdateBoat(boat) as ObjectResult;
        Assert.NotNull(result);
        Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        boat = result.Value as Boat;
        Assert.AreEqual("test", boat.Name);
        
        // test null update
        result = _boatRepositoryController.UpdateBoat(null) as ObjectResult;
        Assert.AreEqual(StatusCodes.Status400BadRequest, result!.StatusCode);

        // test update on boat that does not exist
        Boat imaginaryBoat = new Boat
        {
            Code = inactiveBoatCode
        };
        result = _boatRepositoryController.UpdateBoat(imaginaryBoat) as ObjectResult;
        Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);
    }
    
    [Test]
    public void Test_DeleteBoat()
    {
        var result = _boatRepositoryController.DeleteBoat(activeBoatCode) as ObjectResult;
        Assert.AreEqual(StatusCodes.Status200OK, result!.StatusCode);
        
        result = _boatRepositoryController.DeleteBoat(inactiveBoatCode) as ObjectResult;
        Assert.AreEqual(StatusCodes.Status404NotFound, result!.StatusCode);
    }
    
    [Test]
    public void Test_Code()
    {
        BoatModel model = new BoatModel();
        var result = _boatRepositoryController.GetAllBoats() as ObjectResult;
        List<BoatObject> listToConvert = (List<BoatObject>)result.Value;

        //ensure 9 and z are reset and 4 is incremented
        BoatObject codeToTestReset = new BoatObject();
        codeToTestReset.Code = "ABCD-1234-z9";
        listToConvert.Add(codeToTestReset);
        
        var newCode = model.GetNewCode(mapper.Map<List<Boat>>(listToConvert));
        Assert.AreEqual("ABCD-1235-A0", newCode);
        
        codeToTestReset = new BoatObject();
        codeToTestReset.Code = "Zzzz-9999-z9";
        listToConvert.Add(codeToTestReset);

        newCode = model.GetNewCode(mapper.Map<List<Boat>>(listToConvert));
        Assert.AreEqual("aAAA-0000-A0", newCode);
    }
}