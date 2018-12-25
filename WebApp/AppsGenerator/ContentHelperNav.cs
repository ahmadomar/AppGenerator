using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AppsGenerator
{
    public static class ContentHelperNav
    {
        public static MvcHtmlString MenuLinkBootstrap(
this HtmlHelper htmlHelper,
string text, string actionName, string controllerName = null)
        {
            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");


            if (controllerName == currentController)
            {
                return new MvcHtmlString("<li class=\"active\"><a href='/"+ actionName + "'>" + text + "</a></li>");
            }
            return new MvcHtmlString("<li><a href='/" + actionName + "'>" + text + "</a></li>");
        }

    }
}
