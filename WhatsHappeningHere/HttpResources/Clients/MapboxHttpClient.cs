using System;
using System.Net.Http;

namespace WhatsHappeningHere.HttpResources.Clients
{
    public class MapboxHttpClient : IDisposable
    {
        public static HttpClient Client { get; } = new HttpClient();
        public const string mapboxUsername = "acatalfano";

        static MapboxHttpClient()
        {
            Client.BaseAddress = new Uri(@"https://api.mapbox.com/");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() => Dispose(true);
        #endregion
    }
}
