using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Library.SocketCommunication.DataStruct
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
