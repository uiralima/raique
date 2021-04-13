namespace DependencyInjectionTests.Types
{
    public class TestType2 : TestTypeBase, ITestInterface
    {
        public TestType2(IInjectedType injected)
        { }
    }
}
