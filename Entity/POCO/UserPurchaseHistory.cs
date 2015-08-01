using Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.POCO
{
   public class UserPurchaseHistory : BaseEntity
    {
       public Int64 UserId { get; set; }
       public Int64 ItemId { get; set; }
       public ItemType ItemType {get;set;}
       public int ItemCount { get; set; }

       [Column(TypeName = "VARCHAR")]
       [StringLength(255)]
       public string AmountPaid { get; set; }
       public DateTime TransactionDate { get; set;}
    }
}
