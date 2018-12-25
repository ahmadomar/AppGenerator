using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppsGenerator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.IsAuthenticated = true;
            else
                ViewBag.IsAuthenticated = false;
            return View();
        }
    }
}