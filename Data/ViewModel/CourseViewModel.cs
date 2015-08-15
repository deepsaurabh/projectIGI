using Data.Enum;
using Entity.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.ViewModel
{
    public class BaseViewModel
    {
        public string price { get; set; }
        public Currency currencyType { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public Content freeContent { get; set; }
        public Content publicContent { get; set; }
        public Content paidContent { get; set; }
    }

    public class CourseViewModel : BaseViewModel
    {
        public Int64 courseID { get; set; }
        public string courseName { get; set; }

    }

    public class ToolkitViewModel : BaseViewModel
    {
        public Int64 toolkitID { get; set; }
        public string toolkitName { get; set; }
    }

    public class Content
    {
        public String description { get; set; }
        public List<DocumentAttached> fileAttachment { get; set; }
    }
    public class DocumentAttached
    {
        public long attachmentID { get; set; }
        public bool isDeleted { get; set; }
        public String imageURL { get; set; }
        public String documentName { get; set; }
        public DocumentScope documentScope { get; set; }
    }
}

