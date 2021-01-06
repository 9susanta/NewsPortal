using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Common
{
    public class WordLanguage
    {
        public int ValId { get; set; }
        public string ValLangName { get; set; }
        public int KeyId { get; set; }
        public string KeyLangName { get; set; }
        public int RulesId { get; set; }
        public string RulesName { get; set; }
        public int LangId { get; set; }
        public string LangName { get; set; }
        public bool ValIsActive { get; set; }
        public bool ValIsDeleted { get; set; }
        public bool KeyIsActive { get; set; }
        public bool KeyIsDeleted { get; set; }
    }
}