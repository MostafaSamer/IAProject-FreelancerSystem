using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using IAProject_FreelancerSystem.Models;

namespace IAProject_FreelancerSystem.Controllers
{
    public class WallController : Controller
    {
        public ActionResult Index()
        {
            // Users
            if(Session["User"] != null)
            {
                var user = Session["User"] as IAProject_FreelancerSystem.Models.User;
                // SavedJob
                List<SavedJob> savedJobs = new List<SavedJob>();
                savedJobs = new SavedJobDB().SelectAll();
                savedJobs = savedJobs.FindAll(s => s.freelancerID == user.userID);
                ViewData["savedJobs"] = savedJobs;

                // RatedJob
                List<Rate> ratedJobs = new List<Rate>();
                ratedJobs = new RateDB().SelectAll();
                ratedJobs = ratedJobs.FindAll(r => r.freelancerID == user.userID);
                ViewData["ratedJobs"] = ratedJobs;


            }

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return View();
        }

        [HttpPost]
        public ViewResult LoginForm(FormCollection formCollection)
        {
            var userName = formCollection["userName"];
            var password = formCollection["password"];
            List<User> list = new List<User>();
            list = new UserDB().SelectAll();
            list = list.FindAll(u => u.userName == userName && u.userPassword == password);
            if(list.Count() == 0)
            {
                Session["User"] = null;
            }
            else
            {
                Session["User"] = list[0];
                ViewData["Role"] = list[0].role;
            }

            // SavedJob
            var user = Session["User"] as IAProject_FreelancerSystem.Models.User;
            List<SavedJob> savedJobs = new List<SavedJob>();
            savedJobs = new SavedJobDB().SelectAll();
            savedJobs = savedJobs.FindAll(s => s.freelancerID == user.userID);
            ViewData["savedJobs"] = savedJobs;

            // RatedJob
            List<Rate> ratedJobs = new List<Rate>();
            ratedJobs = new RateDB().SelectAll();
            ratedJobs = ratedJobs.FindAll(r => r.freelancerID == user.userID);
            ViewData["ratedJobs"] = ratedJobs;

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return View("Index");
        }
        
        public ViewResult LogoutForm(FormCollection formCollectiion)
        {
            // User
            Session["User"] = null;

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return View("Index");
        }
        
        public ViewResult RegisterForm(FormCollection formCollectiion)
        {
            User user = new User();
            user.userPhoto = formCollectiion["userPhoto"];
            user.fName = formCollectiion["fName"];
            user.lName = formCollectiion["lName"];
            user.userName = formCollectiion["userName"];
            user.email = formCollectiion["email"];
            user.phoneNum = formCollectiion["phoneNum"];
            user.userPassword = formCollectiion["userPassword"];
            user.role = formCollectiion["role"];

            new UserDB().Insert(user);

            // User
            Session["User"] = null;

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return View("Index");
        }
        public ViewResult SaveJob(FormCollection formCollectiion)
        {
            // Save the Job
            var UserID = formCollectiion["userID"];
            var JobID = formCollectiion["jobID"];
            SavedJob saveJob = new SavedJob();
            saveJob.jobID = Int32.Parse(JobID);
            saveJob.freelancerID = Int32.Parse(UserID);
            new SavedJobDB().Insert(saveJob);

            // SavedJob
            var user = Session["User"] as IAProject_FreelancerSystem.Models.User;
            List<SavedJob> savedJobs = new List<SavedJob>();
            savedJobs = new SavedJobDB().SelectAll();
            savedJobs = savedJobs.FindAll(s => s.freelancerID == user.userID);
            ViewData["savedJobs"] = savedJobs;

            // RatedJob
            List<Rate> ratedJobs = new List<Rate>();
            ratedJobs = new RateDB().SelectAll();
            ratedJobs = ratedJobs.FindAll(r => r.freelancerID == user.userID);
            ViewData["ratedJobs"] = ratedJobs;

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return View("Index");
        }

        public ViewResult GiveRate(FormCollection formCollection)
        {
            // Get Data
            var userID = formCollection["userID"];
            var jobID = formCollection["jobID"];
            var rateToAdd = formCollection["rateToAdd"];

            // Save the Rate
            Rate rate = new Rate();
            rate.freelancerID = Int32.Parse(userID);
            rate.jobID = Int32.Parse(jobID);
            rate.rate = Int32.Parse(rateToAdd);
            new RateDB().Insert(rate);

            // SavedJob
            var user = Session["User"] as IAProject_FreelancerSystem.Models.User;
            List<SavedJob> savedJobs = new List<SavedJob>();
            savedJobs = new SavedJobDB().SelectAll();
            savedJobs = savedJobs.FindAll(s => s.freelancerID == user.userID);
            ViewData["savedJobs"] = savedJobs;

            // RatedJob
            List<Rate> ratedJobs = new List<Rate>();
            ratedJobs = new RateDB().SelectAll();
            ratedJobs = ratedJobs.FindAll(r => r.freelancerID == user.userID);
            ViewData["ratedJobs"] = ratedJobs;

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return View("Index");
        }

        public ActionResult SavedJobs() {

            if (Session["User"] == null)
            {
                // Jobs
                List<Job> jobs = new List<Job>();
                jobs = new JobDB().SelectAll();
                jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

                ViewData["Jobs"] = jobs;

                return View("Index");
            }

            var user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // Get Saved Job for the user in the session
            List<SavedJob> SavedJobs = new List<SavedJob>();
            SavedJobs = new SavedJobDB().SelectAll();
            SavedJobs = SavedJobs.FindAll(s => s.freelancerID == user.userID);

            // Get All Jobs saved for the user
            List<Job> Jobs = new List<Job>();
            List<Job> JobsToSend = new List<Job>();
            Jobs = new JobDB().SelectAll();
            for(int i = 0; i<SavedJobs.Count(); i++)
            {
                JobsToSend.AddRange(Jobs.FindAll(j => j.jobID == SavedJobs[i].jobID));
            }

            ViewData["SavedJobs"] = JobsToSend;

            return View();
        }
    }
}