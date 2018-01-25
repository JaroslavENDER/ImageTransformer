using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace Kontur.ImageTransformer.Transformer.StressTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (var a = 0; a < 4; a++)
            {
                SampleTest();
            }
            Console.ReadLine();
        }

        private static void SampleTest()
        {
            var watch = new Stopwatch();

            watch.Restart();
            var bitmap = new Bitmap("TestImage.png");
            var image = ImgConverter.ConvertFromBitmap(bitmap);
            ImgConverter.ConvertToBitmap(image).Save("ResultTestImage.png");
            Console.WriteLine(watch.Elapsed.TotalSeconds);

            watch.Restart();
            var bytes = File.ReadAllBytes("TestImage.png");
            var image2 = ImgConverter.ConvertFromBytes(bytes);
            image2 = Filter.SetGrayscale(image2);
            ImgConverter.ConvertToBitmap(image2).Save("ResultTestImage.png");
            Console.WriteLine(watch.Elapsed.TotalSeconds);

            Console.WriteLine(new string('-', 50));
        }
    }
}
