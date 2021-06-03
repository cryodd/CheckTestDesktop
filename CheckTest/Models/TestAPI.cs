using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckTest.ViewModels
{
    class TestAPI
    {
        [JsonProperty("id_test")]
        public int IdTest { get; set; }
        [JsonProperty("test_input")]
        public byte[] TestInput { get; set; }
        [JsonProperty("test_output")]
        public byte[] TestOutput { get; set; }
        [JsonProperty("describe_task")]
        public string DescribeTask { get; set; }
    }
}
