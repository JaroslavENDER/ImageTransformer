using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kontur.ImageTransformer.Engine
{
    [Obsolete("Use IFilter", error: false)] //Сделаем вид, что я не переименовывал проект ;) / version 3.0.0.0 -> 3.1.0.0
    public static class Filter
    {
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
