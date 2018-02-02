using Kontur.ImageTransformer.Transformer;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Kontur.ImageTransformer.Server.StressTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var percentilCounter = new Ender.PercentilCounter<double>(80);
            foreach (var time in percentilCounter.RunTestAndGetResult(SampleTest, 40))
                Console.WriteLine(time);

            Console.ReadLine();
        }

        private static double SampleTest()
        {
            var watch = new Stopwatch();
            watch.Restart();
            var bytes = File.ReadAllBytes("TestImage.png");
            var request = WebRequest.Create("http://localhost:8080/") as HttpWebRequest;
            request.Method = "POST";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                var result = ImgConverter.ConvertFromStream(responseStream);
                ImgConverter.ConvertToBitmap(result).Save("ResultTestImage.png");
            }

            var time = watch.Elapsed.TotalSeconds;
            Console.Write(time + "\t");
            return time;
        }
    }
}
