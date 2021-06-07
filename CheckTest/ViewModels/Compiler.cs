using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Windows;

namespace CheckTest.ViewModels
{
    class Compiler
    {
        const string path = "comp/Compiler.exe"; //Путь для испоняемого файла компилятора

        public CompilerErrorCollection Compile(string text,int IdTask)
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
            var result = codeProvider.CompileAssemblyFromSource(cp, text);
            if (result.Errors.HasErrors)
            {
                return result.Errors;
            }
            TestTask tests = new TestTask(IdTask, path);
            IEnumerable<Tests> TestResult = TestTaskAPI.GetTestByIdTask(IdTask).Where(x => x.id_test == 12);
            List<int> work = new List<int>(TestResult.Count());

            //Проверка всех тестов
            foreach (var item in TestResult)
            {
                File.WriteAllBytes("comp/test/input", TestTask.StringToByte(item.test_input));
                int res = tests.Check(TestTask.StringToByte(item.test_output));
                if (res != 2)
                {
                    work.Add(res);
                }
                else
                {
                    MessageBox.Show("Произошла ошибка");
                    return result.Errors;
                }
            }

            return result.Errors;


        }
    }
}
