using System;
using System.Net.Http;

namespace WhatsHappeningHere.HttpResources.Clients
{
    public class HereHttpClient : IDisposable
    {
        public static HttpClient PlacesClient { get; } = new HttpClient();
        public static HttpClient TrafficClient { get; } = new HttpClient();

        private const string appID = "EVeUmiv6qrPEaoKK9zNI";
        private const string appCode = "Elq5ypOXQKI9VMQUOUE01w";

        static HereHttpClient()
        {
            PlacesClient.BaseAddress = new Uri(@"https://places.cit.api.here.com/");
            TrafficClient.BaseAddress = new Uri(@"https://traffic.api.here.com/");
        }

        public static string CredentialsUrlSegment() => $"&app_id={appID}&app_code={appCode}";

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PlacesClient.Dispose();
                    TrafficClient.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() => Dispose(true);
        #endregion
    }
}
