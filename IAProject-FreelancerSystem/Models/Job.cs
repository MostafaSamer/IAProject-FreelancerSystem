using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAProject_FreelancerSystem.Models
{
    public class Job
    {
        public int jobID { set; get; }
        public int freelancerID { set; get; }
        public int clientID { set; get; }
        public string jobTitle { set; get; }
        public int jobBudget { set; get; }
        public string jobType { set; get; }
        public string creationDate { set; get; }
        public string jobDescription { set; get; }
        public int jobAVGRate { set; get; }
        public string jobStatus { set; get; }
        public string jobAdminAcceptance { set; get; }
        public int propCount { set; get; }
    }
}