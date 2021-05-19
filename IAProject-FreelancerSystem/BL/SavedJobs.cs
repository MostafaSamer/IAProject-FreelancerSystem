using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAProject_FreelancerSystem.BL
{
    public class SavedJobs
    {
        public int savedID { set; get; }
        public int freelancerID { set; get; }
        public int jobID { set; get; }
        public string jobTitle { set; get; }
        public string jobDate { set; get; }
        public string clientName { set; get; }
    }
}