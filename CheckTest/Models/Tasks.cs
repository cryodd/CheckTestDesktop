using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TestCheck.Models
{
    public class Tasks
    {
            [JsonProperty("id_task")]
            public int IdTask { get; set; }
            [JsonProperty("name_task")]
            public string NameTask { get; set; }
            [JsonProperty("describe_task")]
            public string DescribeTask { get; set; }
    }
}
