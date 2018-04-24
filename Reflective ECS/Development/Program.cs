using Development.Testing;

namespace Development
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var performanceTesting = new PerformanceTester(
                frameCount: 50,
                systemCount: 50,
                entityCount: 10_000,
                componentPerEntity: 10,
                clearConsoleBeforeFinal: true,
                beepOnEnd: true
                );

            performanceTesting.Run();
        }
    }
}
