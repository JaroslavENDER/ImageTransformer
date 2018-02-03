using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kontur.ImageTransformer.Transformer
{
    public static class Filter
    {
        [Obsolete("Use SetGrayscale(Bitmap image)", error: false)]
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
        public static unsafe void SetGrayscale(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            
            byte R, G, B;
            var imageData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            try
            {
                byte* curpos;
                for (var h = 0; h < height; h++)
                {
                    curpos = ((byte*)imageData.Scan0) + h * imageData.Stride;
                    for (var w = 0; w < width; w++)
                    {
                        B = *(curpos++);
                        G = *(curpos++);
                        R = *(curpos++);
                        var middle = (byte)((R + G + B) / 3);
                        curpos -= 3;
                        *(curpos++) = middle;
                        *(curpos++) = middle;
                        *(curpos++) = middle;
                        curpos++;
                    }
                }
            }
            finally
            {
                image.UnlockBits(imageData);
            }
        }

        [Obsolete("Use SetThreshold(Bitmap image, int param)", error: false)]
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
        public static unsafe void SetThreshold(Bitmap image, int param)
        {
            int width = image.Width;
            int height = image.Height;

            byte R, G, B;
            var imageData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)imageData.Scan0) + h * imageData.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        B = *(curpos++);
                        G = *(curpos++);
                        R = *(curpos++);
                        var newValue = ((R + G + B) / 3) >= (255 * param / 100)
                            ? (byte)255
                            : (byte)0;
                        curpos -= 3;
                        *(curpos++) = newValue;
                        *(curpos++) = newValue;
                        *(curpos++) = newValue;
                        curpos++;
                    }
                }
            }
            finally
            {
                image.UnlockBits(imageData);
            }
        }

        [Obsolete("Use SetSepia(Bitmap image)", error: false)]
        public static Img SetSepia(Img image)
        {
            for (var x = 0; x < image.Size.Width; x++)
                for (var y = 0; y < image.Size.Height; y++)
                {
                    int r = (int)(image[x, y].R * .393 + image[x, y].G * .769 + image[x, y].B * .189);
                    int g = (int)(image[x, y].R * .349 + image[x, y].G * .686 + image[x, y].B * .168);
                    int b = (int)(image[x, y].R * .272 + image[x, y].G * .534 + image[x, y].B * .131);
                    image[x, y] = Color.FromArgb(
                        image[x, y].A,
                        r > 255 ? 255 : r,
                        g > 255 ? 255 : g,
                        b > 255 ? 255 : b);
                }
            return image;
        }
        public static unsafe void SetSepia(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;

            byte R, G, B;
            int r, g, b;
            var imageData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)imageData.Scan0) + h * imageData.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        B = *(curpos++);
                        G = *(curpos++);
                        R = *(curpos++);
                        b = (int)(R * .272 + G * .534 + B * .131);
                        g = (int)(R * .349 + G * .686 + B * .168);
                        r = (int)(R * .393 + G * .769 + B * .189);
                        curpos -= 3;
                        *(curpos++) = b > 255 ? (byte)255 : (byte)b;
                        *(curpos++) = g > 255 ? (byte)255 : (byte)g;
                        *(curpos++) = r > 255 ? (byte)255 : (byte)r;
                        curpos++;
                    }
                }
            }
            finally
            {
                image.UnlockBits(imageData);
            }
        }
    }
}
