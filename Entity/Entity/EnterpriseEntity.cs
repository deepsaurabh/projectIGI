using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entity
{
    public class User : BaseEntity
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
    }
}
