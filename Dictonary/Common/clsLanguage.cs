using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Common
{
    public class clsLanguage
    {
        public int Id { get; set; }
        public string LangName { get; set; }
        public bool IsActive{ get; set; }
        public bool IsDeleted { get; set; }
    }
}