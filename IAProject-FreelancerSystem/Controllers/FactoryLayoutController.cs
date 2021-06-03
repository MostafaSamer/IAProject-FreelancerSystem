using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IAProject_FreelancerSystem.Models;

namespace IAProject_FreelancerSystem.Controllers
{
    public class FactoryLayoutController : Controller
    {
        // GET: FactoryLayout
        public ActionResult Profile()
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Profile", "Dashboard");
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

            User lastUser = new UserDB().SelectwithId(user.userID.ToString());
            ViewData["User"] = lastUser;

            return View();
        }
        public ActionResult CreateNewPost()
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;
            // Users
            if (Session["User"] != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Profile", "Dashboard");
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

            return View();
        }
        [HttpPost]
        public ActionResult createnewpost(Job post)
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;
            // Users
            if (Session["User"] != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Profile", "Dashboard");
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

            post.clientID = user.userID;
            post.jobStatus = "Waitting";
            post.jobAdminAcceptance = "Waitting";
            int clientID = post.clientID;
            string jobTitle = post.jobTitle; //= formCollection["jobtitle"];
            int jobBudget = post.jobBudget; //= Int32.Parse(formCollection["jobbudget"]);
           // string creationDate = post.creationDate; // = formCollection["creationdate"];
            string jobDescription = post.jobDescription; //= formCollection["jobdescription"];
            new JobDB().Insert(post);

            List<Job> list_posts = new List<Job>();
            list_posts = new JobDB().SelectAll();
            list_posts = list_posts.FindAll(J => J.clientID == post.clientID );
            // Return to View Profile
            ViewData["jobs"] = list_posts;
            return View("MyPosts");
        }
           public ActionResult myposts(Job post)
           {

            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;
            // Users
            if (Session["User"] != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Profile", "Dashboard");
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

            List<Job> list_posts = new List<Job>();
            list_posts = new JobDB().SelectAll();
            list_posts = list_posts.FindAll(J => J.clientID == user.userID);
            // Return to View Profile
            ViewData["jobs"] = list_posts;
            return View("MyPosts");
        }
           public ActionResult receivedproposals(Proposal proposal)
           {

            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;
            // Users
            if (Session["User"] != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Profile", "Dashboard");
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

            // Get all User Job
            List<Job> list_posts = new JobDB().SelectAll();
            list_posts = list_posts.FindAll(J => J.clientID == user.userID);

            // Get all Proposels
            List<Proposal> list_proposals = new ProposalsDB().SelectAll();

            for(int i = 0; i<list_posts.Count(); i++)
            {
                // List<Proposal> temp = list_proposals.FindAll()
            }
            // ** We need to send Jobs , Proposel for each Job, Client for each Proposel **
            // Return to View Profile
            ViewData["proposals"] = list_proposals;
            return View("ReceivedProposals");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Profile", "FactoryLayout");
        }

        public ActionResult UpdateAdmin(FormCollection formCollection)
        {
            User user = new User();
            user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Users
            if (Session["User"] != null)
            {
                if (user.role == "admin")
                {
                    return RedirectToAction("Profile", "Dashboard");
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

            ViewData["User"] = user;

            return RedirectToAction("Profile");
        }

    }
}