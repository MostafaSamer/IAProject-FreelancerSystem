using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAProject_FreelancerSystem.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult UsersPage()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult PostsPage()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PostsRequests()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}