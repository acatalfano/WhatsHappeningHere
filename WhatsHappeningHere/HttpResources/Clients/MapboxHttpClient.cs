using System;
using System.Net.Http;

namespace WhatsHappeningHere.HttpResources.Clients
{
    public static class MapboxHttpClient
    {
        public static HttpClient Client { get; } = new HttpClient();
        public const string mapboxUsername = "acatalfano";

        static MapboxHttpClient()
        {
            Client.BaseAddress = new Uri(@"https://api.mapbox.com/");
        }
    }
}
