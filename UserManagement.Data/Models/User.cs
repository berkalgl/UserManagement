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
    [Table("USER_MANAGEMENT.USER")]
    public class User : Entity
    {
        [Column("NAME")]
        [StringLength(400)]
        public string Name { get; set; }

        [Column("SURNAME")]
        [StringLength(400)]
        public string Surname { get; set; }

        [NotMapped]
        public string Fullname { get { return Name + ' ' + Surname; } }

        [Column("SURNAME")]
        [StringLength(400)]
        public string Mail { get; set; }

        [Column("BIRTHDAY", TypeName = "Date")]
        public DateTime Birthday { get; set; }
    }
}
