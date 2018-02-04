using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Server.StressTest
{
    internal class RPSTest : ITest
    {
        public double Invoke()
        {
            var percentilCounter = new Ender.PercentilCounter<double>(20);
            foreach (var rps in percentilCounter.RunTestAndGetResult(OneRPSTest, 40))
                Console.WriteLine(rps);
            return 0;
        }

        private double OneRPSTest()
        {
            var watch = new Stopwatch();
            var time = 0d;
            var maxRPS = 0;
            var image = File.ReadAllBytes("TestImage.png");

            do
            {
                maxRPS++;
                var tasks = new Task[maxRPS];
                watch.Restart();
                for (var i = 0; i < maxRPS; i++)
                    tasks[i] = Task.Run(() => DoRequest(image));
                Task.WaitAll(tasks);
                time = watch.Elapsed.TotalSeconds;
            }
            while (time < 1);
            Console.Write(maxRPS + "\t");
            return maxRPS;
        }

        private void DoRequest(byte[] image)
        {
            var watch = new Stopwatch();

            var request = WebRequest.Create("http://localhost:8080");
            request.Method = "POST";
            request.GetRequestStream().Write(image, 0, image.Length);

            watch.Restart();
            request.GetResponse();
        }
    }
}
