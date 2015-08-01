
using Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entity.POCO
{
    public class CourseDocument : BaseEntity
    {
        public Int64 CourseId { get; set; }
        public DocumentType DocumentType { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1023)]
        public string DocumentPath { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string DocumentName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string DocumentPseudoName { get; set; }
        public DocumentScope Scope { get; set; }

        public virtual Course Course { get; set; }

    }
}

