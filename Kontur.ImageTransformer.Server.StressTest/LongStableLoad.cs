using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Server.StressTest
{
    class LongStableLoad : ITest
    {
        private Ender.PercentilCounter<double> percentilCounter = new Ender.PercentilCounter<double>(20);
        private object block = new object();
        private int counter = 0;
        private double requestPerSecondCount = 30;
        private int realyRequestPerSecondCount = 0;

        public double Invoke()
        {
            var image = File.ReadAllBytes("TestImage.png");
            var counterTask = Task.Run(() => Counter());
            while (true)
            {
                Task.Run(() => DoRequest(image));
                Interlocked.Increment(ref realyRequestPerSecondCount);
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
                Console.WriteLine(string.Format("Latency: {0:0.0000}; RPS: {1}; RequestCount: {2} ({3:0.0})", latency, counter - localCounter, realyRequestPerSecondCount, requestPerSecondCount));
                Interlocked.Exchange(ref realyRequestPerSecondCount, 0);
                localCounter = counter;
                percentilCounter.Restart();
                Thread.Sleep(999);
            }
        }

        private void DoRequest(byte[] image)
        {
            var watch = new Stopwatch();

            var request = WebRequest.Create("http://localhost:8080");
            request.Method = "POST";
            request.GetRequestStream().Write(image, 0, image.Length);

            watch.Restart();
            var status = 0;
            try { status = (int)(request.GetResponse() as HttpWebResponse).StatusCode; }
            //catch (WebException ex) { Console.WriteLine(ex.Message.Split('(', ')')[1]); }
            catch { }
            if (status == 200)
            {
                lock (block)
                {
                    percentilCounter.Add(watch.Elapsed.TotalSeconds);
                    counter++;
                }
            }
        }
    }
}
