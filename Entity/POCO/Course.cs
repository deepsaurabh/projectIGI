using Core;
using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.POCO
{
    public class Course : BaseEntity
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string CourseName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string CourseFreeContent { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string CoursePublicContent { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string CoursePaidContent { get; set; }
        
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Price { get; set; }
        public Currency CurrencyType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CourseAttachmentMapping : BaseEntity
    {
        public long courseId { get; set; }
        public virtual Course course { get; set; }

        public long courseDocumentID { get; set; }
        public virtual CourseDocument courseDocument { get; set; }
    }
}
