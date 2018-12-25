using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppsGenerator.Models
{
    public class AppTableView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TableColumn> Columns = new List<TableColumn>();
    }

    public class TableColumn
    {
        public string Name { get; set; }
    }
}