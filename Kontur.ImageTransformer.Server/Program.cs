using Ender.HttpServer;
using Kontur.ImageTransformer.Engine.Filters;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Threading;

namespace Kontur.ImageTransformer.Server
{
    internal class Program
    {
        private static int activeRequestsCount = 0;
        private static int maxActiveRequestsCount = Environment.ProcessorCount;
        private static int counter = 0;

        static void Main(string[] args)
        {
            using (var server = new HttpServer())
            {
                server.RegisterHandler(server_Handler);
                server.StartAsync("http://localhost:8080/");
                Console.ReadLine();
            }
        }

        private static void server_Handler(HttpListenerContext context)
        {
            if (activeRequestsCount < maxActiveRequestsCount)
                OkHanlder(context);
            else
                SkipHandler(context, (int)HttpStatusCode.Forbidden);

            Console.WriteLine(counter++ + "\tActive requests: " + activeRequestsCount);
        }

        private static void OkHanlder(HttpListenerContext context)
        {
            Interlocked.Increment(ref activeRequestsCount);
            var inputStream = context.Request.InputStream;
            var outputStream = context.Response.OutputStream;
            try
            {
                var result = HandleImage(Image.FromStream(inputStream) as Bitmap, context.Request.RawUrl);

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                result.Save(outputStream, ImageFormat.Png);
            }
            catch(Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                outputStream.Close();
                Interlocked.Decrement(ref activeRequestsCount);
            }
        }

        private static Bitmap HandleImage(Bitmap image, string rawUrl)
        {
            new GrayscaleFilter().Process(image);
            return image;
        }

        private static void SkipHandler(HttpListenerContext context, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.OutputStream.Close();
        }
    }
}
