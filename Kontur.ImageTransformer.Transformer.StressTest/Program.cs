using System;
using System.Diagnostics;
using System.Drawing;

namespace Kontur.ImageTransformer.Transformer.StressTest
{
    internal class Program
    {
        private static readonly string testImage = "TestImage.png";
        private static readonly string resultTestImage = "ResultTestImage.png";

        static void Main(string[] args)
        {
            var percentilCounter = new Ender.PercentilCounter<double>(80);
            foreach (var value in percentilCounter.RunTestAndGetResult(SnippingToolTest, 1000))
                Console.WriteLine(value);

            Console.ReadLine();
        }
        
        private static double SampleTest()
        {
            var watch = new Stopwatch();
            watch.Restart();
            var image = Image.FromFile(testImage) as Bitmap;
            Filter.SetGrayscale(image);
            image.Save(resultTestImage);
            var time = watch.Elapsed.TotalSeconds;
            Console.Write(time + "\t");
            return time;
        }
        private static double SnippingToolTest()
        {
            var watch = new Stopwatch();
            var image = Image.FromFile(testImage) as Bitmap;
            watch.Restart();
            image = SnippingTool.Cut(image, new Rectangle(10, 10, image.Width - 20, image.Height - 20));
            var time = watch.Elapsed.TotalSeconds;
            Console.Write(time + "\t");
            return time;
        }
    }
}
