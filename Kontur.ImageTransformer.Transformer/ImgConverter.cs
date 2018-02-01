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

        public static unsafe Img ConvertFromBitmap(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            var image = new Img(width, height);
            int A, R, G, B;
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bitmapData.Scan0) + h * bitmapData.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        B = *(curpos++);
                        G = *(curpos++);
                        R = *(curpos++);
                        A = *(curpos++);
                        image[w, h] = Color.FromArgb(A, R, G, B);
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
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

        public static unsafe Bitmap ConvertToBitmap(Img image)
        {
            var width = image.Size.Width;
            var height = image.Size.Height;
            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bd = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            try
            {
                byte* curpos;
                for (int h = 0; h < height; h++)
                {
                    curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                    for (int w = 0; w < width; w++)
                    {
                        *(curpos++) = image[w, h].B;
                        *(curpos++) = image[w, h].G;
                        *(curpos++) = image[w, h].R;
                        *(curpos++) = image[w, h].A;
                    }
                }
            }
            finally
            {
                result.UnlockBits(bd);
            }

            return result;
        }
    }
}
