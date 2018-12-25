using AppsGenerator.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Design;
using System.Data.Metadata.Edm;
using AppsGenerator.CSharpGenerator.Web.MvcView;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using AppsGenerator.CSharpGenerator.Web.ReverseEngineerCodeFirst;
using AppsGenerator.Models;
using System.Net;
using System.Data;
using AppsGenerator.Models.Repositories;
using AppsGenerator.Classes.Utilities;
using AppsGenerator.Classes.Generator;
using AppsGenerator.Classes.Publish;
using PagedList;
using System.IO;
using System.Data.SqlClient;
using AppsGenerator.Classes.Layers.Controllers;
using AppsGenerator.Classes.Azure;
using System.Threading.Tasks;

namespace AppsGenerator.Controllers
{
    
    public partial class ApplicationController : Controller
    {
        [Authorize]
        [Route("GenerateApp")]
        public ActionResult Generate(int id = 0)
        {
            try
            {
                if (id == 0)
                    return HttpNotFound();

                Application application = applicationRepository
                            .FindFirst(ap => ap.Member.username == User.Identity.Name && ap.Id == id);

                if (application != null)
                {
                    //Get ConnectionString
                    string connectionString = Globals.GetSQLServerConnectionString(application);

                    List<AppTableView> appTables = GetApplicationTables(connectionString, application);

                    ViewBag.appId = id;
                    return View(appTables);
                }
            }
            catch (Exception ex)
            {
                return View("Error", new MessageView()
                {
                    Message = ex.Message
                });
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("GenerateApp")]
        public ActionResult Generate(int appId = 0, string[] selectedTables = null, string selectedTheme = "")
        {
            try
            {
                string userName = User.Identity.Name;
                Application application = applicationRepository.FindFirst(ap => ap.Member.username == userName && ap.Id == appId);

                if (application != null)
                {
                    bool connectionSuccess = CheckConnection(application);
                    if (connectionSuccess == true)
                    {
                        DeleteExistsFile(application);

                        //Change Application Theme
                        application.theme_id = GetTheme(selectedTheme);
                        applicationRepository.Edit(application);

                        //Create Application Folders
                        string appMainDirectory = CreateAppDirectories(application);
                        if (!string.IsNullOrEmpty(appMainDirectory))
                        {
                            //Generate Database files
                            string connectionString = Globals.GetSQLServerConnectionString(application);
                            string appName = application.Name;

                            var myGenerator = new MyGenerator(connectionString, appName, appMainDirectory);

                            //App Tables
                            var tablesType = myGenerator.DatabaseTables.Where(tb => selectedTables.Contains(tb.Name)).ToList();

                            //Create application models and mapping
                            myGenerator.GenerateCode();

                            CreateContextAndConfig(myGenerator.DatabaseTables.ToList(), application, appMainDirectory);

                            //Create application controllers
                            var controllerGenerator = new ControllerGenerator(myGenerator, appName, appMainDirectory);
                            controllerGenerator.Generate(tablesType, connectionString);

                            //Create application navigation menu
                            ApplicationUtilities utilities = new ApplicationUtilities(appMainDirectory, application.Name);
                            utilities.CreateAppNav(tablesType);

                            //Create Application project file
                            string[] controllers = tablesType.Select(tp => tp.Name).ToArray<string>();
                            string[] allModels = myGenerator.DatabaseTables.Select(tp => tp.Name).ToArray<string>();
                            utilities.CreateProjectFile(controllers, allModels, application.Theme.FileName);

                            //Create Home Controller
                            utilities.CreateControllerHome(tablesType);

                            application.generated = true;
                            applicationRepository.Edit(application);

                            //Zip Application Folder
                            string member_public_id = application.Member.public_id;
                            utilities.ZipApplication(application);

                            //Check the application has folders or not
                            //DirectoryUtility.DeleteFolder(Globals.APP_DATA_PATH + "\\" + member_public_id + "\\" + application.Name + "_" + application.Id);

                            return View("Success", new MessageView()
                            {
                                Message = "Your application has been generated successfully."
                            });
                        }
                    }
                    else
                    {
                        return View("Error", new MessageView()
                        {
                            Message = "An error while connecting to your database."
                        });
                    }
                }
                ViewBag.appId = application.Id;
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new MessageView()
                {
                    Message = "An error occured while generating the application"
                });
            }
        }


