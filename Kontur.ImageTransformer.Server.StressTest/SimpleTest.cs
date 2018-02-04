using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace Kontur.ImageTransformer.Server.StressTest
{
    internal class SimpleTest : ITest
    {
        public double Invoke()
        {
            var watch = new Stopwatch();

            var request = WebRequest.Create("http://localhost:8080/process") as HttpWebRequest;
            request.Method = "POST";
            Image.FromFile("TestImage.png").Save(request.GetRequestStream(), ImageFormat.Png);

            watch.Restart();
            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                var time = watch.Elapsed.TotalSeconds;
                Console.Write(time + "\t");
                Image.FromStream(responseStream).Save("ResultTestImage.png", ImageFormat.Png);
                return time;
            }
        }
    }
}
