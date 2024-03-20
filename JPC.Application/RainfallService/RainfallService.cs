using JPC.Application.Shared.Rainfall.Configuration;
using JPC.Application.Shared.Rainfall.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JPC.Application.RainfallService
{
    public class RainfallService : IRainfallService
    {
        private readonly HttpClient _httpClient;
        private readonly RainfallApiConfiguration _rainfallApiConfiguration;

        public RainfallService(HttpClient httpClient,
            RainfallApiConfiguration rainfallApiConfiguration)
        {
            _httpClient = httpClient;
            _rainfallApiConfiguration = rainfallApiConfiguration;
        }

        public async Task<RainfallReadingResponse> GetRainfallReadingsAsync(string stationId, int count, CancellationToken cancellationToken)
        {
            // Construct the URL for the API endpoint
            var url = $"{_rainfallApiConfiguration.GetByStationIdUrl}{stationId}";

            // Make the HTTP request to the API
            var response = await _httpClient.GetAsync(url, cancellationToken);

            // Ensure the request was successful
            if (!response.IsSuccessStatusCode)
            {
                // Handle error responses here, such as 404 or 500
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new RainfallApiException("Invalid request", null, System.Net.HttpStatusCode.BadRequest);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new RainfallApiException("No readings found for the specified stationId", null, System.Net.HttpStatusCode.NotFound);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    throw new RainfallApiException("Internal server error", null, System.Net.HttpStatusCode.InternalServerError);
                }
            }

            // Parse the response body to the RainfallReadingResponse model
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            JObject resultJsonObject = JsonConvert.DeserializeObject<JObject>(content.ToString());
            JToken itemNode = resultJsonObject.SelectToken("items");
            string measuresNodeString = itemNode.SelectToken("measures").ToString();

            // Deserialize from our data contract
            var readings = JsonConvert.DeserializeObject<List<RainfallStation>>(measuresNodeString);

            // Apply the limit to the number of readings
            List<RainfallReading> rainfallReading = readings.Take(count).Select(a => new RainfallReading()
            {
                DateMeasured = a.LatestReading.DateTime,
                AmountMeasured = a.LatestReading.Value
            }).ToList();

            // Return the readings wrapped in a RainfallReadingResponse object
            return new RainfallReadingResponse { Readings = rainfallReading };
        }
    }
}
