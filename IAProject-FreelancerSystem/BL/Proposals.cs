using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAProject_FreelancerSystem.BL
{
    public class Proposals
    {
        public int propID { set; get; }
        public int jobID { set; get; }
        public int freelancerID { set; get; }
        public string propDescription { set; get; }
        public int propPrice { set; get; }
        public string clientAcceptance { set; get; }
    }
}