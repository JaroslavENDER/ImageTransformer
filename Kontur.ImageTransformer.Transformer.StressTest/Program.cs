using System;
using System.Diagnostics;
using System.Drawing;

namespace Kontur.ImageTransformer.Transformer.StressTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var outWatch = new Stopwatch();
            var innerWatch = new Stopwatch();
            outWatch.Start();
            for (var a = 0; a < 4; a++)
            {
                innerWatch.Restart();
                SampleTest();
                innerWatch.Stop();
                Console.WriteLine(innerWatch.Elapsed.TotalSeconds);
            }
            outWatch.Stop();
            Console.WriteLine("Total: " + outWatch.Elapsed.TotalSeconds);
            Console.ReadLine();
        }

        private static void SampleTest()
        {
            var image = new Bitmap("TestImage.jpg");
            image = Filter.SetGrayscale(image);
            image.Save("ResultTestImage.jpg");
        }
    }
}
