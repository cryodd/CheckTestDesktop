using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace CheckTest.ViewModels
{
    class Compiler
    {
        public CompilerErrorCollection Compile(string text1)
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
                
            }
        }
    }";
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            var result = icc.CompileAssemblyFromSource(new CompilerParameters(), text);
            return result.Errors;
        }
    }
}
