﻿using Kontur.ImageTransformer.Transformer;
using System;
using System.IO;
using System.Net;

namespace Kontur.ImageTransformer.Server.StressTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SampleTest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static void SampleTest()
        {
            var bytes = File.ReadAllBytes("TestImage.png");
            var request = WebRequest.Create("http://localhost:8080/") as HttpWebRequest;
            request.Method = "POST";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            using (var responseStream = request.GetResponse().GetResponseStream())
            {
                var result = ImgConverter.ConvertFromStream(responseStream);
                ImgConverter.ConvertToBitmap(result).Save("ResultTestImage.png");
            }
        }
    }
}