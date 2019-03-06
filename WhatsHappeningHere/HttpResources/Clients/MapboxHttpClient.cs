using System;
using System.Net.Http;

namespace WhatsHappeningHere.HttpResources.Clients
{
    public class MapboxHttpClient
    {
        public static HttpClient Client { get; } = new HttpClient()
        {
            BaseAddress = new Uri(@"https://api.mapbox.com/")
        };

        public const string mapboxUsername = "acatalfano";
        
    }
}
