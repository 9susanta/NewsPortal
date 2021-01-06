using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Common
{
    public class NewsType
    {
        public int NewsTypeId { get; set; }
        public string NewsTypeName { get; set; }
        public string NewsTypeOdia { get; set; }
        public bool IsMenu { get; set; }
        public bool IsDeleted { get; set; }
    }
}