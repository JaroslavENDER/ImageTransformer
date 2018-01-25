using System.Drawing;

namespace Kontur.ImageTransformer.Transformer
{
    public class Img
    {
        private Color[,] pixels;
        public Size Size { get; private set; }

        public Img(int width, int height)
        {
            Size = new Size(width, height);
            pixels = new Color[width, height];
        }

        public Color this[int x, int y]
        {
            get =>
                pixels[x, y];
            set =>
                pixels[x, y] = value;
        }
    }
}
