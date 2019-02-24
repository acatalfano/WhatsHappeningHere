using System.Text;
using System.Net;
using System.Net.Http;

namespace WhatsHappeningHere.HttpResources.QueryStringObjects
{
    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    


    public class HereTrafficFlowRequest
    {
        private const string _endpoint = "traffic/6.2/flow.xml";

        public Coordinates NW_coords { get; }
        public Coordinates SE_coords { get; }

        
        public HereTrafficFlowRequest( double NW_lat, double NW_lng, double SE_lat, double SE_lng )
        {
            NW_coords = new Coordinates { Latitude = NW_lat, Longitude = NW_lng };
            SE_coords = new Coordinates { Latitude = SE_lat, Longitude = SE_lng };
        }


        public string MakeRequest()
        {
            HttpResponseMessage response = HereHttpClient.TrafficClient.GetAsync(RequestUrl()).GetAwaiter().GetResult();

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

        private string RequestUrl() => HereHttpClient.TrafficClient.BaseAddress.ToString() + _endpoint + QueryString();

        private string QueryString()
        {
            StringBuilder url = new StringBuilder("?responseattributes=shape&units=imperial");

            url.Append($"&bbox={NW_coords.Latitude},{NW_coords.Longitude};{SE_coords.Latitude},{SE_coords.Longitude}");

            url.Append($"app_id={HereHttpClient.appID}&app_code={HereHttpClient.appCode}");

            return url.ToString();
        }
    }
}
