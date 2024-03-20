using Newtonsoft.Json;

namespace JPC.Application.Shared.Rainfall.Dto
{
    public class RainfallReading
    {
        public string DateMeasured { get; set; }
        public double AmountMeasured { get; set; }
    }

    public class RainfallStation
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("latestReading")]
        public LatestReading LatestReading { get; set; }
    }

    public class LatestReading
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("dateTime")]
        public string DateTime { get; set; }

        [JsonProperty("measure")]
        public string Measure { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }

}
