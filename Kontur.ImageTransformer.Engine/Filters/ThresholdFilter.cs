using System.Drawing;
using System.Drawing.Imaging;

namespace Kontur.ImageTransformer.Engine.Filters
{
    public class ThresholdFilter : IThresholdFilter
    {
        public Bitmap Process(Bitmap image)
        {
            return Process(image, 80);
        }

        public unsafe Bitmap Process(Bitmap image, int param)
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
            return image;
        }
    }
}
