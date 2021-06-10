using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Windows;
using CheckTest.Models;

namespace CheckTest.ViewModels
{
    class Compiler
    {
        const string path = "comp/Compiler.exe"; //Путь для испоняемого файла компилятора
        //Компиляция
        public CompilerErrorCollection Compile(string text)
        {
            //Удаление файла, если он уже существует
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            var cp = new CompilerParameters
            {
                GenerateExecutable = true,
                OutputAssembly = path
            };
            var result = codeProvider.CompileAssemblyFromSource(cp, text);//Создание исполняемого файла компилятора
            if (result.Errors.HasErrors)
            {
                return result.Errors;
            }
            return result.Errors;
        }
        //Тестирование
        public List<Details> Test(int IdTask)
        {
            try
            {


                TestTask tests = new TestTask(IdTask, path);
                IEnumerable<Tests> TestResult = TestTaskAPI.GetTestByIdTask(IdTask).Where(x => x.id_task == IdTask);//Все тесты, для конкретного задания
                List<Details> result = new List<Details>(); //Список всех результатов тестов(0- тест не пройден, 1- пройден)
                //Проверка всех тестов
                foreach (var item in TestResult)
                {
                    File.WriteAllBytes("comp/test/input", TestTask.StringToByte(item.test_input)); //Создание файла со входными данными
                    Details res = tests.Check(TestTask.StringToByte(item.test_output)); //Проверка теста с эталонным значением
                    res.id_test = item.id_test;
                    if (res.sucsess != 2)//Если нет ошибок
                    {
                        result.Add(res);
                    }
                    else//Если есть ошибки
                    {
                        MessageBox.Show("Произошла ошибка");
                        return null;
                    }
                    
                }
                return result;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
