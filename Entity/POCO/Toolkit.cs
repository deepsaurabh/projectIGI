using Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.POCO
{
    public class Toolkit : BaseEntity
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string ToolkitName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string ToolkitFreeContent { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string ToolkitPublicContent { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1000)]
        public string ToolkitPaidContent { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Price { get; set; }
        public Currency CurrencyType { get; set; }
        public virtual ICollection<ToolkitDocument> ToolkitDocument { get; set; }
    }
}
