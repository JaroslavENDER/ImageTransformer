using System;

namespace Kontur.ImageTransformer.Transformer
{
    [Obsolete("Use Fiter and SnippingTool", error: false)]
    public static class Transformer
    {
        public static Img Transform(Img image, FilterType type, int x, int y, int width, int height, int param = 0)
        {
            switch (type)
            {
                case FilterType.Grayscale:
                    return Filter.SetGrayscale(SnippingTool.Cut(image, x, y, width, height));
                case FilterType.Threshold:
                    return Filter.SetThreshold(SnippingTool.Cut(image, x, y, width, height), param);
                case FilterType.Sepia:
                    return Filter.SetSepia(SnippingTool.Cut(image, x, y, width, height));
                default:
                    return image;
            }
        }
    }
}
