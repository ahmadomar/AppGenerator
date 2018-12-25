using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsGenerator.Classes.Generator
{
    public class ViewsContent
    {
        public string ViewDataTypeName { get; set; }
        public string ViewDataTypeShortName { get; set; }
        public bool IsPartialView { get; set; }
        public bool IsLayoutPageSelected { get; set; }
        public bool ReferenceScriptLibraries { get; set; }
        public bool IsBundleConfigPresent { get; set; }
        public string ViewName { get; set; }
        public string LayoutPageFile { get; set; }
        public string JQueryVersion { get; set; }
        public Version MvcVersion { get; set; }
        public Microsoft.AspNet.Scaffolding.Core.Metadata.ModelMetadata ModelMetadata { get; set; }
    }
}