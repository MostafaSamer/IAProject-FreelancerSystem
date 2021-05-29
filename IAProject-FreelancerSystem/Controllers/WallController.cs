using System;
using System.Collections.Generic;
using System.IO;
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

                if (user.role == "Admin")
                {
                    return RedirectToAction("Profile", "Dashboard");
                }
                else if (user.role == "Client")
                {
                    return RedirectToAction("");
                }
                else
                {
                    // ProposeledJob
                    List<Proposal> proposelJob = new List<Proposal>();
                    proposelJob = new ProposalsDB().SelectAll();
                    proposelJob = proposelJob.FindAll(p => p.freelancerID == user.userID);
                    ViewData["proposelJob"] = proposelJob;

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


            }

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return View();
        }

        [HttpPost]
        public ActionResult LoginForm(FormCollection formCollection)
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

                var user = Session["User"] as IAProject_FreelancerSystem.Models.User;

                // ProposeledJob
                List<Proposal> proposelJob = new List<Proposal>();
                proposelJob = new ProposalsDB().SelectAll();
                proposelJob = proposelJob.FindAll(p => p.freelancerID == user.userID);
                ViewData["proposelJob"] = proposelJob;

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

            return RedirectToAction("Index");
        }
        
        public ActionResult LogoutForm(FormCollection formCollectiion)
        {
            // User
            Session["User"] = null;

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return RedirectToAction("Index");
        }
        
        public ActionResult RegisterForm(FormCollection formCollectiion)
        {
            User user = new User();
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

                user.userPhoto = "https://localhost:44388/Uploads/" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpg";

            }
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

            return RedirectToAction("Index");
        }
        public ActionResult SaveJob(FormCollection formCollectiion)
        {
            // Save the Job
            var UserID = formCollectiion["userID"];
            var JobID = formCollectiion["jobID"];
            SavedJob saveJob = new SavedJob();
            saveJob.jobID = Int32.Parse(JobID);
            saveJob.freelancerID = Int32.Parse(UserID);
            new SavedJobDB().Insert(saveJob);

            var user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // ProposeledJob
            List<Proposal> proposelJob = new List<Proposal>();
            proposelJob = new ProposalsDB().SelectAll();
            proposelJob = proposelJob.FindAll(p => p.freelancerID == user.userID);
            ViewData["proposelJob"] = proposelJob;

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

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return RedirectToAction("Index");
        }

        public ActionResult GiveRate(FormCollection formCollection)
        {
            // Get Data
            var userID = formCollection["userID"];
            var jobID = formCollection["jobID"];
            var rateToAdd = formCollection["rateToAdd"];

            // Update Rate Ang in this Jobs
            List<Rate> listOldRate = new RateDB().SelectAll();
            var numUser = listOldRate.FindAll(r => r.jobID == Int32.Parse(jobID)).Count();
            var oldJob = new JobDB().SelectwithId(jobID);
            var oldAvgRate = oldJob.jobAVGRate;
            var newRate = ((oldAvgRate*numUser)+Int32.Parse(rateToAdd))/(numUser+1);
            oldJob.jobAVGRate = newRate;
            new JobDB().Update(oldJob);

            // Save the Rate
            Rate rate = new Rate();
            rate.freelancerID = Int32.Parse(userID);
            rate.jobID = Int32.Parse(jobID);
            rate.rate = Int32.Parse(rateToAdd);
            new RateDB().Insert(rate);
            

            var user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // ProposeledJob
            List<Proposal> proposelJob = new List<Proposal>();
            proposelJob = new ProposalsDB().SelectAll();
            proposelJob = proposelJob.FindAll(p => p.freelancerID == user.userID);
            ViewData["proposelJob"] = proposelJob;

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

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return RedirectToAction("Index");
        }

        public ActionResult GivePropsel(FormCollection formCollection)
        {
            // Get the data
            var userID = formCollection["userID"];
            var jobID = formCollection["jobID"];
            var propPrice = formCollection["propPrice"];
            var propDescription = formCollection["propDescription"];
            var clientAcceptance = "Waitting";

            Proposal proposal = new Proposal();
            proposal.freelancerID = Int32.Parse(userID);
            proposal.jobID = Int32.Parse(jobID);
            proposal.propPrice = Int32.Parse(propPrice);
            proposal.propDescription = propDescription;
            proposal.clientAcceptance = clientAcceptance;

            new ProposalsDB().Insert(proposal);

            var user = Session["User"] as IAProject_FreelancerSystem.Models.User;

            // ProposeledJob
            List<Proposal> proposelJob = new List<Proposal>();
            proposelJob = new ProposalsDB().SelectAll();
            proposelJob = proposelJob.FindAll(p => p.freelancerID == user.userID);
            ViewData["proposelJob"] = proposelJob;

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

            // Jobs
            List<Job> jobs = new List<Job>();
            jobs = new JobDB().SelectAll();
            jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

            ViewData["Jobs"] = jobs;

            return RedirectToAction("Index");
        }

        public ActionResult SavedJobs() {

            if (Session["User"] == null)
            {
                // Jobs
                List<Job> jobs = new List<Job>();
                jobs = new JobDB().SelectAll();
                jobs = jobs.FindAll(j => j.jobAdminAcceptance == "Accepted" && j.jobStatus == "Waitting");

                ViewData["Jobs"] = jobs;

                return RedirectToAction("Index");
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