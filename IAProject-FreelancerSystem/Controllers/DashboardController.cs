using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using IAProject_FreelancerSystem.Models;

namespace IAProject_FreelancerSystem.Controllers
{
    public class DashboardController : Controller
    {
        // PROFILE
        public ActionResult Profile()
        {

            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "client")
                {
                    return RedirectToAction("");
                }
                else if (user.role == "Freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            ViewData["User"] = user;
            return View();
        }

        [HttpPost]
        public ViewResult UpdateAdmin(FormCollection formCollection)
        {
            // Action with Data
            User userToAdd = new User();

            // Upload File
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase postedFile = Request.Files["postedFile"];
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpg");

                var userPhoto = "https://localhost:44388/Uploads/" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpg";
                userToAdd.userPhoto = userPhoto;

            }

            userToAdd.userID = Int32.Parse(formCollection["userID"]);
            userToAdd.fName = formCollection["fName"];
            userToAdd.lName = formCollection["lName"];
            userToAdd.userName = formCollection["userName"];
            userToAdd.email = formCollection["email"];
            userToAdd.phoneNum = formCollection["phoneNum"];
            userToAdd.userPassword = formCollection["userPassword"];
            userToAdd.role = formCollection["role"];
            new UserDB().Update(userToAdd);

            User user = new User();
            user = new UserDB().SelectwithId("1");
            ViewData["User"] = user;

            return View("Profile");
        }

        // USERS PAGE
        public ActionResult UsersPage(string userID)
        {
            // All User
            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.userID != 1 );
            ViewData["Users"] = list;
            return View();
        }

        [HttpPost]
        public ViewResult AddUser(FormCollection formCollection)
        {
            var test = formCollection["role"];

            // Action with Data
            User userToAdd = new User();
            // Upload File
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase postedFile = Request.Files["postedFile"];
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                postedFile.SaveAs(path + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpg");

                userToAdd.userPhoto = "https://localhost:44388/Uploads/" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpg";

            }
            userToAdd.fName = formCollection["fName"];
            userToAdd.lName = formCollection["lName"];
            userToAdd.userName = formCollection["userName"];
            userToAdd.email = formCollection["email"];
            userToAdd.phoneNum = formCollection["phoneNum"];
            userToAdd.userPassword = formCollection["userPassword"];
            userToAdd.role = formCollection["role"];
            new UserDB().Insert(userToAdd);
            // All User
            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.userID != 1);
            // Return to View Profile
            ViewData["Users"] = list;
            return View("UsersPage");
        }

        [HttpPost]
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
            return View("UsersPage");
        }

        // POST PAGE
        public ActionResult PostsPage(string jobID)
        {
            Job job = new Job();
            if (jobID != null)
            {
                job = new JobDB().SelectwithId(jobID);
            }
            else
            {
                job = null;
            }
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Accepted" && u.jobStatus == "Waitting");
            ViewData["Jobs"] = list;
            ViewData["Job"] = job;
            return View();
        }

        [HttpPost]
        public ViewResult UpdateJob(FormCollection formCollection)
        {
            // Get Job
            Job jobToEdit = new Job();
            jobToEdit = new JobDB().SelectwithId(formCollection["jobID"]);
            jobToEdit.jobTitle = formCollection["jobTitle"];
            jobToEdit.jobBudget = Int32.Parse(formCollection["jobBudget"]);
            jobToEdit.jobType = formCollection["jobType"];
            jobToEdit.jobDescription = formCollection["jobDescription"];

            // Update Data
            new JobDB().Update(jobToEdit);

            // View Data
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Accepted" && u.jobStatus == "Waitting");
            ViewData["Jobs"] = list;
            ViewData["Job"] = null;
            return View("PostsPage");
        }
        
        public ViewResult DeleteJob(FormCollection formCollection)
        {
            // Get Job
            Job job = new Job();
            new JobDB().Delete(formCollection["jobID"]);

            // View Data
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Accepted" && u.jobStatus == "Waitting");
            ViewData["Jobs"] = list;
            ViewData["Job"] = null;
            return View("PostsPage");
        }

        // POST REQ
        public ActionResult PostsRequests()
        {
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Waitting");
            ViewData["Jobs"] = list;
            return View();
        }

        [HttpPost]
        public ViewResult UpdateJobType(FormCollection formCollection)
        {
            // Get the Job
            Job job = new Job();
            job = new JobDB().SelectwithId(formCollection["jobID"]);
            // Update Role
            job.jobAdminAcceptance = formCollection["type"];
            // Set the Job
            new JobDB().Update(job);
            // Get Jobs
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Waitting");
            ViewData["Jobs"] = list;
            return View("PostsRequests");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Profile", "Dashboard");
        }

    }
}