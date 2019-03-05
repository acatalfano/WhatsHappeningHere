using System.Net;
using System.Net.Http;
using WhatsHappeningHere.HttpResources.HelperData;

namespace WhatsHappeningHere.HttpResources.Requests
{
    public class HereTrafficFlowRequest
    {
        private const string _endpoint = "traffic/6.2/flow.xml";

        public BoundingBox BBox { get; set; }
        
        public string MakeRequest()
        {
            HttpResponseMessage response = Clients.HereHttpClient.TrafficClient.GetAsync(RequestUrl()).GetAwaiter().GetResult();

            HttpStatusCode status = response.StatusCode;

            if (status == HttpStatusCode.OK)
            {
                return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            else if (status == HttpStatusCode.NoContent)
            {
                return "No Traffic Data Available";
            }
            else
            {
                throw new HttpRequestException(
                    $"Error in Traffic Flow Request with status code = {status} and response content = \n"
                    + response.Content.ReadAsStringAsync().GetAwaiter().GetResult()     );
            }
        }

        private string RequestUrl() => Clients.HereHttpClient.TrafficClient.BaseAddress.ToString() + _endpoint + QueryString();
        
        private string QueryString() =>
            "?responseattributes=shape" +
            "&units=imperial" +
            $"&bbox={BBox.NW.Latitude},{BBox.NW.Longitude};{BBox.SE.Latitude},{BBox.SE.Longitude}" +
            Clients.HereHttpClient.CredentialsUrlSegment();
    }
}
