using NewsPortal.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Concrete
{
    public class clsNewsType
    {
        public int CheckNewsType(string NewsType)
        {
            try
            {
                using (NewsPortalEntities portalEntities = new NewsPortalEntities())
                {
                    var items = portalEntities.tblNewsType.Where(x => x.NewsType == NewsType).ToList().Count;
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