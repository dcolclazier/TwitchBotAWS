using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;

namespace AWS.TwitchBot.AWS.Core.Lambda
{
    public class DockerMessage
    {
        [JsonProperty("Trace")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TraceLevel Trace { get; set; }
        public string Message { get; set; }

    }
}
