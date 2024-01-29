using boat_app_v2.Entities.Models;
using boat_app_v2.Services;

namespace boat_app_v2.Models;

public class BoatModel
{
    private RuleService rules = new RuleService();

    public string GetNewCode(IEnumerable<Boat> boats)
    {
        var x = boats.OrderBy(b => b.Code).ToArray();
        return rules.IncrementBoatCode(x[boats.Count()-1].Code);
    }
}