using AppsGenerator.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;

namespace AppsGenerator.Classes.Utilities
{
    public class Globals
    {
        public static string APP_DATA_PATH = HttpContext.Current.Server.MapPath("~/App_Data");
        public static string GetSQLServerConnectionString(Application application)
        {
            String user_id = application.db_user_id, password = application.db_password;

            var scsb = new SqlConnectionStringBuilder();

            scsb.DataSource = application.db_datasource;

            scsb.InitialCatalog = application.db_name;
            scsb.MultipleActiveResultSets = true;
            if (user_id != null && password != null)
            {
                scsb.IntegratedSecurity = false;    
                scsb.UserID = user_id;
                scsb.Password = password;
            }
            else
            {
                scsb.IntegratedSecurity = true;
            }
            return scsb.ToString();
        }

        public static bool IsReservedUsername(string username)
        {
            string usernamesPath = APP_DATA_PATH + "\\usernames.txt";
            String body = File.ReadAllText(usernamesPath);

            bool isMatch = Regex.IsMatch(body, "\\b" + username + "\\b");
            if (isMatch)
                return true;
            return false;
        }
    }
}