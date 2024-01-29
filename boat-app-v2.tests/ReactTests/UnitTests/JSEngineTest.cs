using JavaScriptEngineSwitcher.Core;
using Moq;
using React;
using Xunit;

namespace boat_app_v2.tests.ReactTests.UnitTests;

public class JSEngineTest
{
    [Fact]
    public void Test_JsonReturn()
    {
        var engine = new Mock<IJsEngine>();
        engine.Object.CallFunctionReturningJson<int>("hello world");
        engine.Verify(x => x.CallFunction<int>("hello world"));
    }
}