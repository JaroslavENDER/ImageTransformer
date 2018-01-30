using System;
using System.Diagnostics;
using System.IO;

namespace Kontur.ImageTransformer.Transformer.StressTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            for (var a = 0; a < 40; a++)
            {
                SampleTest(watch);
            }
            Console.ReadLine();
        }

        private static void SampleTest(Stopwatch watch)
        {
            watch.Restart();
            var image = ImgConverter.ConvertFromStream(File.OpenRead("TestImage.png"));
            image = Filter.SetGrayscale(image);
            File.WriteAllBytes("ResultTestImage.png", ImgConverter.ConvertToBytes(image));
            Console.WriteLine(watch.Elapsed.TotalSeconds);

            Console.WriteLine(new string('-', 50));
        }
    }
}
