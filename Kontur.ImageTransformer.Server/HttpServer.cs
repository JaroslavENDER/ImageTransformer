using System;
using System.Collections.Generic;
using System.Net;

namespace Kontur.ImageTransformer.Server
{
    internal class HttpServer : IDisposable
    {
        private Dictionary<string, HttpListener> listeners;
        private Action<HttpListenerContext> handlers;
        private bool isDisposed = false;

        public HttpServer()
        {
            listeners = new Dictionary<string, HttpListener>();
        }

        public async void StartAsync(string prefix)
        {
            if (isDisposed)
                throw new ObjectDisposedException("HttpServer", "This object has been closed");

            if (listeners.ContainsKey(prefix))
                return;

            var listener = new HttpListener();
            listeners.Add(prefix, listener);
            listener.Prefixes.Clear();
            listener.Prefixes.Add(prefix);
            listener.Start();

            while (true)
            {
                if (listener.IsListening && handlers != null)
                    handlers.BeginInvoke(await listener.GetContextAsync(), null, null);
            }
        }

        public void Stop(string prefix)
        {
            if (isDisposed)
                throw new ObjectDisposedException("HttpServer", "This object has been closed");

            var listener = listeners[prefix];
            if (listener == null)
                return;
            (listener as IDisposable).Dispose();
            listeners.Remove(prefix);
        }

        public void Stop()
        {
            if (isDisposed)
                throw new ObjectDisposedException("HttpServer", "This object has been closed");

            foreach (var kvp in listeners)
                (kvp.Value as IDisposable).Dispose();
            listeners.Clear();
        }

        public void RegisterHandler(Action<HttpListenerContext> handler)
        {
            if (handlers != null)
                throw new InvalidOperationException("You can register only one handler. You must remove current handler to register another");

            handlers += handler;
        }

        public void RemoveHandler()
        {
            handlers = null;
        }

        public void Dispose()
        {
            Stop();
            isDisposed = true;
        }
    }
}
