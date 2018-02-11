using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Server.StressTest
{
    internal class MaxStressTest : ITest
    {
        public double Invoke()
        {
            var image = File.ReadAllBytes("TestImage.png");
            while (true)
                Task.Run(() => DoRequest(image));
        }

        private void DoRequest(byte[] image)
        {
            var watch = new Stopwatch();

            var request = WebRequest.Create("http://localhost:8080");
            request.Method = "POST";
            request.GetRequestStream().Write(image, 0, image.Length);

            watch.Restart();
            request.GetResponse();
            Console.WriteLine(watch.Elapsed.TotalSeconds);
        }
    }
}
