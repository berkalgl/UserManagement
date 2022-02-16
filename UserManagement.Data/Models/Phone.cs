using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Data.Models.Base;

namespace UserManagement.Data.Models
{
    public class Phone : Entity
    {
        [ForeignKey("User")]
        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("NUMBER")]
        [StringLength(400)]
        public string Number { get; set;}

        [Column("TYPE")]
        public int Type { get; set; }

        public virtual User User { get; set; }
    }
}
