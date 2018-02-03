using Kontur.ImageTransformer.Transformer;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace Kontur.ImageTransformer.Server
{
    class Program
    {
        private static int counter = 0;

        static void Main(string[] args)
        {
            var server = new AsyncHttpServer();
            server.AddHandler(server_Handler);
            server.StartAsync("http://localhost:8080/");

            Console.ReadLine();
        }

        private static void server_Handler(HttpListenerContext context)
        {
            var inputStream = context.Request.InputStream;
            var outputStream = context.Response.OutputStream;

            var result = HandleImage(Image.FromStream(inputStream) as Bitmap, null);

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            result.Save(outputStream, ImageFormat.Png);
            outputStream.Close();
            Console.WriteLine("Response: " + counter++);
        }

        private static Bitmap HandleImage(Bitmap image, string queryString)
        {
            Filter.SetGrayscale(image);
            return image;
        }
    }
}
