using Core;
using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.POCO
{
    public class CheckOutAddress : BaseEntity
    {

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string UserName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string CompleteAddress { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(25)]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string State { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string City { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(25)]
        public string PinCode { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string EmailAddress { get; set; }
    }
}
