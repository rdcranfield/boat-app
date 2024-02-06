using System.Text.RegularExpressions;
using AutoMapper;
using boat_app_v2.BusinessLogic.Repository;
using boat_app_v2.Entities.DataTransferObjects;
using boat_app_v2.Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace boat_app_v2.Controllers;

public class BoatRepositoryController: ControllerBase
{
    private readonly ILogger<BoatRepositoryController> _logger;
    private IRepositoryController Repository { get; }
    private IMapper Mapper { get; }

    public BoatRepositoryController(ILogger<BoatRepositoryController> logger, IRepositoryController repository, IMapper mapper) 
    { 
        _logger = logger; 
        Repository = repository; 
        Mapper = mapper;
    }

   
    [HttpGet] 
    public async Task<IActionResult> GetAllBoats() 
    { 
        try
        {
            var boats = await Repository.BoatRepository.GetAllBoatsAsync();

            // map to data transfer object, only used for testing
            var result = Mapper.Map<IEnumerable<BoatObject>>(boats);
            
            return Ok(result); 
        } 
        catch (Exception ex) 
        { 
            _logger.LogError($"Something went wrong inside GetAllBoats action: {ex.Message}"); 
            
            return StatusCode(500, "Internal server error"); 
        } 
    }
    
    [HttpGet] 
    public async Task<IActionResult> GetBoatById(string id) 
    { 
        try 
        { 
            var boat = await Repository.BoatRepository.GetBoatByIdAsync(id); 
            _logger.LogInformation("Returning boat by Id from database.");
            
            if (boat == null) return NotFound(boat);
            
            return Ok(boat); 
        } 
        catch (Exception ex) 
        { 
            _logger.LogError($"Something went wrong inside GetBoatById action: {ex.Message}"); 
                
            return StatusCode(500, "Internal server error"); 
        } 
    }
    
    [HttpPost] 
    public async Task<IActionResult> CreateBoat([FromBody] Boat boat) 
    { 
        try
        {
            //not really needed, considering user never defines the code, the back-end does
            if (!Regex.Match(boat.Code!, @"^[A-Za-z]{4}-[0-9]{4}-[A-Za-z]{1}[0-9]{1}$", RegexOptions.IgnoreCase).Success)
            {
                return BadRequest(boat);
            }
            var aBoat = await Repository.BoatRepository.GetBoatByIdAsync(boat.Code!);

            if (aBoat != null)
            {
                return Conflict(boat);
            }
            
            Repository.BoatRepository.CreateBoat(boat);
            await Repository.SaveAsync();
            
            _logger.LogInformation("Created boat in database.");
            
            return Ok(boat); 
        } 
        catch (Exception ex) 
        { 
            _logger.LogError($"Something went wrong inside CreateBoat action: {ex.Message}"); 
                
            return StatusCode(500, "Internal server error"); 
        } 
    }
    
    [HttpPost] 
    public async Task<IActionResult> UpdateBoat([FromBody] Boat? boat) 
    { 
        try
        {
            if (boat == null) return BadRequest(boat);
            
            var aBoat = await Repository.BoatRepository.GetBoatByIdAsync(boat.Code!);
            if (aBoat == null) return NotFound(boat);
            
            Repository.BoatRepository.UpdateBoat(boat);
            await Repository.SaveAsync();
            
            _logger.LogInformation("Updated boat in database.");
            
            return Ok(boat); 
        } 
        catch (Exception ex) 
        { 
            _logger.LogError($"Something went wrong inside UpdateBoat action: {ex.Message}"); 
                
            return StatusCode(500, "Internal server error"); 
        } 
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoat(string id)
    {
        try
        {
            var boat = await Repository.BoatRepository.GetBoatByIdAsync(id);

            if (boat == null)
            {
                _logger.LogError($"Boat with id: {id}, not found in the db.");
                return NotFound(id);
            }

            Repository.BoatRepository.DeleteBoat(boat);
            await Repository.SaveAsync();

            return Ok(id); 
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside DeleteBoat action: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}