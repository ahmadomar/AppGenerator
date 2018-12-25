using AppsGenerator.CSharpGenerator.Web.Database;
using AppsGenerator.CSharpGenerator.Web.ReverseEngineerCodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Web;

namespace AppsGenerator.Classes.Generator
{
    /// <summary>
    /// Use this class to generate the application DbContext file 
    /// Send the applicatoin name and the main application path to allow the apps generator to create the DbContext file
    /// </summary>
    public class DbContextGenerator
    {
        /// <summary>
        /// Use this function to Generate the DbContext
        /// </summary>
        /// <param name="models">The Application Entity types</param>
        /// <returns>True if Generated</returns>
        public static bool Generate(string ApplicationName, string AppPath, IEnumerable<EntityType> models)
        {
            try
            {
                ApplicationName = ApplicationName.Replace(" ", "");

                //Create DbContext
                AppDbContext appDbContext = new AppDbContext();
                appDbContext.DbContextNamespace = ApplicationName + ".Models";
                appDbContext.DbContextType = ApplicationName + "DbContext";
                appDbContext.ConnectionStringName = ApplicationName + "Entities";

                List<AppDbContext.EntityMember> entityMembers = new List<AppDbContext.EntityMember>();

                foreach (var item in models)
                {
                    entityMembers.Add(new AppDbContext.EntityMember()
                    {
                        EntityTypeName = item.Name,
                        EntityTypeNamePluralized = item.Name
                    });
                }

                appDbContext.EntityMembers = entityMembers;

                //Create DbContext file
                File.WriteAllText(AppPath + "\\Models\\" + ApplicationName + "Context.cs", appDbContext.TransformText());

                return true;
            }
            catch
            {
                return false;
            }
        }
}
}