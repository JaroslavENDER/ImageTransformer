using System;

namespace Kontur.ImageTransformer.Server.StressTest
{
    internal class SimpleLatencyTest : ITest
    {
        public double Invoke()
        {
            var simpleTest = new SimpleTest();
            var percentilCounter = new Ender.PercentilCounter<double>(80);
            foreach (var time in percentilCounter.RunTestAndGetResult(simpleTest.Invoke, 1000))
                Console.WriteLine(time);
            return 0;
        }
    }
}
