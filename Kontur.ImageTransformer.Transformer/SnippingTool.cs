using System;
using System.Drawing;

namespace Kontur.ImageTransformer.Transformer
{
    public static class SnippingTool
    {
        public static Bitmap Cut(Bitmap image, Rectangle area)
        {
            area = NormalizeCoords(image.Size, area);
            return image.Clone(area, image.PixelFormat);
        }

        [Obsolete("Use Cut(Bitmap image, ...)", error: false)]
        public static Img Cut(Img image, int x, int y, int width, int height)
        {
            throw new NotImplementedException("Is Obsolete method");
            //NormalizeCoords(image.Size, ref x, ref y, ref width, ref height);
            var newImage = new Img(width, height);
            for (var X = 0; X < width; x++, X++)
                for (int Y = 0, tempY = y; Y < height; tempY++, Y++)
                    newImage[X, Y] = image[x, tempY];

            return newImage;
        }

        private static Rectangle NormalizeCoords(Size imageSize, Rectangle rect)
        {
            if (rect.Width < 0)
            {
                rect.X  += rect.Width + 1;
                rect.Width *= -1;
            }
            if (rect.Height < 0)
            {
                rect.Y += rect.Height + 1;
                rect.Height *= -1;
            }
            if (rect.X < 0)
            {
                rect.Width += rect.X;
                rect.X = 0;
            }
            if (rect.Y < 0)
            {
                rect.Height += rect.Y;
                rect.Y = 0;
            }
            if (rect.X + rect.Width > imageSize.Width)
            {
                rect.Width = imageSize.Width - rect.X;
            }
            if (rect.Y + rect.Height > imageSize.Height)
            {
                rect.Height = imageSize.Height - rect.Y;
            }

            return rect;
        }
    }
}
