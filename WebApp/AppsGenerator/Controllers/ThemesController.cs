using AppsGenerator.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppsGenerator.Controllers
{
    public class ThemesController : Controller
    {
        private ThemeRepository rpTheme = new ThemeRepository();
        // GET: Themes
        public ActionResult GetAll()
        {
            var all = rpTheme.GetAll().ToList();

            return PartialView("themes_view",all);
        }
    }
}