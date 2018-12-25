using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AppsGenerator.Classes.Publish
{
    public class Compile
    {
        public static int CompileDbDLL(String fileName,String code, String outputPath)
        {
            var compiler = new CSharpCodeProvider();

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Data.Entity.dll");
            parameters.ReferencedAssemblies.Add("System.Data.Entity.Design.dll");
            parameters.ReferencedAssemblies.Add("System.Runtime.Serialization.dll");


            parameters.OutputAssembly = outputPath + fileName + ".dll";
            var result = compiler.CompileAssemblyFromSource(parameters, code);
            return result.Errors.Count;
        }

        public static Assembly CreateFromCSFiles(string pathName,string appName,string dbAssebmly)
        {
            CSharpCodeProvider csCompiler = new CSharpCodeProvider();

            CompilerParameters compilerParams = new CompilerParameters();
            //compilerParams.GenerateInMemory = true;

            // here you must add all the references you need. 
            // I don't know whether you know all of them, but you have to get them
            // someway, otherwise it can't work

            compilerParams.ReferencedAssemblies.Add("System.dll");
            //compilerParams.ReferencedAssemblies.Add("System.Collections.dll");
            compilerParams.ReferencedAssemblies.Add("System.Linq.dll");
            compilerParams.ReferencedAssemblies.Add("System.Web.dll");
            compilerParams.ReferencedAssemblies.Add("System.Data.dll");
            compilerParams.ReferencedAssemblies.Add("System.Data.Entity.dll");
            compilerParams.ReferencedAssemblies.Add("System.Data.Entity.Design.dll");
            compilerParams.ReferencedAssemblies.Add(typeof(System.Web.Mvc.ActionResult).Assembly.Location);
            compilerParams.ReferencedAssemblies.Add(typeof(System.Web.Optimization.Bundle).Assembly.Location);
            compilerParams.ReferencedAssemblies.Add(typeof(System.Data.Entity.DbContext).Assembly.Location);
            compilerParams.ReferencedAssemblies.Add(typeof(System.Linq.Enumerable).Assembly.Location);
            
            ArrayList files = filePaths(pathName);

            string[] csPaths = new string[files.Count];
            for (int i = 0; i < csPaths.Length; i++)
            {
                csPaths[i] = (string) files[i];
            }

            compilerParams.OutputAssembly = pathName + "\\bin\\" + appName + ".dll";
            CompilerResults result = csCompiler.CompileAssemblyFromFile(compilerParams, csPaths);
            if (result.Errors.HasErrors)
                return null;

            File.Delete(pathName + "\\Global.asax.cs");

            return result.CompiledAssembly;
        }

        private static ArrayList filePaths(string pathName)
        {
            ArrayList filesArraylist = new ArrayList();
            String[] dirs = new string[] {"Controllers","App_Start","Models" };

            for (int i = 0; i < dirs.Length; i++)
            {
               DirectoryInfo csDir = new DirectoryInfo(pathName+"\\"+dirs[i]);
               FileInfo[] files = csDir.GetFiles();
               for (int x = 0; x < files.Length; x++)
                   filesArraylist.Add(files[x].FullName);
            }
            filesArraylist.Add(pathName + "\\Global.asax.cs");
            return filesArraylist;
        }
    }
}