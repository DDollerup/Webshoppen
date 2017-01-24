using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webshoppen.Factories;
using Webshoppen.Models;

namespace Webshoppen.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        HomeFactory homeFac = new HomeFactory();

        public ActionResult Index()
        {
            Home home = homeFac.Get(1);
            return View(home);
        }

        [HttpPost]
        public ActionResult IndexSubmit(Home home)
        {
            homeFac.Update(home);
            TempData["MSG"] = "Home has been updated.";
            return RedirectToAction("Index");
        }
    }
}