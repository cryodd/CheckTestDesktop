using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckTest.Models
{
    class Details
    {
        public int sucsess { get; set; }
        public string user_output { get; set; }
        public int id_test { get; set; }
        public Details Detail(int s, List<string> u)
        {
            string Text = "";
            foreach (var item in u)
            {
                Text += item;
                Text += '*';
            }
            Text += u.Count;
            var det = new Details();
            det.sucsess = s;
            det.user_output = Text;
            return det;
        }
    }
}
