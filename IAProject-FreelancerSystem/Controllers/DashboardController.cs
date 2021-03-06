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
                    return RedirectToAction("Profile", "FactoryLayout");
                }
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            // Update Data
            user = new UserDB().SelectwithId(user.userID.ToString());

            ViewData["User"] = user;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateAdmin(FormCollection formCollection)
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "client")
                {
                    return RedirectToAction("Profile", "FactoryLayout");
                }
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            // Action with Data
            User userToAdd = new User();
            userToAdd = new UserDB().SelectwithId(formCollection["userID"]);
            // Upload File
            if (Request.Files["postedFile"].ContentLength > 0)
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

            ViewData["User"] = user;

            return RedirectToAction("Profile");
        }

        // USERS PAGE
        public ActionResult UsersPage(string userID)
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "client")
                {
                    return RedirectToAction("Profile", "FactoryLayout");
                }
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            // All User
            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.role != "admin" );
            ViewData["Users"] = list;
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(FormCollection formCollection)
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
            return RedirectToAction("UsersPage");
        }

        [HttpPost]
        public ActionResult DeleteUser(FormCollection formCollection)
        {
            // Delete User
            new UserDB().Delete(formCollection["userID"]);
            // All User
            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.userID != 1);
            // Return to View Profile
            ViewData["Users"] = list;
            return RedirectToAction("UsersPage");
        }

        // POST PAGE
        public ActionResult PostsPage(string jobID)
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "client")
                {
                    return RedirectToAction("Profile", "FactoryLayout");
                }
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            Job job = new Job();
            if (jobID != null)
            {
                job = new JobDB().SelectwithId(jobID);
            }
            else
            {
                job = null;
            }
            // Jobs
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Accepted" && u.jobStatus == "Waitting");
            // Clients
            List<User> clients = new List<User>();
            for (int i = 0; i < list.Count(); i++)
            {
                clients.Add(new UserDB().SelectwithId(list[i].clientID.ToString()));
            }
            ViewData["Jobs"] = list;
            ViewData["Clients"] = clients;
            ViewData["Job"] = job;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateJob(FormCollection formCollection)
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "client")
                {
                    return RedirectToAction("Profile", "FactoryLayout");
                }
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

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
            // Clients
            List<User> clients = new List<User>();
            for (int i = 0; i < list.Count(); i++)
            {
                clients.Add(new UserDB().SelectwithId(list[i].clientID.ToString()));
            }
            ViewData["Jobs"] = list;
            ViewData["Job"] = null;
            ViewData["Clients"] = clients;
            return RedirectToAction("PostsPage");
        }
        
        public ActionResult DeleteJob(FormCollection formCollection)
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "client")
                {
                    return RedirectToAction("Profile", "FactoryLayout");
                }
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            // Get Job
            Job job = new Job();
            new JobDB().Delete(formCollection["jobID"]);

            // View Data
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Accepted" && u.jobStatus == "Waitting");
            // Clients
            List<User> clients = new List<User>();
            for (int i = 0; i < list.Count(); i++)
            {
                clients.Add(new UserDB().SelectwithId(list[i].clientID.ToString()));
            }
            ViewData["Jobs"] = list;
            ViewData["Job"] = null;
            ViewData["Clients"] = clients;
            return RedirectToAction("PostsPage");
        }

        // POST REQ
        public ActionResult PostsRequests()
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "client")
                {
                    return RedirectToAction("Profile", "FactoryLayout");
                }
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            // Jobs
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Waitting");
            // Clients
            List<User> clients = new List<User>();
            for(int i = 0; i<list.Count(); i++)
            {
                clients.Add(new UserDB().SelectwithId(list[i].clientID.ToString()));
            }

            ViewData["Jobs"] = list;
            ViewData["Clients"] = clients;
            return View();
        }

        [HttpPost]
        public ActionResult UpdateJobType(FormCollection formCollection)
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "client")
                {
                    return RedirectToAction("Profile", "FactoryLayout");
                }
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            // Get the Job
            Job job = new Job();
            job = new JobDB().SelectwithId(formCollection["jobID"]);
            // Update Role
            job.jobAdminAcceptance = formCollection["type"];
            // Set the Job
            new JobDB().Update(job);
            // Jobs
            List<Job> list = new List<Job>();
            list = new JobDB().SelectAll();
            list = list.FindAll(u => u.jobAdminAcceptance == "Waitting");
            // Clients
            List<User> clients = new List<User>();
            for (int i = 0; i < list.Count(); i++)
            {
                clients.Add(new UserDB().SelectwithId(list[i].clientID.ToString()));
            }

            ViewData["Jobs"] = list;
            ViewData["Clients"] = clients;
            return RedirectToAction("PostsRequests");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Profile", "Dashboard");
        }

    }
}