using System.Drawing;

namespace Kontur.ImageTransformer.Transformer
{
    internal static class Filter
    {
        public static Bitmap SetGrayscale(Bitmap image)
        {
            for (var x = 0; x < image.Size.Width; x++)
                for (var y = 0; y < image.Size.Height; y++)
                {
                    var newIntensity = (image.GetPixel(x, y).R + image.GetPixel(x, y).G + image.GetPixel(x, y).B) / 3;
                    image.SetPixel(x, y, Color.FromArgb(image.GetPixel(x, y).A, newIntensity, newIntensity, newIntensity));
                }
            return image;
        }

        public static Bitmap SetThreshold(Bitmap image, int param)
        {
            for (var x = 0; x < image.Size.Width; x++)
                for (var y = 0; y < image.Size.Height; y++)
                {
                    var newIntensity = (image.GetPixel(x, y).R + image.GetPixel(x, y).G + image.GetPixel(x, y).B) / 3;
                    image.SetPixel(x, y, newIntensity >= 255 * param / 100
                        ? Color.FromArgb(image.GetPixel(x, y).A, 255, 255, 255)
                        : Color.FromArgb(image.GetPixel(x, y).A, 0, 0, 0));
                }
            return image;
        }

        public static Bitmap SetSepia(Bitmap image)
        {
            for (var x = 0; x < image.Size.Width; x++)
                for (var y = 0; y < image.Size.Height; y++)
                {
                    var newIntensity = (image.GetPixel(x, y).R + image.GetPixel(x, y).G + image.GetPixel(x, y).B) / 3;
                    int r = (int)(image.GetPixel(x, y).R * .393 + image.GetPixel(x, y).G * .769 + image.GetPixel(x, y).B * .189);
                    int g = (int)(image.GetPixel(x, y).R * .349 + image.GetPixel(x, y).G * .686 + image.GetPixel(x, y).B * .168);
                    int b = (int)(image.GetPixel(x, y).R * .272 + image.GetPixel(x, y).G * .534 + image.GetPixel(x, y).B * .131);
                    image.SetPixel(x, y, Color.FromArgb(image.GetPixel(x, y).A, r, g, b));
                }
            return image;
        }
    }
}
