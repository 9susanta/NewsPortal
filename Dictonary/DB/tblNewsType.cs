//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewsPortal.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblNewsType
    {
        public int Id { get; set; }
        public string OdiaName { get; set; }
        public string NewsType { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsMenu { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
