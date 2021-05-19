using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IAProject_FreelancerSystem.BL
{
    public class User
    {
        public int userID { set; get; }
        public string userPassword { set; get; }
        public string userName { set; get; }
        public string fName { set; get; }
        public string lName { set; get; }
        public string email { set; get; }
        public string phoneNum { set; get; }
        public string userPhoto { set; get; }
        public string role { set; get; }
    }

}