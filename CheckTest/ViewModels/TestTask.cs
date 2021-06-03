using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace CheckTest.ViewModels
{
    class TestTask
    {
        string dir = "comp/test";
        int id_task;
        string proc_path,input_path,output_path,etalon,line;
        List<string> lineMass = new List<string>(); 
        Process proc = new Process();
        public TestTask(int id,string path)
        {
            id_task = id;
            proc_path = path;
            input_path = dir + "/input";
            output_path = dir +"/output";
            etalon = dir + "/etanol";
            
        }
        public int Check()
        { 
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                proc.StartInfo.FileName = proc_path;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                using (StreamReader streamReader = new StreamReader(input_path))
                {
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        proc.StandardInput.WriteLine(line);
                    }
                }
                using (StreamWriter streamWriter = new StreamWriter(output_path))
                {
                    while ((line = proc.StandardOutput.ReadLine()) != null)
                    {
                        lineMass.Add(line);

                    }
                    for (int i = 0; i < lineMass.Count() - 1; i++)
                    {
                        streamWriter.WriteLine(lineMass[i]);
                    }
                    streamWriter.Write(lineMass[lineMass.Count() - 1]);
                    proc.WaitForExit();
                    // Directory.Delete(dir,true)    
                    if (FileEquals(etalon, output_path))
                    {
                        return 1;

                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch(Exception e)
            {
                proc.Kill();
                MessageBox.Show(e.Message);
                return 2;
            }
            
        }
        static bool FileEquals(string path1, string path2)
        {
            byte[] file1 = File.ReadAllBytes(path1);
            byte[] file2 = File.ReadAllBytes(path2);
            if (file1.Length == file2.Length)
            {
                for (int i = 0; i < file1.Length; i++)
                {
                    if (file1[i] != file2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }

}
