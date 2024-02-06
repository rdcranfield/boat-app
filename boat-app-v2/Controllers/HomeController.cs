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
    public async Task<IActionResult> AllBoats()
    {
        var allBoats = await _repository.BoatRepository.GetAllBoatsAsync();
        return Json(allBoats);
    }
    
    [Route("boats/new")]
    [HttpPost]
    public async Task<IActionResult> AddBoat(Boat boat)
    {
        var lastBoat = await _repository.BoatRepository.GetAllBoatsAsync();
        boat.Code = _boatModel.GetNewCode(lastBoat.Last());

        _repository.BoatRepository.CreateBoat(boat);
        await _repository.SaveAsync();
        
        return Content("Success :)");
    }
    
    [Route("boats/update")]
    [HttpPost]
    public async Task<IActionResult> UpdateBoat(Boat boat)
    {
        _repository.BoatRepository.UpdateBoat(boat);
        await _repository.SaveAsync();

        return Content("Success :)");
    }
    
    [Route("boats/delete")]
    [HttpPost]
    public async Task<IActionResult> DeleteBoat(Boat boat)
    {
        _repository.BoatRepository.DeleteBoat(boat);
        await _repository.SaveAsync();

        return Content("Success :)");
    }

    public async Task<IActionResult> Index()
    {
        var allBoats = await _repository.BoatRepository.GetAllBoatsAsync();
        return View(allBoats);
    }
}