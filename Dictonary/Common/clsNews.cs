using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Common
{
    public class clsNews
    {
        public decimal Id
        { get; set; }

        public string EnglishTitle
        { get; set; }

        public string OdiaTitle
        { get; set; }

        public string EngShortDesc
        { get; set; }

        public string SeoMeta
        { get; set; }

        public string ODShortDesc
        { get; set; }

        public string ODContent
        { get; set; }

        public DateTime? PostedOn
        { get; set; }

        public DateTime? PostedDate
        { get; set; }

        public DateTime? Modified
        { get; set; }

        public int? CategoryId
        { get; set; }

        public string Category
        { get; set; }

        public string CategoryOdia
        { get; set; }

        public string Tags
        { get; set; }

        public string HeaderImageName
        { get; set; }

        public string Reviewed
        { get; set; }

        public string Created
        { get; set; }

        public bool IsReviewed
        { get; set; }

        public bool IsDeleted
        { get; set; }

        public int? CreatedBy
        { get; set; }

        public int? ReviewedBy
        { get; set; }
    }
}