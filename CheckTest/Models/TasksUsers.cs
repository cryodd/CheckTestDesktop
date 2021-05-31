using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace TestCheck.Models
{
    public class TasksUsers
    {
            [JsonProperty("email")]
            public string Email { get; set; }
            [JsonProperty("password")]
            public string Password { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("access")]
            public long Access { get; set; }
    }
}
