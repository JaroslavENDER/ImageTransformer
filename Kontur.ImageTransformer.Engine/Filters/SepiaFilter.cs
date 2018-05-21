using System.Drawing;
using System.Drawing.Imaging;

namespace Kontur.ImageTransformer.Engine.Filters
{
    public class SepiaFilter : ISepiaFilter
    {
        public unsafe void Process(Bitmap image)
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
