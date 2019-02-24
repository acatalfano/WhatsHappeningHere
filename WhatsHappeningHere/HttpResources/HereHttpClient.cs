using System;
using System.Net.Http;

namespace WhatsHappeningHere.HttpResources
{
    public static class HereHttpClient
    {
        public static HttpClient PlacesClient { get; } = new HttpClient();
        public static HttpClient TrafficClient { get; } = new HttpClient();

        internal const string appID = "EVeUmiv6qrPEaoKK9zNI";
        internal const string appCode = "Elq5ypOXQKI9VMQUOUE01w";

        static HereHttpClient()
        {
            PlacesClient.BaseAddress = new Uri(@"https://places.cit.api.here.com/");
            TrafficClient.BaseAddress = new Uri(@"https://traffic.api.here.com/");
        }
    }
}
