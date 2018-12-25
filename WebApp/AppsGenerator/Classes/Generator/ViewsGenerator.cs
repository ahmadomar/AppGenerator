using AppsGenerator.Classes.Generator;
using AppsGenerator.Classes.Mains;
using AppsGenerator.CSharpGenerator.Web.MvcView;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Web;

namespace AppsGenerator.Classes.Generator
{
    public class ViewsGenerator : ApplicationMain
    {
        public Version MvcVersion { get; set; }

        public ViewsGenerator(string ApplicationName, string AppPath)
            : base(ApplicationName,AppPath)
        {
           MvcVersion = new Version("5.1.0.0");
        }

        public void Generate(ModelMetadata modelMetadata, List<ViewType> viewTypes, EntityType table)
        {
            string ViewDataTypeName = ApplicationName + ".Models." + table.Name;
            string ViewDataTypeShortName = table.Name;

            foreach (var viewType in viewTypes)
            {

                switch (viewType)
                {
                    case ViewType.Create:
                        GenerateCreate(modelMetadata, ViewDataTypeName, ViewDataTypeShortName);
                        break;
                    case ViewType.Update:
                        GenerateEdit(modelMetadata, ViewDataTypeName, ViewDataTypeShortName);
                        break;
                    case ViewType.List:
                        GenerateList(modelMetadata, ViewDataTypeName, ViewDataTypeShortName);
                        break;
                    case ViewType.Details:
                        GenerateDetails(modelMetadata, ViewDataTypeName, ViewDataTypeShortName);
                        break;
                    case ViewType.Delete:
                        GenerateDelete(modelMetadata, ViewDataTypeName, ViewDataTypeShortName);
                        break;
                }
            }
        }


        #region Operations
        private void GenerateCreate(ModelMetadata metadata, string ViewDataTypeName, string ViewDataTypeShortName)
        {
            Create create = new Create();
            create.ViewDataTypeName = ViewDataTypeName;
            create.ViewDataTypeShortName = ViewDataTypeShortName;
            create.IsPartialView = false;
            create.IsLayoutPageSelected = true;
            create.ReferenceScriptLibraries = false;
            create.IsBundleConfigPresent = true;
            create.ViewName = "Create";
            create.LayoutPageFile = "";
            create.JQueryVersion = "1.10.2";
            create.ModelMetadata = metadata;
            create.MvcVersion = MvcVersion;

            String cc = create.TransformText();

            File.WriteAllText(AppPath + "Views\\" + ViewDataTypeShortName + "\\Create.cshtml", cc);

        }


        private void GenerateEdit(ModelMetadata metadata, string ViewDataTypeName, string ViewDataTypeShortName)
        {
            Edit edit = new Edit();
            edit.ViewDataTypeName = ViewDataTypeName;
            edit.ViewDataTypeShortName = ViewDataTypeShortName;
            edit.IsPartialView = false;
            edit.IsLayoutPageSelected = true;
            edit.ReferenceScriptLibraries = false;
            edit.IsBundleConfigPresent = true;
            edit.ViewName = "Edit";
            edit.LayoutPageFile = "";
            edit.JQueryVersion = "1.10.2";
            edit.ModelMetadata = metadata;
            edit.MvcVersion = MvcVersion;

            String cc = edit.TransformText();

            File.WriteAllText(AppPath + "Views\\" + ViewDataTypeShortName + "\\Edit.cshtml", cc);

        }
        private void GenerateList(ModelMetadata metadata, string ViewDataTypeName, string ViewDataTypeShortName)
        {
            List list = new List();
            list.ViewDataTypeName = ViewDataTypeName;
            list.ViewDataTypeShortName = ViewDataTypeShortName;
            list.IsPartialView = false;
            list.IsLayoutPageSelected = true;
            list.ReferenceScriptLibraries = false;
            list.IsBundleConfigPresent = true;
            list.ViewName = "Index";
            list.LayoutPageFile = "";
            list.JQueryVersion = "1.10.2";
            list.ModelMetadata = metadata;
            list.MvcVersion = MvcVersion;

            String cc = list.TransformText();

            File.WriteAllText(AppPath + "Views\\" + ViewDataTypeShortName + "\\Index.cshtml", cc);

        }


        private void GenerateDetails(ModelMetadata metadata, string ViewDataTypeName, string ViewDataTypeShortName)
        {
            Details details = new Details();
            details.ViewDataTypeName = ViewDataTypeName;
            details.ViewDataTypeShortName = ViewDataTypeShortName;
            details.IsPartialView = false;
            details.IsLayoutPageSelected = true;
            details.ReferenceScriptLibraries = false;
            details.IsBundleConfigPresent = true;
            details.ViewName = "Details";
            details.LayoutPageFile = "";
            details.JQueryVersion = "1.10.2";
            details.ModelMetadata = metadata;
            details.MvcVersion = MvcVersion;

            String cc = details.TransformText();

            File.WriteAllText(AppPath + "Views\\" + ViewDataTypeShortName + "\\Details.cshtml", cc);

        }

        private void GenerateDelete(ModelMetadata metadata, string ViewDataTypeName, string ViewDataTypeShortName)
        {
            Delete delete = new Delete();
            delete.ViewDataTypeName = ViewDataTypeName;
            delete.ViewDataTypeShortName = ViewDataTypeShortName;
            delete.IsPartialView = false;
            delete.IsLayoutPageSelected = true;
            delete.ReferenceScriptLibraries = false;
            delete.IsBundleConfigPresent = true;
            delete.ViewName = "Delete";
            delete.LayoutPageFile = "";
            delete.JQueryVersion = "1.10.2";
            delete.ModelMetadata = metadata;
            delete.MvcVersion = MvcVersion;

            String cc = delete.TransformText();

            File.WriteAllText(AppPath + "Views\\" + ViewDataTypeShortName + "\\Delete.cshtml", cc);

        }
        #endregion
    }
}