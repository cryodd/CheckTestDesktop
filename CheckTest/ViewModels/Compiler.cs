using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;

namespace CheckTest.ViewModels
{
    class Compiler
    {
        const string path = "comp/Compiler.exe";
        public CompilerErrorCollection Compile(string text2,int IdTask)
        {
            
            string text = @"using System;
namespace HelloWorld
    {
        /// <summary>
        /// Summary description for Class1.
        /// </summary>
        class HelloWorldClass
        {
            static void Main(string[] args)
            {
int a;
for(int i=1;i<6;i++){
a = Convert.ToInt32(Console.ReadLine());
a*=a;
Console.WriteLine(a);

};
                
                

            }
        }
    }"; 
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
            
            TestTask tests = new TestTask(IdTask, path);
            Console.WriteLine(tests.Check());
            if (tests.Check() != 2)
            {

            }
            return result.Errors;
        }
    }
}
