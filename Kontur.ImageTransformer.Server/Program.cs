using Kontur.ImageTransformer.Transformer;
using System.Drawing.Imaging;

namespace Kontur.ImageTransformer.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Ender.HttpServer("http://localhost:8080/");
            server.ReceivedRequest += server_Handler;
            server.Start();
        }

        private static void server_Handler(object sender, System.Net.HttpListenerContext context)
        {
            var stream = context.Request.InputStream;
            var image = ImgConverter.ConvertFromStream(stream);
            image = Transformer.Transformer.Transform(image, FilterType.Grayscale, 0, 0, 2000, 2000);
            ImgConverter.ConvertToBitmap(image).Save(context.Response.OutputStream, ImageFormat.Png);
            context.Response.OutputStream.Close();
        }
    }
}
