using Core;
using Entity.POCO;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.POCO
{
    public class User : BaseEntity
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public String FirstName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public String LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
