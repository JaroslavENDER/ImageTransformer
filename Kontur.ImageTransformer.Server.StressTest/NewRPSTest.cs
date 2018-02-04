using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Server.StressTest
{
    internal class NewRPSTest : ITest
    {
        private Ender.PercentilCounter<double> percentilCounter = new Ender.PercentilCounter<double>(20);
        private object block = new object();
        private int counter = 0;
        private double requestPerSecondCount = 10;
        private bool pause = false;

        public double Invoke()
        {
            var image = File.ReadAllBytes("TestImage.png");
            var counterTask = Task.Run(() => Counter());
            while (true)
            {
                if (pause)
                {
                    Thread.Sleep(100);
                    continue;
                }
                Task.Run(() => DoRequest(image));
                Thread.Sleep((int)(1000 / requestPerSecondCount));
                var a = counterTask.Status;
            }
        }

        private void Counter()
        {
            var localCounter = 0;
            Thread.Sleep(900);
            while (true)
            {
                var latency = percentilCounter.GetPercentil();
                Console.WriteLine(string.Format("Latency: {0:0.0000}; RPS: {1}; RequestCount: {2:0.0}", latency, counter - localCounter, requestPerSecondCount));
                localCounter = counter;
                percentilCounter.Restart();
                if (latency > 1)
                {
                    requestPerSecondCount -= 0.5;
                    pause = true;
                    Thread.Sleep(5000);
                    pause = false;
                }
                else
                {
                    requestPerSecondCount += 0.2;
                    Thread.Sleep(999);
                }
            }
        }

        private void DoRequest(byte[] image)
        {
            var watch = new Stopwatch();

            var request = WebRequest.Create("http://localhost:8080");
            request.Method = "POST";
            request.GetRequestStream().Write(image, 0, image.Length);

            watch.Restart();
            request.GetResponse();
            lock (block)
            {
                //Console.WriteLine(watch.Elapsed.TotalSeconds);
                percentilCounter.Add(watch.Elapsed.TotalSeconds);
                counter++;
            }
        }
    }
}