        [Authorize]
        [Route("DownloadApp")]
        public ActionResult DownloadApp(int id = 0)
        {
            string userName = User.Identity.Name;
            Application application = applicationRepository.FindFirst(ap => ap.Member.username == userName && ap.Id == id);

            if (application != null)
            {
                string fileName = application.Name + "_" + application.Id + ".zip";
                string content_type = "application/zip";

                string filePath = Globals.APP_DATA_PATH + "\\" + application.Member.public_id + "\\" + fileName;

                if (System.IO.File.Exists(filePath))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    return File(fileBytes, content_type, fileName);
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult PublishAzure(int id = 0)
        {
            try
            {
                if (id == 0)
                    return HttpNotFound();

                Application application = applicationRepository
                            .FindFirst(ap => ap.Member.username == User.Identity.Name && ap.Id == id);

                if (application != null)
                {
                    if (application.generated.HasValue && application.generated.Value)
                    {
                        string projectFileName = application.Name.Replace(" ", "");
                        WebDeploy deploy = new WebDeploy();
                        string publicId = application.Member.public_id;
                        string appFile = Server.MapPath("~/App_Data/" + publicId + "/" + application.Name + "_"+application.Id+"/" + projectFileName + ".csproj");
                        string pubXml = Server.MapPath("~/App_Data/"+publicId+"/"+application.Name+"#"+application.url+".pubxml");

                        string res = deploy.BuildApp(appFile, pubXml);
                        if (res == "Success")
                        {
                            return View("AzureSuccess", new MessageView()
                            {
                                Message = application.url
                            });
                        }
                        else
                        {
                            return View("Error", new MessageView()
                            {
                                Message = res//"An error occured, Application not Published."
                            });
                        }
                    }

                    return RedirectToAction("Details", new { id = id });
                }
            }
            catch (Exception ex)
            {
                return View("Error", new MessageView()
                {
                    Message = ex.Message
                });
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [Route("ConfigApp")]
        public ActionResult ConfigApp(int id = 0)
        {
            try
            {
                if (id == 0)
                    return HttpNotFound();

                Application application = applicationRepository
                            .FindFirst(ap => ap.Member.username == User.Identity.Name && ap.Id == id);

                if (application != null)
                {
                    //Get ConnectionString
                    string connectionString = Globals.GetSQLServerConnectionString(application);

                    List<AppTableView> appTables = GetApplicationTables(connectionString, application);

                    ViewBag.appId = id;
                    return View(appTables);
                }
            }
            catch (Exception ex)
            {
                return View("Error", new MessageView()
                {
                    Message = ex.Message
                });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("ConfigApp")]
        public ActionResult ConfigApp(int appId = 0, string[] selectedTables = null, string selectedTheme = "")
        {

            return View();
        }

        [Authorize]
        [Route("ViewApp")]
        public ActionResult ManageApp(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = applicationRepository.GetById(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        [HttpPost]
        public async Task<bool> StartStopApp(Application app)
        {
            Application application = applicationRepository.GetById(app.Id);
            string state = "";
            if (application.is_running.HasValue && application.is_running.Value == false)
            {
                application.is_running = true;
                state = "Running";
            }
            else{
                state = "Stopped";
                application.is_running = false;
            }
                
            applicationRepository.Edit(application);
            Websites website = new Websites();
            await website.ChangeAppState(application.url, state);
            return application.is_running.Value;
        }

        [HttpPost]
        public async Task<bool> RestartApp(Application app)
        {
            Application application = applicationRepository.GetById(app.Id);
                
            applicationRepository.Edit(application);
            Websites website = new Websites();
            await website.RestartWebSite(application.url);
            return true;
        }
        

        [Authorize]
        public ActionResult GetApplicationTables(int id)
        {
            if (id == 0)
                return HttpNotFound();

            Application application = applicationRepository
                        .FindFirst(ap => ap.Member.username == User.Identity.Name && ap.Id == id);

            if (application != null)
            {
                //Get ConnectionString
                string connectionString = Globals.GetSQLServerConnectionString(application);

                List<AppTableView> appTables = GetApplicationTables(connectionString, application);

                ViewBag.appId = id;

                return PartialView("_applicationTables", appTables);
            }
            return RedirectToAction("Index");
        }

        public ActionResult CompileApplication(int id = 0)
        {
            if (id == 0)
                return HttpNotFound();

            string userName = User.Identity.Name;
            Application application = applicationRepository.FindFirst(ap => ap.Member.username == userName && ap.Id == id);

            if (application != null)
            {
                string appName = application.Name + application.Id;
                string member_public_id = application.Member.public_id;
                Compile.CreateFromCSFiles(Globals.APP_DATA_PATH+"\\" + member_public_id + "\\" + appName, appName.Replace(" ", ""), application.db_name);
            }

            return RedirectToAction("Index");

        }

        private string CreateAppDirectories(Application application)
        {
            String dbName = application.db_name;
            String applicationName = application.Name;
            String member_public_id = application.Member.public_id;

            String memberDirectory = Globals.APP_DATA_PATH + "\\" + member_public_id + "\\";
            String applicationDirectoryName = applicationName + "_" + application.Id;
            memberDirectory = DirectoryUtility.CreateDirectory(memberDirectory, applicationDirectoryName);

            //Copy general application folders
            string appRoot = Environment.CurrentDirectory;
            string pth = Globals.APP_DATA_PATH + "\\ApplicationFolders\\";
            DirectoryUtility.CopyDirectory(pth, memberDirectory);

            //Create application directories
            ApplicationUtilities utilities = new ApplicationUtilities(memberDirectory, application.Name);
            utilities.CreateMainAppFiles(application);

            //Copy Theme File from App_Data/Themes
            string appTheme = application.Theme.FileName;
            System.IO.File.Copy(Globals.APP_DATA_PATH + "\\" + "Themes\\" + appTheme, memberDirectory + "\\Content\\" + appTheme);
            return memberDirectory;
        }


        private void CreateContextAndConfig(List<EntityType> tablesType, Application application,string appPath)
        {
            bool generated = DbContextGenerator.Generate(application.Name, appPath,tablesType);

            if (generated)
            {
                //Create web.config
                string connectionString = Globals.GetSQLServerConnectionString(application);
                ApplicationUtilities.CreateWebConfig(appPath, application, connectionString);

                //Create Global.asax
                ApplicationUtilities.CreateGlobalAsax(application,appPath);
            }
        }


        private List<AppTableView> GetApplicationTables(String connectionString, Application application)
        {
            var myGenerator = new MyGenerator(connectionString, application.Name,"");
            if (myGenerator.DatabaseTables!=null)
            {
                var tables = myGenerator.DatabaseTables.Select(tb => tb.Name).ToList<string>();
                var tablesNames = myGenerator.DatabaseTables.ToList();

                var appTablesView = new List<AppTableView>();
                foreach (var item in tables)
                {
                    AppTableView appTable = new AppTableView();
                    appTable.Name = item;
                    var currentTable = tablesNames.Where(t => t.Name == item).FirstOrDefault();
                    var columns = currentTable.Properties.ToList();
                    //Read table columns
                    foreach (var column in columns)
                    {
                        appTable.Columns.Add(new TableColumn
                        {
                            Name = column.Name
                        }); 
                    }
                    appTablesView.Add(appTable);
                }
                return appTablesView;
            }
            return null;
        }

        private void CreateProjectDLL(String appName, String dbName, String appDirectory)
        {
            Compile.CreateFromCSFiles(appDirectory, appName.Replace(" ", ""), dbName);
        }

    }


    #region Notes

    /*
     *
        //Compile datbase dll file
        //Compile.CompileDbDLL(dbName, System.IO.File.ReadAllText(appDirectory + dbName + ".cs"), appDirectory + "\\bin\\");

        //Delete .cs file
        //System.IO.File.Delete(appDirectory + dbName + ".cs");
     * 
     * 
     * //Compile datbase dll file
      Compile.CompileDbDLL(dbName, System.IO.File.ReadAllText(appDirectory + dbName + ".cs"), appDirectory + "\\bin\\");

      //Delete .cs file
      System.IO.File.Delete(appDirectory + dbName + ".cs");
     * 
     * //Compile project as dll file
     * CreateProjectDLL(application.Name, dbName, appDirectory);
     * 
     * */

    #endregion
}
