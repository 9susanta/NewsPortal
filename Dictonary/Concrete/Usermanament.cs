using log4net;
using NewsPortal.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Concrete
{
    public class Usermanament
    {
        ILog Log = log4net.LogManager.GetLogger(typeof(Usermanament));
        public clsUsers Login(string UserName,string Password)
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    var loggeduser = (from user in portalEntities.tblUsers
                                      join rol in portalEntities.tblRoles on user.RoleId equals rol.RoleId
                                      where user.UserName == UserName && user.Password == Password && (user.IsDeleted == false && user.IsBlocked == false)
                                      select new {user.UserName,user.FullName,rol.RoleName,user.UserId }).ToList();


                    if (loggeduser.Count>0)
                    {
                        return new clsUsers {
                            FullName = loggeduser.FirstOrDefault().FullName,
                            UserName = loggeduser.FirstOrDefault().UserName,
                            RoleName = loggeduser.FirstOrDefault().RoleName,
                            UserId = loggeduser.FirstOrDefault().UserId
                        };
                    }
                }
            }
            catch (Exception ex)
            {

                Log.Info(ex.ToString());
                throw ex;
            }
            return null;
        }
    }
}