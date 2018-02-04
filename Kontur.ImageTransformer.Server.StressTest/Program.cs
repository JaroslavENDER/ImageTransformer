using System;

namespace Kontur.ImageTransformer.Server.StressTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new NewRPSTest().Invoke();

            Console.ReadLine();
        }
    }
}
