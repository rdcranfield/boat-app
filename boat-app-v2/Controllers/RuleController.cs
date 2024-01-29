using boat_app_v2.Services;

namespace boat_app_v2.Controllers;

public class RuleController
{
    private ILogger<RuleController> _logger; 
    private RuleService _service;
        
    public RuleController(ILogger<RuleController> logger, RuleService service) 
    { 
        _logger = logger; 
        _service = service; 
    }
}