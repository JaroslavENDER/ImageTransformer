using Kontur.ImageTransformer.Transformer;
using System;
using System.Drawing.Imaging;
using System.Net;

namespace Kontur.ImageTransformer.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new AsyncHttpServer();
            server.AddHandler(server_Handler);
            server.StartAsync("http://localhost:8080/");

            Console.ReadLine();
        }

        private static void server_Handler(HttpListenerContext context)
        {
            Console.WriteLine("Request: " + context.Request.QueryString);
            var inputStream = context.Request.InputStream;
            var outputStream = context.Response.OutputStream;

            var result = HandleImage(ImgConverter.ConvertFromStream(inputStream), null);

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            ImgConverter.ConvertToBitmap(result).Save(outputStream, ImageFormat.Png);
            outputStream.Close();
            Console.WriteLine("Response: " + context.Request.QueryString);
        }

        private static Img HandleImage(Img image, string queryString)
        {
            return Filter.SetThreshold(image, 50);
        }
    }
}
