using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsGenerator.Classes.Mains
{
    public class ApplicationMain
    {
        public string ApplicationName { get; set; }
        public string AppPath { get; set; }

        public string ConnectionString { get; set; }

        public ApplicationMain(string ConnectionString,string ApplicationName,string AppPath = null)
        {
            this.ConnectionString = ConnectionString;
            this.ApplicationName = ApplicationName.Replace(" ", "");
            this.AppPath = AppPath;
        }

        public ApplicationMain(string ApplicationName,string AppPath)
        {
            this.ApplicationName = ApplicationName.Replace(" ", "");
            this.AppPath = AppPath;
        }
    }
}