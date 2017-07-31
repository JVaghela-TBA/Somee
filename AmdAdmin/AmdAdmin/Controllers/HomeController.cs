using System.Linq;
using System.Web.Mvc;
using System.Web.Http;

namespace AmdAdmin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View("index");
        }
        //[ValidateAntiForgeryToken]
        [System.Web.Mvc.HttpPost]
        public ActionResult Login([FromBody]AdminUser user)
        {
            if (ModelState.IsValid)
            {
                using (TestAmbEntity entity = new TestAmbEntity())
                {
                    var obj = entity.AdminUsers.Where(a => a.Username.Equals(user.Username) && a.Password.Equals(user.Password));
                    var x = obj.FirstOrDefault();
                    if (x!=null)
                    {
                        Session["username"] = x.Username;
                        Session["userid"] = x.UserId;
                        return RedirectToAction("Index", "DashBoard");
                    }
                }
            }
            return View("index");
        }

        public ActionResult LogOut()
        {
            Session["username"] = "";
            Session["userid"] = "";
            return View("index");
        }

	}
}