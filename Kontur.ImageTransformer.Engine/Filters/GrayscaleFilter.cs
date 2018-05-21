using System.Drawing;
using System.Drawing.Imaging;

namespace Kontur.ImageTransformer.Engine.Filters
{
    public class GrayscaleFilter : IGrayscaleFilter
    {
        public unsafe void Process(Bitmap image)
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
    }
}
