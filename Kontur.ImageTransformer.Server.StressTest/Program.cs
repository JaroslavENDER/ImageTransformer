using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Server.StressTest
{
    class Program
    {
        private readonly static string testImage = "TestImage.png";
        private readonly static string resultTestImage = "ResultTestImage.png";

        static void Main(string[] args)
        {
            RPSTest();

            Console.ReadLine();
        }

        private static void MaxStressTest()
        {
            var image = File.ReadAllBytes(testImage);
            while (true)
                Task.Run(() => DoRequest(image));
        }

        private static void RPSTest()
        {
            var percentilCounter = new Ender.PercentilCounter<double>(20);
            foreach (var rps in percentilCounter.RunTestAndGetResult(OneRPSTest, 40))
                Console.WriteLine(rps);
        }

        private static double OneRPSTest()
        {
            var watch = new Stopwatch();
            var time = 0d;
            var maxRPS = 0;
            var image = File.ReadAllBytes(testImage);

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

        private static void DoRequest(byte[] image)
        {
            var watch = new Stopwatch();

            var request = WebRequest.Create("http://localhost:8080");
            request.Method = "POST";
            request.GetRequestStream().Write(image, 0, image.Length);

            watch.Restart();
            request.GetResponse();
            //Console.WriteLine(watch.Elapsed.TotalSeconds);
        }

        private static void SimpleLatencyTest()
        {
            var percentilCounter = new Ender.PercentilCounter<double>(80);
            foreach (var time in percentilCounter.RunTestAndGetResult(SimpleTest, 1000))
                Console.WriteLine(time);
        }

        private static double SimpleTest()
        {
            var watch = new Stopwatch();
            
            var request = WebRequest.Create("http://localhost:8080/process") as HttpWebRequest;
            request.Method = "POST";
            Image.FromFile(testImage).Save(request.GetRequestStream(), ImageFormat.Png);

            watch.Restart();
            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                var time = watch.Elapsed.TotalSeconds;
                Console.Write(time + "\t");
                Image.FromStream(responseStream).Save(resultTestImage, ImageFormat.Png);
                return time;
            }
        }
    }
}
