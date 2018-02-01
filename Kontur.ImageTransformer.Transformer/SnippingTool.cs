using System.Drawing;

namespace Kontur.ImageTransformer.Transformer
{
    public static class SnippingTool
    {
        public static Img Cut(Img image, int x, int y, int width, int height)
        {
            NormalizeCoords(image.Size, ref x, ref y, ref width, ref height);
            var newImage = new Img(width, height);
            for (var X = 0; X < width; x++, X++)
                for (int Y = 0, tempY = y; Y < height; tempY++, Y++)
                    newImage[X, Y] = image[x, tempY];

            return newImage;
        }

        private static void NormalizeCoords(Size imageSize, ref int x, ref int y, ref int width, ref int height)
        {
            if (width < 0)
            {
                x  += width + 1;
                width *= -1;
            }
            if (height < 0)
            {
                y += height + 1;
                height *= -1;
            }
            if (x < 0)
            {
                width += x;
                x = 0;
            }
            if (y < 0)
            {
                height += y;
                y = 0;
            }
            if (x + width > imageSize.Width)
            {
                width = imageSize.Width - x;
            }
            if (y + height > imageSize.Height)
            {
                height = imageSize.Height - y;
            }
        }
    }
}
