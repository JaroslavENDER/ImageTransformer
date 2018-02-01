using System;
using System.Diagnostics;
using System.IO;

namespace Kontur.ImageTransformer.Transformer.StressTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            watch = new Stopwatch();
            var percentilCounter = new Ender.PercentilCounter<double>(80);
            foreach (var value in percentilCounter.RunTestAndGetResult(SampleTest, 40))
                Console.WriteLine(value);

            Console.ReadLine();
        }

        private static Stopwatch watch;
        private static double SampleTest()
        {
            watch.Restart();
            var image = ImgConverter.ConvertFromStream(File.OpenRead("TestImage.png"));
            image = Filter.SetGrayscale(image);
            ImgConverter.ConvertToBitmap(image).Save("ResultTestImage.png");
            var time = watch.Elapsed.TotalSeconds;
            Console.Write(time + "\t");
            return time;
        }
    }
}
