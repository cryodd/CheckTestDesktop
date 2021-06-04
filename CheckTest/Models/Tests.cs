using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckTest.ViewModels
{
    class Tests
    {
        public int id_test { get; set; }
        public Test_Input test_input { get; set; }
        public Test_Output test_output { get; set; }
        public int id_task { get; set; }
        public class Test_Input
        {
            public string type { get; set; }
            public int[] data { get; set; }
        }
        public class Test_Output
        {
            public string type { get; set; }
            public int[] data { get; set; }
        }
    }
}
