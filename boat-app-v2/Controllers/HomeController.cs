using boat_app_v2.BusinessLogic.Repository;
using boat_app_v2.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace boat_app_v2.Controllers;

public class HomeController : Controller
{ 
    private readonly BoatModel _boatModel = new BoatModel(); 
    private readonly IRepositoryController _repository;
    
    public HomeController(IRepositoryController repository)
    {
        this._repository = repository;
    }

    [Route("boats")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult AllBoats()
    {
        return Json(_repository.BoatRepository.GetAllBoats());
    }
    
    [Route("boats/new")]
    [HttpPost]
    public ActionResult AddBoat(Boat boat)
    {
        boat.Code = _boatModel.GetNewCode(_repository.BoatRepository.GetAllBoats().Last());

        _repository.BoatRepository.CreateBoat(boat);
        _repository.Save();
        
        return Content("Success :)");
    }
    
    [Route("boats/update")]
    [HttpPost]
    public ActionResult UpdateBoat(Boat boat)
    {
        _repository.BoatRepository.UpdateBoat(boat);
        _repository.Save();

        return Content("Success :)");
    }
    
    [Route("boats/delete")]
    [HttpPost]
    public ActionResult DeleteBoat(Boat boat)
    {
        _repository.BoatRepository.DeleteBoat(boat);
        _repository.Save();

        return Content("Success :)");
    }

    public IActionResult Index()
    {
        return View(_repository.BoatRepository.GetAllBoats());
    }
}