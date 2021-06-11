using CheckTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace CheckTest.ViewModels
{
    class TestTask
    {
        string dir = "comp/test"; //Рабочая директория
        int id_task; //Номер задания
        string proc_path, input_path, output_path, line;
        List<string> lineMass = new List<string>();
        Details det = new Details();
        public TestTask(int id, string path)
        {
            id_task = id; //Номер задачи
            proc_path = path; //Путь исполняемого файл
            input_path = dir + "/input"; //Входной файл
            output_path = dir + "/output"; //Выходной файл

        }
        //Проверка
        public Details Check(byte[] EtalonByte)
        {
            //Процесс
            using (Process proc = new Process())
            {
                try
                {
                    proc.StartInfo.FileName = proc_path;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.Start(); //Запуск процесса
                    //Запись исходных данных во входной файл
                    using (StreamReader streamReader = new StreamReader(input_path))
                    {
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            proc.StandardInput.WriteLine(line);
                        }
                    }
                    proc.StandardInput.Close();//Закрывает ввод, на случай если количество строк ввода пользователя не совпадает с тестовым
                    if (!proc.WaitForExit(300))
                    {
                       
                        proc.Dispose();
                        lineMass = new List<string>();
                        lineMass.Add("Ошибка в программе");
                        return det.Detail(0, lineMass);
                    }
                    File.Delete(output_path); //Удаление файла с выходными данными
                    lineMass.Clear(); //Удаление массива с выходными данными
                    //Запись выходных данных в файл
                    using (StreamWriter streamWriter = new StreamWriter(output_path))
                    {
                        var a = proc.StandardOutput.EndOfStream; //Проверка, есть ли выходные данные
                        if (!a)
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
                        }
                        else
                        {
                            proc.StandardOutput.Close();
                            proc.WaitForExit();
                            lineMass = new List<string>();
                            lineMass.Add("Ошибка в программе");
                            return det.Detail(0, lineMass);
                        }
                    }
                    proc.StandardOutput.Close();
                    proc.StandardInput.Close();//Закрывает ввод, на случай если количество строк ввода пользователя не совпадает с тестовым
                    proc.Dispose(); //Ожидание завершения файла
                    
                    //Проверка совпадения эталона и выходных данных
                    if (FileEquals(output_path, EtalonByte))
                    {
                        return det.Detail(1, lineMass);
                    }
                    else
                    {
                        return det.Detail(0, lineMass);
                    }

                }
                catch (Exception e)
                {
                    proc.Dispose();
                    List<string> vs = new List<string>();
                    vs.Add(e.Message);//Вывод ошибки
                    return det.Detail(0, vs);

                }
            }
        }

        //Проверка эквивалентности файла и массива из байтов
        static bool FileEquals(string path, byte[] bytes)
        {
            byte[] file1 = File.ReadAllBytes(path);
            if (file1.Length == bytes.Length)
            {
                for (int i = 0; i < file1.Length; i++)
                {
                    if (file1[i] != bytes[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        //Преобразование из string в byte[]
        public static byte[] StringToByte(string StringByte)
        {
            string[] vs = StringByte.Split('*');
            byte[] result = new byte[Convert.ToInt32(vs[vs.Length - 1])];

            //Преобразование
            for (int i = 0; i < vs.Length - 1; i++)
            {

                result[i] = Convert.ToByte(vs[i]);
            };
            return result;
        }
    }

}
