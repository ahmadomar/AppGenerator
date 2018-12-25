using AppsGenerator.Classes.Mains;
using AppsGenerator.Classes.Utilities;
using AppsGenerator.CSharpGenerator.Web.MvcControllerWithContext;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Web;

namespace AppsGenerator.Classes.Generator
{

    /// <summary>
    /// Controller Generator, use this to Generate controller
    /// </summary>
    public class ControllerGenerator : ApplicationMain
    {
        private MyGenerator myGenerator { get; set; }
        
        public ControllerGenerator(MyGenerator myGenerator, string ApplicationName, String AppPath)
            :base(ApplicationName,AppPath)
        {
            this.myGenerator = myGenerator;
        }

        /// <summary>
        /// USe this function to Generate the controllers
        /// </summary>
        /// <param name="tablesType">Controllers Models</param>
        /// <param name="connectionString">Application Connection String</param>
        public void Generate(List<EntityType> tablesType, String connectionString)
        {
            ViewsGenerator viewsGenerator = new ViewsGenerator(ApplicationName, AppPath);

            foreach (var table in tablesType)
            {
                Microsoft.AspNet.Scaffolding.Core.Metadata.ModelMetadata metadata = myGenerator.GetModelMetadata(table);
                //Create Controller
                CreateController(table, metadata);

                viewsGenerator.Generate(metadata,ViewsType.ToList(), table);
            }
        }

        /// <summary>
        /// Use this to create specific controller
        /// </summary>
        /// <param name="tableType">EntityType</param>
        /// <param name="modelMetadata">Entity Type Metadata</param>
        private void CreateController(EntityType tableType, ModelMetadata modelMetadata)
        {
            Controller controller1 = new Controller();
            controller1.ControllerName = tableType.Name + "Controller";
            controller1.ControllerRootName = tableType.Name;
            controller1.Namespace = ApplicationName;
            controller1.AreaName = "";
            controller1.ContextTypeName = ApplicationName + "DbContext";
            controller1.ModelTypeName = tableType.Name;
            controller1.ModelVariable = "_" + tableType.Name;

            controller1.ModelMetadata = modelMetadata;
            controller1.EntitySetVariable = "_" + tableType.Name;
            controller1.UseAsync = false;
            controller1.IsOverpostingProtectionRequired = false;
            controller1.BindAttributeIncludeText = "";
            controller1.OverpostingWarningMessage = "";
            controller1.RequiredNamespaces = new HashSet<string>();

            File.WriteAllText(AppPath + "\\Controllers\\" + tableType.Name + "Controller.cs", controller1.TransformText());

            //Create controller view folder
            DirectoryUtility.CreateDirectory(AppPath, "Views\\" + tableType.Name);
        }

    }
}