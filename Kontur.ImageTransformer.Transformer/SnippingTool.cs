using System;
using System.Drawing;

namespace Kontur.ImageTransformer.Transformer
{
    public class SnippingTool
    {
        [Obsolete("Use new SnippingTool().Snip()", error: false)]
        public static Bitmap Cut(Bitmap image, Rectangle area)
        {
            area = NormalizeCoords(image.Size, area);
            return image.Clone(area, image.PixelFormat);
        }

        public Bitmap Snip(Bitmap image, Rectangle area)
        {
            area = NormalizeCoords(image.Size, area);
            return image.Clone(area, image.PixelFormat);
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
