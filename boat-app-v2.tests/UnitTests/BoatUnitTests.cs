using System.Diagnostics;
using AutoMapper;
using boat_app_v2.Controllers;
using boat_app_v2.Entities.DataTransferObjects;
using boat_app_v2.Entities.Models;
using boat_app_v2.Services;
using boat_app_v2.tests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace boat_app_v2.tests.UnitTests;

public class Tests
{
    private BoatRepositoryController? _boatRepositoryController;

    private const string InactiveBoatCode = "ABCD-1234-A1";
    private const string ActiveBoatCode = "ABCD-1234-C1";
    private IMapper? _mapper;

    private static IMapper GetMapper()
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
        _mapper = GetMapper();

        _boatRepositoryController = new BoatRepositoryController(logger, repositoryControllerMock.Object, _mapper);
    }

    [Test]
    public void Test_GetAllBoats()
    {
        var result = _boatRepositoryController!.GetAllBoats().Result as ObjectResult;
        Assert.NotNull(result);
        Assert.That(result!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.IsAssignableFrom<List<BoatObject>>(result.Value);
    }
    
    
    [Test]
    public void Test_GetBoatById()
    {
        //test inactive code
        Debug.Assert(_boatRepositoryController != null, nameof(_boatRepositoryController) + " != null");
        var result = _boatRepositoryController.GetBoatById(InactiveBoatCode).Result as ObjectResult;
        Assert.That(result!.StatusCode,  Is.EqualTo(StatusCodes.Status404NotFound));
        var boat = result.Value as Boat;
        Assert.That(boat,  Is.EqualTo(null));

        //test active code
        result = _boatRepositoryController.GetBoatById(ActiveBoatCode).Result as ObjectResult;
        Assert.That(result!.StatusCode,  Is.EqualTo(StatusCodes.Status200OK));
        Assert.NotNull(result);
        boat = result.Value as Boat;
        Assert.That(boat?.Name,  Is.EqualTo("Sir Jon's Cabin Cruiser"));
    }
    
    [Test]
    public void Test_CreateBoat()
    {
        //not really needed, considering user never defines the code, the back-end does, but just to be extra safe
        var incorrectCode = "123177&&";
        var createBoat = new Boat
        {
            Code = incorrectCode
        };
        
        //test incorrect code
        var result = _boatRepositoryController?.CreateBoat(createBoat).Result as ObjectResult;
        Assert.That(result!.StatusCode,  Is.EqualTo(StatusCodes.Status400BadRequest));

        //test unique code, try create boat with an existing code
        createBoat = new Boat
        {
            Code = ActiveBoatCode
        };
        result = _boatRepositoryController?.CreateBoat(createBoat).Result as ObjectResult;
        Assert.That(result!.StatusCode,  Is.EqualTo(StatusCodes.Status409Conflict));
        
        //test new code, ´
        createBoat = new Boat
        {
            Code = InactiveBoatCode
        };
        result = _boatRepositoryController?.CreateBoat(createBoat).Result as ObjectResult;
        Assert.That(result!.StatusCode,  Is.EqualTo(StatusCodes.Status200OK));
        Assert.NotNull(result);
        var boat = result.Value as Boat;
        Assert.NotNull(boat?.Code);
        Assert.That(boat?.Code, Is.Not.EqualTo(ActiveBoatCode));
        Assert.That(boat?.Code,  Is.EqualTo(InactiveBoatCode));
    }
    
    
    [Test]
    public void Test_UpdateBoat()
    {
        // test update on existing boat
        var result = _boatRepositoryController?.GetBoatById(ActiveBoatCode).Result as ObjectResult;
        var boat = result!.Value as Boat;
        boat!.Name = "test";
        
        result = _boatRepositoryController?.UpdateBoat(boat).Result as ObjectResult;
        Assert.NotNull(result);
        Assert.That(result!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        boat = result.Value as Boat;
        Assert.That(boat!.Name,  Is.EqualTo("test"));
        
        // test null update
        result = _boatRepositoryController?.UpdateBoat(null).Result as ObjectResult;
        Assert.That(result!.StatusCode,  Is.EqualTo(StatusCodes.Status400BadRequest));

        // test update on boat that does not exist
        var imaginaryBoat = new Boat
        {
            Code = InactiveBoatCode
        };
        result = _boatRepositoryController?.UpdateBoat(imaginaryBoat).Result as ObjectResult;
        Assert.That(result!.StatusCode,  Is.EqualTo(StatusCodes.Status404NotFound));
    }
    
    [Test]
    public void Test_DeleteBoat()
    {
        var result = _boatRepositoryController?.DeleteBoat(ActiveBoatCode).Result as ObjectResult;
        Assert.That(result!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        
        result = _boatRepositoryController?.DeleteBoat(InactiveBoatCode).Result as ObjectResult;
        Assert.That(result!.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
    }
    
    [Test]
    public void Test_Code()
    {
        BoatModel model = new BoatModel();
        var result = _boatRepositoryController?.GetAllBoats().Result as ObjectResult;
        var listToConvert = (List<BoatObject>)result!.Value!;

        //ensure 9 and z are reset and 4 is incremented
        var codeToTestReset = new BoatObject
        {
            Code = "ABCD-1234-z9"
        };
        listToConvert.Add(codeToTestReset);

        if (_mapper == null) return;
        var newCode = model.GetNewCode(_mapper.Map<Boat>(listToConvert.Last()));
        Assert.That(newCode,  Is.EqualTo("ABCD-1235-A0"));
        
        codeToTestReset = new BoatObject
        {
            Code = "Zzzz-9999-z9"
        };
        listToConvert.Add(codeToTestReset);

        newCode = model.GetNewCode(_mapper.Map<Boat>(listToConvert.Last()));
        Assert.That(newCode,  Is.EqualTo("aAAA-0000-A0"));
    }
}