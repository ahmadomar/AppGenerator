using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppsGenerator.Classes.Generator
{
    public class ViewsType
    {
        /// <summary>
        /// List of the Controller views type
        /// </summary>
        /// <returns></returns>
        public static List<ViewType> ToList()
        {
            List<ViewType> generateViews = new List<ViewType>();
            generateViews.Add(ViewType.Create);
            generateViews.Add(ViewType.Update);
            generateViews.Add(ViewType.List);
            generateViews.Add(ViewType.Delete);
            generateViews.Add(ViewType.Details);
            return generateViews;
        }
    }
    public enum ViewType
    {
        Create,List,Delete,Update,Details
    }

}