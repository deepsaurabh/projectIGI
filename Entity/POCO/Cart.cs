using Core;
using Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.POCO
{
    public class Cart : BaseEntity
    {

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string UserName { get; set; }
        public Int64 ItemId { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string type { get; set; }
        public int quantity { get; set; }
    }
}
