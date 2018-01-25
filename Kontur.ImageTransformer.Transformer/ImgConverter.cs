using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Kontur.ImageTransformer.Transformer
{
    public static class ImgConverter
    {
        public static Img ConvertFromStream(Stream stream)
        {
            var bitmap = Image.FromStream(stream) as Bitmap;
            return ConvertFromBitmap(bitmap);
        }

        public static Img ConvertFromBytes(byte[] bytes)
        {
            Bitmap bitmap = null;
            using (var stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                bitmap = Image.FromStream(stream) as Bitmap;
            }
            return ConvertFromBitmap(bitmap);
        }

        public static Img ConvertFromBitmap(Bitmap bitmap)
        {
            var image = new Img(bitmap.Width, bitmap.Height);
            for (var y = 0; y < bitmap.Height; y++)
                for (var x = 0; x < bitmap.Width; x++)
                    image[x, y] = bitmap.GetPixel(x, y);
            return image;
        }

        public static byte[] ConvertToBytes(Img image)
        {
            var bitmap = ConvertToBitmap(image);
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Dispose();
            return bytes;
        }

        public static Bitmap ConvertToBitmap(Img image)
        {
            var bitmap = new Bitmap(image.Size.Width, image.Size.Height);
            for (var y = 0; y < bitmap.Height; y++)
                for (var x = 0; x < bitmap.Width; x++)
                    bitmap.SetPixel(x, y, image[x, y]);
            return bitmap;
        }
    }
}
