using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppsGenerator.Classes.Layers.Controllers
{
    interface IGenericController<ControllerModel>
    {
        ActionResult Index(int? page);
        ActionResult Create();
        ActionResult Create(ControllerModel model);
        ActionResult Edit(int id = 0);
        ActionResult Edit(ControllerModel model);
        ActionResult Delete(int id = 0);
        ActionResult DeleteConfirmed(int id);
    }
}