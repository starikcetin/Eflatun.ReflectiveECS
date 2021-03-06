﻿using Eflatun.ReflectiveECS.Development.Testing;

namespace Eflatun.ReflectiveECS.Development
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var performanceTesting = new PerformanceTester(
                frameCount: 20,
                systemCount: 50,
                entityCount: 100_000,
                componentPerEntity: 10,
                clearConsoleBeforeFinal: true,
                beepOnEnd: true
                );

            performanceTesting.Run();

            var correctnessTester = new CorrectnessTester();
            correctnessTester.Run();
        }
    }
}
