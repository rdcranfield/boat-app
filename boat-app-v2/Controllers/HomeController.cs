using AutoMapper;
using boat_app_v2.Entities.Models;
using boat_app_v2.Models;
using boat_app_v2.Services;
using Microsoft.AspNetCore.Mvc;

namespace boat_app_v2.Controllers;

public class HomeController : Controller
{ 
    
    private BoatModel _boatModel = new BoatModel(); 
    private readonly IMapper mapper;
    private BoatService _service;
    
    public HomeController(BoatService service)
    {
        mapper = GetMapper();
        _service = service;
        //_repository = repository;
    }

    public static IMapper GetMapper()
    {
        var mappingProfile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
        return new Mapper(configuration);
    }

    [Route("boats")]
   // [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult AllBoats()
    {
        return Json(_service.repository.BoatRepository.GetAllBoats());
    }
    
    [Route("boats/new")]
    [HttpPost]
    public ActionResult AddBoat(Boat boat)
    {
        boat.Code = _boatModel.GetNewCode(_service.repository.BoatRepository.GetAllBoats());

        _service.repository.BoatRepository.CreateBoat(boat);
        _service.repository.Save();
        
        return Content("Success :)");
    }
    
    [Route("boats/update")]
    [HttpPost]
    public ActionResult UpdateBoat(Boat boat)
    {
        _service.repository.BoatRepository.UpdateBoat(boat);
        _service.repository.Save();

        return Content("Success :)");
    }
    
    [Route("boats/delete")]
    [HttpPost]
    public ActionResult DeleteBoat(Boat boat)
    {
        _service.repository.BoatRepository.DeleteBoat(boat);
        _service.repository.Save();

        return Content("Success :)");
    }

    // GET
    public IActionResult Index()
    {
        return View(_service.repository.BoatRepository.GetAllBoats());
    }
}