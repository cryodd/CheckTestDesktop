using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CheckTest.ViewModels
{
    class TestTask
    {
        string InputPath = "comp/a";
        string OutputPath = "comp/a.a";
        string line;
        List<string> lineMass = new List<string>();
        public TestTask(int id,string path)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = path;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
            using (StreamReader streamReader = new StreamReader(InputPath))
            {
                while((line = streamReader.ReadLine()) != null)
                {
                    proc.StandardInput.WriteLine(line);
                }
            }
            using (StreamWriter streamWriter = new StreamWriter(OutputPath))
            {
                while ((line = proc.StandardOutput.ReadLine())!=null)
                {
                    lineMass.Add(line);
                    
                    
                    
                }
                for(int i=0;i<lineMass.Count()-1;i++)
                {
                    streamWriter.WriteLine(lineMass[i]);
                }
                streamWriter.Write(lineMass[lineMass.Count()-1]);

            }
             

            //Console.WriteLine(proc.StandardOutput());
            proc.WaitForExit();
        }
    }
}
