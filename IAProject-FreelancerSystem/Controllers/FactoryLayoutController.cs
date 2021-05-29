using System;
using System.Collections.Generic;
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
            user = new UserDB().SelectwithId("1");
            ViewData["User"] = user;

            return View();
        }
        public ActionResult CreateNewPost()
        {

            return View();
        }
        [HttpPost]
        public ActionResult createnewpost(Job post)
        {
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
           public ViewResult myposts(Job post)
           {
            List<Job> list_posts = new List<Job>();
            list_posts = new JobDB().SelectAll();
            list_posts = list_posts.FindAll(J => J.clientID == 201700000);
            // Return to View Profile
            ViewData["jobs"] = list_posts;
            return View("MyPosts");
        }
           public ViewResult receivedproposals(Proposal proposal)
           {
            int x = 0;
            List<Job> list_posts = new List<Job>();
            list_posts = new JobDB().SelectAll();
            list_posts = list_posts.FindAll(J => J.clientID == 2);

            List<Proposal> list_proposals = new List<Proposal>();
            list_proposals = new ProposalsDB().SelectAll();
            list_proposals = list_proposals.FindAll(p => p.jobID == list_posts[x].jobID);
            x++;
            // Return to View Profile
            ViewData["proposals"] = list_proposals;
            return View("ReceivedProposals");
        }
        
    }
}