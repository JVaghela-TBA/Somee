using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AmdAdmin.Controllers
{
    public class DashBoardController : Controller
    {
        //
        // GET: /DashBoard/
        public ActionResult Index()
        {
            if (Session["username"] == null && Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("home");
        }
	}
}