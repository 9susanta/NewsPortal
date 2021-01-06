using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Common
{
    public class Users
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNo { get; set; }
        public string EmailID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public DateTime DateOfCreate { get; set; }
        public DateTime DateOfUpdate { get; set; }
        public int NoofFaliureAttempt { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }

    }
}