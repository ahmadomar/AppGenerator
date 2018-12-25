using AppsGenerator.Classes.Mains;
using AppsGenerator.CSharpGenerator.Web.App;
using AppsGenerator.CSharpGenerator.Web.AppStart;
using AppsGenerator.CSharpGenerator.Web.Configuration;
using AppsGenerator.CSharpGenerator.Web.Design;
using AppsGenerator.CSharpGenerator.Web.Me;
using AppsGenerator.CSharpGenerator.Web.MvcView;
using AppsGenerator.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AppsGenerator.Classes.Utilities
{

    /// <summary>
    /// Use this to create the application files and it's configuration
    /// </summary>
    public class ApplicationUtilities : ApplicationMain
    {
        public ApplicationUtilities(String ApplicationPath, String ApplicationName)
            : base(ApplicationName,ApplicationPath)
        {}

        /// <summary>
        /// Create main aplication files like View config, RouteConfig.cs, _Layout.cshtml and others
        /// </summary>
        public void CreateMainAppFiles(Application application)
        {
            //Create Web.Config in views folder
            CreateViewsWebConfig();

            //Create RouteConfig.cs and BundleConfig.cs
            CreateAppStartFiles(application.Theme.FileName);

            //Create _Layout.cshtml
            CreateAppLayout();

            //Create Applciation Content Helper
            CreateContentHelper();
        }




        public static void CreateWebConfig(string appPath,Application application, string ConnectionString)
        {
            WebConfig webConfig = new WebConfig();
            webConfig.DatabaseName = application.db_name;
            webConfig.AppName = application.Name.Replace(" ", "");
            webConfig.ConnectionString = ConnectionString;
            File.WriteAllText(appPath + "\\web.config", webConfig.TransformText());
        }

        public static void CreateGlobalAsax(Application application, string appPath)
        {
            GlobalAsaxCS globalAsax = new GlobalAsaxCS();
            String appName = application.Name.Replace(" ", "");
            globalAsax.ApplicationName = appName;
            File.WriteAllText(appPath + "\\Global.asax.cs", globalAsax.TransformText());

            GlobalAsax asax = new GlobalAsax();
            asax.Namespace = appName;
            File.WriteAllText(appPath + "\\Global.asax", asax.TransformText());
        }

        public void CreateViewsWebConfig()
        {
            ViewsWebConfig webConfig = new ViewsWebConfig();
            webConfig.Namespace = ApplicationName;
            File.WriteAllText(AppPath + "\\Views\\web.config", webConfig.TransformText());
        }

        public void CreateAppStartFiles(string AppTheme)
        {
            //Create Route File
            AppRouteConfig routeConfig = new AppRouteConfig();
            routeConfig.Namespace = ApplicationName;
            File.WriteAllText(AppPath + "\\App_Start\\RouteConfig.cs", routeConfig.TransformText());

            //Create Bundle File
            AppBundleConfig bundleConfig = new AppBundleConfig();
            bundleConfig.Namespace = ApplicationName;
            bundleConfig.AppTheme = AppTheme;
            File.WriteAllText(AppPath + "\\App_Start\\BundleConfig.cs", bundleConfig.TransformText());
        }

        public void CreateAppLayout()
        {
            AppLayout appLayout = new AppLayout();
            appLayout.ApplicationName = ApplicationName;
            File.WriteAllText(AppPath + "\\Views\\Shared\\_Layout.cshtml", appLayout.TransformText());
        }

        public void CreateAppNav(List<EntityType> tables)
        {
            AppNav appNav = new AppNav();
            appNav.tables = tables;
            File.WriteAllText(AppPath + "\\Views\\Shared\\_nav.cshtml", appNav.TransformText());
        }


        public void CreateProjectFile(string[] controllers,string[] models,string appTheme)
        {
            AppProj appProj = new AppProj();
            appProj.ApplicationName = ApplicationName;
            appProj.Controllers = controllers;
            appProj.AppTheme = appTheme;
            appProj.Models = models;
            File.WriteAllText(AppPath + "\\" + ApplicationName + ".csproj", appProj.TransformText());

        }

        public void CreateContentHelper()
        {
            ContentHelper contentHelper = new ContentHelper();
            contentHelper.NameSpace = ApplicationName;
            File.WriteAllText(AppPath + "\\ContentHelper.cs", contentHelper.TransformText());
        }

        public void CreateControllerHome(List<EntityType> tables)
        {
            HomeControllerTemplate controller = new HomeControllerTemplate();
            controller.Namespace = ApplicationName;
            File.WriteAllText(AppPath + "\\Controllers\\HomeController.cs", controller.TransformText());

            HomeIndexView homeIndexView = new HomeIndexView();
            homeIndexView.ApplicationName = ApplicationName;
            homeIndexView.Tables = tables;
            File.WriteAllText(AppPath + "\\Views\\Home\\Index.cshtml", homeIndexView.TransformText());

        }

        public void ZipApplication(Application application)
        {
            using (ZipFile zipFile = new ZipFile())
            {
                String TargetPath = Globals.APP_DATA_PATH + "\\";
                zipFile.AddDirectory(AppPath);
                TargetPath = TargetPath + application.Member.public_id;
                zipFile.Save(TargetPath + "\\" + application.Name + "_" + application.Id + ".zip");
            }

        }
    }
}