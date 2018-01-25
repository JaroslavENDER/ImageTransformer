using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace Kontur.ImageTransformer.Transformer.Test
{
    [TestClass]
    public class SnippingToolTest
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
        public void NormalCut()
        {
            var image = SnippingTool.Cut(GetTestImage(), 2, 3, 4, 2);
            Assert.AreEqual(4, image.Size.Width);
            Assert.AreEqual(2, image.Size.Height);
            Assert.AreEqual(23, image[0, 0].R);
            Assert.AreEqual(54, image[3, 1].R);
        }

        [TestMethod]
        public void NegativeSize()
        {
            var image = SnippingTool.Cut(GetTestImage(), 4, 5, -3, -2);
            Assert.AreEqual(3, image.Size.Width);
            Assert.AreEqual(2, image.Size.Height);
            Assert.AreEqual(24, image[0, 0].R);
            Assert.AreEqual(45, image[2, 1].R);
        }

        [TestMethod]
        public void SlimCut()
        {
            var image = SnippingTool.Cut(GetTestImage(), 0, 0, 1, 6);
            Assert.AreEqual(1, image.Size.Width);
            Assert.AreEqual(6, image.Size.Height);
            Assert.AreEqual(0, image[0, 0].R);
            Assert.AreEqual(5, image[0, 5].R);
        }
    }
}
