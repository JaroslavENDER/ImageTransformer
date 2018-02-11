using System;
using System.Net;
using System.Threading;

namespace Kontur.ImageTransformer.Server
{
    internal class HttpServer
    {
        private readonly HttpListener listener;
        private Action<HttpListenerContext> handlers;

        public HttpServer()
        {
            listener = new HttpListener();
        }

        public void Start(string prefix)
        {
            listener.Prefixes.Clear();
            listener.Prefixes.Add(prefix);
            listener.Start();

            Listen();
        }

        public void AddHandler(Action<HttpListenerContext> handler)
        {
            handlers += handler;
        }

        private void Listen()
        {
            while (true)
            {
                if (listener.IsListening && handlers != null)
                    handlers.BeginInvoke(listener.GetContext(), null, null);
            }
        }
    }
}
