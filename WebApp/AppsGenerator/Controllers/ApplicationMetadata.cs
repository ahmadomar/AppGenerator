using AppsGenerator.Classes.Layers.Controllers;
using AppsGenerator.Classes.Utilities;
using AppsGenerator.Models;
using AppsGenerator.Models.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using AppsGenerator.Classes.Azure;
using AppsGenerator.CSharpGenerator.Web.WebDeploy;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AppsGenerator.Controllers
{
    public partial class ApplicationController {

        public ApplicationRepository applicationRepository = new ApplicationRepository();
        public MemberRepository memberRepository = new MemberRepository();
        public ThemeRepository themeRepository = new ThemeRepository();

        [Authorize]
        public ActionResult Index(int? page,string search = "")
        {
            Member _member = memberRepository.GetAll().Where(m => m.username == User.Identity.Name).FirstOrDefault();
            if (_member != null)
            {
                var apps = applicationRepository.GetAll()
                                .Where(ap => ap.member_id == _member.id)
                               .ToList().OrderByDescending(ap => ap.created_at);
                if (search != "")
                {
                    apps = apps.Where(d => d.Name.Contains(search))
                                .ToList().OrderByDescending(ap => ap.created_at);
                    ViewBag.Search = search;
                }
                
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(apps.ToPagedList(pageNumber, pageSize));
            }
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [Route("CreateApp")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateApp")]
        public async Task<ActionResult> Create(Application model)
        {
            try
            {
                string username = User.Identity.Name;

                var member  = memberRepository.GetAll()
                    .Where(m => m.username == username).FirstOrDefault();
                //Save Application to db
                model.member_id = member.id;
                model.created_at = DateTime.Now;


                await CreatePubXML(model, member.public_id);
                
                applicationRepository.Insert(model);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return Content("An Error");
            }
        }

        private async Task<string> CreatePubXML(Application app,string PublicId)
        {
            if (app == null)
                return "App is Null";
            //Create Azure Website
            Websites website = new Websites();
            WebSiteGetPublishProfileResponse pubProfile = await website.CreateWebsite(app.url);

            if (pubProfile == null)
                return "pubProfile is Null";

            var profile = pubProfile.PublishProfiles[0];
            //Create Publish XML File
            string pubPath = Server.MapPath("~/App_Data/" + PublicId + "/");
            string pubXmlFile = app.Name + "#" + app.url + ".pubxml";

            PubXML pubXml = new PubXML();
            pubXml.UserName = profile.UserName;
            pubXml.Password = profile.UserPassword;
            pubXml.MSDeployServiceURL = profile.PublishUrl;
            pubXml.DeployIisAppPath = profile.MSDeploySite;
            pubXml.SiteUrlToLaunchAfterPublish = profile.DestinationAppUri.AbsoluteUri;

            System.IO.File.AppendAllText(pubPath + pubXmlFile, pubXml.TransformText());
            return profile.DestinationAppUri.AbsoluteUri;
        }

        [Authorize]
        [Route("EditApp")]
        public ActionResult Edit(int id = 0)
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



        // POST: /Application/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Route("EditApp")]
        public ActionResult Edit([Bind(Include = "id,name,created_at,db_datasource,db_name,db_user_id,db_password")] Application model)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;

                var memberId = memberRepository.GetAll()
                    .Where(m => m.username == username)
                    .Select(m => m.id).FirstOrDefault();
                model.member_id = memberId;
                applicationRepository.Edit(model);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /DeleteApp/5
        [Authorize]
        [Route("DeleteApp")]
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string userName = User.Identity.Name;
            Application application = applicationRepository.FindFirst(ap => ap.Member.username == userName && ap.Id == id);
            if (application == null)
            {
                return HttpNotFound();
            }

            return View(application);
        }

        // POST: /Application/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("DeleteApp")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            string userName = User.Identity.Name;
            Application application = applicationRepository.FindFirst(ap => ap.Member.username == userName && ap.Id == id);
            if (application != null)
            {
                string member_public_id = application.Member.public_id;

                applicationRepository.Delete(application);

                //Check the application files
                DeleteExistsFile(application);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [Route("AppInfo")]
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }

            string userName = User.Identity.Name;
            Application application = applicationRepository.FindFirst(ap => ap.Member.username == userName && ap.Id == id);
            if (application != null)
            {
                string member_public_id = application.Member.public_id;
                return View(application);
            }
            return RedirectToAction("Index");
        }

        public void DeleteExistsFile(Application application)
        {
            String MainAppsPath = Globals.APP_DATA_PATH + "\\";
            if (application.Member == null)
                application.Member = memberRepository.GetById(application.member_id);
            string path = String.Format("{0}{1}\\{2}_{3}.zip", MainAppsPath, application.Member.public_id, application.Name, application.Id);
            bool exists = System.IO.File.Exists(path);
            if (exists)
                System.IO.File.Delete(path);
        }

        #region Azure

        [HttpPost]
        public async Task<bool> CheckWebsiteUrl(Application application)
        {
            if(ModelState.IsValidField("url"))
            {
                Websites website = new Websites();
                return await website.IsAvailableName(application.url);
            }
            return false;
        }

        #endregion


        [HttpPost]
        public bool CheckConnection(Application application)
        {
            if (ModelState.IsValidField("db_datasource") && ModelState.IsValidField("db_name"))
            {
                SqlConnection connection = null;
                try
                {
                    string connectionString = Globals.GetSQLServerConnectionString(application);
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            else
                return false;
        }

        protected int GetTheme(string selectedTheme = "")
        {
            int themeId = 0;
            if (string.IsNullOrEmpty(selectedTheme))
            {
                Theme theme = themeRepository.FindFirst(th => th.Name == "Cerulean");
                themeId = theme.Id;
            }
            else
            {
                themeId = int.Parse(selectedTheme);
            }
            return themeId;
        }
    }
}