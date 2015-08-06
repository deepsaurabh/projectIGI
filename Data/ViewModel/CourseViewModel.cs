using Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModel
{
    public class CourseViewModel
    {
        public Int64 CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseFreeContent { get; set; }
        public string CoursePublicContent { get; set; }
        public string CoursePaidContent { get; set; }
        public string Price { get; set; }
        public Currency CurrencyType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long FreeContentImageId { get; set; }
        public long PublicContentImageId { get; set; }
        public long PaidContentImageId { get; set; }
        public String ImageURL { get; set; }
    }
}
