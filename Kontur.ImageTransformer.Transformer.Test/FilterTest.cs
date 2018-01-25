using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Kontur.ImageTransformer.Transformer.Test
{
    [TestClass]
    public class FilterTest
    {
        private Img GetTestImage()
        {
            var image = new Img(10, 6);
            for (var x = 0; x < image.Size.Width; x++)
                for (var y = 0; y < image.Size.Height; y++)
                    image[x, y] = Color.FromArgb(1, x * 10 + y, x * 10 + y, x * 10 + y);
            return image;
        }

        [TestMethod]
        public void GrayscaleTest()
        {
            var image = Filter.SetGrayscale(GetTestImage());
            Assert.AreEqual(0, image[0, 0].R);
            Assert.AreEqual(45, image[4, 5].R);
        }

        [TestMethod]
        public void ThresholdTest()
        {
            var image = Filter.SetThreshold(GetTestImage(), 2);
            Assert.AreEqual(255, image[4, 5].R);
            Assert.AreEqual(0, image[0, 1].R);
            Assert.AreEqual(255, image[2, 2].R);
        }
    }
}
