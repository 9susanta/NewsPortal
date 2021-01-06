using NewsPortal.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Concrete
{
    public class ClsRole
    {
        public int CheckRoleName(string RoleName)
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    var items = portalEntities.tblRoles.Where(x => x.RoleName == RoleName).ToList().Count;
                    return items;
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
    }
}