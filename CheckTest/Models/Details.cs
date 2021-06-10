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
            string Text1 = "";
            for (int i = 0; i < u.Count() - 1; i++)
            {
                Text1 += u[i];
                Text1 += "/n";
            }
            Text1 += u[ u.Count() - 1];
            byte[] bt = Encoding.UTF8.GetBytes(Text1);
            foreach (var item in bt)
            {
                Text += item;
                Text += '*';
            }
            Text += bt.Length;
            var det = new Details();
            det.sucsess = s;
            det.user_output = Text;
            return det;
        }
    }
}
