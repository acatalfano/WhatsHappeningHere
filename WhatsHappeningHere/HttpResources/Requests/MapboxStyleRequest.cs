using System.Net;
using System.Net.Http;
using WhatsHappeningHere.HttpResources.Clients;

namespace WhatsHappeningHere.HttpResources.Requests
{
    public class MapboxStyleRequest
    {
        private static string Endpoint(string styleID) =>
            $"styles/v1/{MapboxHttpClient.mapboxUsername}/{styleID}";

        public string MakeRequest(string styleID, string accessToken)
        {
            string requestUrl = MapboxHttpClient.Client.BaseAddress.ToString() +
                Endpoint(styleID) + $"?access_token={accessToken}";

            HttpResponseMessage response = MapboxHttpClient.Client.GetAsync(requestUrl).GetAwaiter().GetResult();
            HttpStatusCode status = response.StatusCode;

            if (status == HttpStatusCode.OK)
            {
                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            else
            {
                throw new HttpRequestException(
                    $"Error in Mapbox Style Request with status code = {status} and response content = \n"
                    + response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            }
        }
    }
}
