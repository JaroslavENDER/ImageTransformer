using System.Drawing;

namespace Kontur.ImageTransformer.Transformer
{
    internal static class Filter
    {
        public static Img SetGrayscale(Img image)
        {
            for (var x = 0; x < image.Size.Width; x++)
                for (var y = 0; y < image.Size.Height; y++)
                {
                    var newIntensity = (image[x, y].R + image[x, y].G + image[x, y].B) / 3;
                    image[x, y] = Color.FromArgb(image[x, y].A, newIntensity, newIntensity, newIntensity);
                }
            return image;
        }

        public static Img SetThreshold(Img image, int param)
        {
            for (var x = 0; x < image.Size.Width; x++)
                for (var y = 0; y < image.Size.Height; y++)
                {
                    var newIntensity = (image[x, y].R + image[x, y].G + image[x, y].B) / 3;
                    image[x, y] = newIntensity >= 255 * param / 100
                        ? Color.FromArgb(image[x, y].A, 255, 255, 255)
                        : Color.FromArgb(image[x, y].A, 0, 0, 0);
                }
            return image;
        }

        public static Img SetSepia(Img image)
        {
            for (var x = 0; x < image.Size.Width; x++)
                for (var y = 0; y < image.Size.Height; y++)
                {
                    var newIntensity = (image[x, y].R + image[x, y].G + image[x, y].B) / 3;
                    int r = (int)(image[x, y].R * .393 + image[x, y].G * .769 + image[x, y].B * .189);
                    int g = (int)(image[x, y].R * .349 + image[x, y].G * .686 + image[x, y].B * .168);
                    int b = (int)(image[x, y].R * .272 + image[x, y].G * .534 + image[x, y].B * .131);
                    image[x, y] = Color.FromArgb(image[x, y].A, r, g, b);
                }
            return image;
        }
    }
}
