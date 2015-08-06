
using Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entity.POCO
{
    public class CourseDocument : BaseEntity
    {        
        public string FileType { get; set; }
        
        public long? FileSize { get; set; }
        
        public byte[] FileData { get; set; }

        public DocumentType DocumentType { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string DocumentName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string DocumentPseudoName { get; set; }

        public DocumentScope Scope { get; set; }       

    }
}

