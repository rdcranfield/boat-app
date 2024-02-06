using boat_app_v2.Services;

namespace boat_app_v2.Entities.Models;

public class BoatModel
{
    private readonly RuleService _rules = new RuleService();

    public string GetNewCode(Boat? boat)
    {
        return _rules.IncrementBoatCode(boat!.Code!);
    }
}