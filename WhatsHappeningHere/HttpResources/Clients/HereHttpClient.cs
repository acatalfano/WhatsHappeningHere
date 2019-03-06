using System;
using System.Net.Http;

namespace WhatsHappeningHere.HttpResources.Clients
{
    public class HereHttpClient
    {
        public static HttpClient PlacesClient { get; } =
            new HttpClient()
            {
                BaseAddress = new Uri(@"https://places.cit.api.here.com/")
            };
        public static HttpClient TrafficClient { get; } =
            new HttpClient()
            {
                BaseAddress = new Uri(@"https://traffic.api.here.com/")
            };

        private const string appID = "EVeUmiv6qrPEaoKK9zNI";
        private static readonly string appCode = "Elq5ypOXQKI9VMQUOUE01w";
        

        public static string CredentialsUrlSegment() => $"&app_id={appID}&app_code={appCode}";
    }
}
