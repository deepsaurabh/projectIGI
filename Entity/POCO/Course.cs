using Core;
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

        public virtual ICollection<CourseDocument> CourseDocument { get; set; }
    }
}
