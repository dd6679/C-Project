using Newtonsoft.Json;

namespace VMS.Client2.Net
{
    public class Result<T>
    {
        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("param")]
        public T Param { get; set; }

    }
}
