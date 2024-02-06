using boat_app_v2.Controllers;
using boat_app_v2.Services;

namespace boat_app_v2.tests.UnitTests;

public class RuleUnitTests
{
    private RuleService _ruleService;

    [SetUp]
    public void Setup()
    {
        _ruleService = new RuleService();
        //_ruleController = new RuleController(logger, );
    }
}