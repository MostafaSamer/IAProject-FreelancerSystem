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
                else if (user.role == "freelancer")
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
                else if (user.role == "freelancer")
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
                else if (user.role == "freelancer")
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
           public ActionResult myposts(string jobID)
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
                    else if (user.role == "freelancer")
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

            Job job = new Job();
            if (jobID != null)
            {
                job = new JobDB().SelectwithId(jobID);
            }
            else
            {
                job = null;
            }
            ViewData["Job"] = job;
            return View("MyPosts");
            }
           public ActionResult receivedproposals()
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
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            // Get all User Job
            List<Job> jobList = new JobDB().SelectAll();
            jobList = jobList.FindAll(J => J.clientID == user.userID);

            // Get all Proposels
            List<Proposal> AllproposalList = new ProposalsDB().SelectAll();
            List<Proposal> proposalList = new List<Proposal>();

            for (int i = 0; i < jobList.Count(); i++)
            {
                List<Proposal> temp = AllproposalList.FindAll(p => p.jobID == jobList[i].jobID);
                proposalList.AddRange(temp);
            }

            // Get all User that proposed
            List<User> userList = new List<User>();
            for(int i = 0; i<proposalList.Count(); i++)
            {
                userList.Add(new UserDB().SelectwithId(proposalList[i].freelancerID.ToString()));
            } 
            
            // ** We need to send Jobs , Proposel for each Job, Client for each Proposel **
            // Return to View Profile
            ViewData["jobList"] = jobList;
            ViewData["proposalList"] = proposalList;
            ViewData["userList"] = userList;
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
            User userToAdd = new UserDB().SelectwithId(user.userID.ToString());

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
            userToAdd.role = formCollection["role"];
            new UserDB().Update(userToAdd);

            ViewData["User"] = user;

            return RedirectToAction("Profile");
        }

        public ActionResult ChangePassword(FormCollection formCollection)
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
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            var oldPassword = formCollection["oldPassword"];
            var newPassword = formCollection["newPassword"];
            User userToEdit = new UserDB().SelectwithId(user.userID.ToString());
            if(oldPassword == userToEdit.userPassword)
            {
                userToEdit.userPassword = newPassword;
                new UserDB().Update(userToEdit);
            }



            return RedirectToAction("Profile");
        }

        public ActionResult EditJob(FormCollection formCollection)
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

            return RedirectToAction("MyPosts");
        }

        public ActionResult Search(FormCollection formCollection)
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
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }


            var dataToSearch = formCollection["dataToSearch"];

            List<Job> list_posts = new List<Job>();
            list_posts = new JobDB().SelectAll();
            list_posts = list_posts.FindAll(J => J.clientID == user.userID);
            if(dataToSearch != "")
                list_posts = list_posts.FindAll(J => J.jobTitle == dataToSearch);

            // Return to View Profile
            ViewData["jobs"] = list_posts;
            ViewData["Job"] = null;

            return View("MyPosts");
        }

        public ActionResult AcceptProposel(FormCollection formCollection)
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
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            // Update Job
            var jobID = formCollection["jobID"];
            var freelancerID = formCollection["freelancerID"];
            Job jobToEdit = new JobDB().SelectwithId(jobID);
            jobToEdit.freelancerID = Int32.Parse(freelancerID);
            jobToEdit.jobStatus = "Accepted";
            new JobDB().Update(jobToEdit);


            // Get all User Job
            List<Job> jobList = new JobDB().SelectAll();
            jobList = jobList.FindAll(J => J.clientID == user.userID);

            // Get all Proposels
            List<Proposal> AllproposalList = new ProposalsDB().SelectAll();
            List<Proposal> proposalList = new List<Proposal>();

            for (int i = 0; i < jobList.Count(); i++)
            {
                List<Proposal> temp = AllproposalList.FindAll(p => p.jobID == jobList[i].jobID);
                proposalList.AddRange(temp);
            }

            // Get all User that proposed
            List<User> userList = new List<User>();
            for (int i = 0; i < proposalList.Count(); i++)
            {
                userList.Add(new UserDB().SelectwithId(proposalList[i].freelancerID.ToString()));
            }

            // ** We need to send Jobs , Proposel for each Job, Client for each Proposel **
            // Return to View Profile
            ViewData["jobList"] = jobList;
            ViewData["proposalList"] = proposalList;
            ViewData["userList"] = userList;
            return View("ReceivedProposals");
        }

        public ActionResult DeletePost(FormCollection formCollection)
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
                else if (user.role == "freelancer")
                {
                    return RedirectToAction("Index", "Wall");
                }

            }
            else
            {
                return RedirectToAction("Index", "Wall");
            }

            var jobID = formCollection["jobID"];

            new JobDB().Delete(jobID);

            List<Rate> jobs_rate = new RateDB().SelectAll();
            jobs_rate = jobs_rate.FindAll(r => r.jobID == Int32.Parse(jobID));
            for(int i = 0; i<jobs_rate.Count(); i++)
            {
                new RateDB().Delete(jobs_rate[i].rateID.ToString());
            }
            
            List<SavedJob> jobs_saved = new SavedJobDB().SelectAll();
            jobs_saved = jobs_saved.FindAll(r => r.jobID == Int32.Parse(jobID));
            for(int i = 0; i< jobs_saved.Count(); i++)
            {
                new SavedJobDB().Delete(jobs_saved[i].savedID.ToString());
            }

            List<Proposal> jobs_proposal = new ProposalsDB().SelectAll();
            jobs_proposal = jobs_proposal.FindAll(r => r.jobID == Int32.Parse(jobID));
            for(int i = 0; i< jobs_proposal.Count(); i++)
            {
                new ProposalsDB().Delete(jobs_proposal[i].propID.ToString());
            }

            return RedirectToAction("MyPosts");
        }

    }
}