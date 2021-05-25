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

        public ActionResult UsersPage(string userID)
        {
            // All User
            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.userID != 1 );
            // User to Edit if exist
            User user = new User();
            if(userID != null)
            {
                user = new UserDB().SelectwithId(userID);
            }
            else
            {
                user = null;
            }
            ViewData["Users"] = list;
            ViewData["User"] = user;

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

        [HttpPost]
        public ViewResult UpdateAdmin(FormCollection formCollection)
        {
            // Action with Data
            User userToAdd = new User();
            userToAdd.userID = Int32.Parse(formCollection["userID"]);
            userToAdd.fName = formCollection["fName"];
            userToAdd.lName = formCollection["lName"];
            userToAdd.userName = formCollection["userName"];
            userToAdd.email = formCollection["email"];
            userToAdd.phoneNum = formCollection["phoneNum"];
            userToAdd.userPassword = formCollection["userPassword"];
            userToAdd.userPhoto = "admin.png";
            userToAdd.role = formCollection["role"];
            new UserDB().Update(userToAdd);
            User user = new User();
            user = new UserDB().SelectwithId("1");
            // Return to View Profile
            ViewData["User"] = user;
            return View("Profile");
        }

        public ViewResult AddUser(FormCollection formCollection)
        {
            var test = formCollection["role"];
            // Action with Data
            User userToAdd = new User();
            userToAdd.fName = formCollection["fName"];
            userToAdd.lName = formCollection["lName"];
            userToAdd.userName = formCollection["userName"];
            userToAdd.email = formCollection["email"];
            userToAdd.phoneNum = formCollection["phoneNum"];
            userToAdd.userPassword = formCollection["userPassword"];
            userToAdd.userPhoto = "admin.png";
            userToAdd.role = formCollection["role"];
            new UserDB().Insert(userToAdd);
            // All User
            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.userID != 1);
            // Return to View Profile
            ViewData["Users"] = list;
            ViewData["User"] = null;
            return View("UsersPage");
        }

        public ViewResult UpdateUser(FormCollection formCollection)
        {
            // Action with Data
            User userToAdd = new User();
            userToAdd.userID = Int32.Parse(formCollection["userID"]);
            userToAdd.fName = formCollection["fName"];
            userToAdd.lName = formCollection["lName"];
            userToAdd.userName = formCollection["userName"];
            userToAdd.email = formCollection["email"];
            userToAdd.phoneNum = formCollection["phoneNum"];
            userToAdd.userPassword = formCollection["userPassword"];
            userToAdd.userPhoto = "admin.png";
            userToAdd.role = formCollection["role"];
            new UserDB().Update(userToAdd);
            // All User
            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.userID != 1);
            // Return to View Profile
            ViewData["Users"] = list;
            ViewData["User"] = null;

            return View("UsersPage");
        }

        public ViewResult DeleteUser(FormCollection formCollection)
        {
            // Delete User
            new UserDB().Delete(formCollection["userID"]);
            // All User
            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.userID != 1);
            // Return to View Profile
            ViewData["Users"] = list;
            ViewData["User"] = null;
            return View("UsersPage");
        }

    }
}