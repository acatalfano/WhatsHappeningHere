using System;
using System.Net.Http;

namespace WhatsHappeningHere.HttpResources
{
    public static class MapboxHttpClient
    {
        public static HttpClient Client { get; } = new HttpClient();
        public const string mapboxUsername = "acatalfano";

        internal const string mapboxUploadToken = "sk.eyJ1IjoiYWNhdGFsZmFubyIsImEiOiJjanJsOW9kOGkwNjl4NGFtdWk4NnJ3cWRzIn0.6SXLfpeKrhngENAUKngsYg";

        static MapboxHttpClient()
        {
            Client.BaseAddress = new Uri(@"https://api.mapbox.com/");
        }
    }
}
