using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TestCheck.Models
{
    public class ResponseAPI<T>
    {
        [JsonProperty("success")]
        public bool Success {get;set;}
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
