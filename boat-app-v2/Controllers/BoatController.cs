using boat_app_v2.BusinessLogic;
using boat_app_v2.Entities.Models;
using boat_app_v2.Models;
using Microsoft.AspNetCore.Mvc;

namespace boat_app_v2.Controllers;

public class BoatController : ControllerBase
{
    private readonly BoatContext _context;

    public BoatController(BoatContext context)
        => _context = context;

    [HttpGet]
    public ActionResult<Boat[]> GetAllBoats()
        => _context.Boats.OrderBy(b => b.Code).ToArray();
}