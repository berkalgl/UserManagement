using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Data.Models.Base
{
    public class Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column("CREATED_BY_USER_ID")]
        public int CreatedByUserId { get; set; }

        [Column("CREATION_DATE")]
        public DateTime CreationDate { get; set; }

        [Column("UPDATED_BY_USER_ID")]
        public int? UpdatedByUserId { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        [Column("IS_DELETED")]
        public bool IsDeleted { get; set; }
    }
}
