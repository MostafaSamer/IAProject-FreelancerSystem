using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using IAProject_FreelancerSystem.Models;

namespace IAProject_FreelancerSystem.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Profile()
        {
            User user = new User();
            user = new UserDB().SelectwithId("1");
            ViewData["User"] = user;

            return View();
        }

        public ActionResult UsersPage()
        {

            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.userID != 1 );
            ViewData["Users"] = list;

            return View();
        }

        public ActionResult PostsPage()
        {
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Accepted" && u.jobStatus == "Waitting");
            ViewData["Jobs"] = list;
            return View();
        }

        public ActionResult PostsRequests()
        {
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Waitting");
            ViewData["Jobs"] = list;
            return View();
        }

    }
}