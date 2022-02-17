using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.Dtos
{
    public class BaseDto
    {
        public int Id { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreationDate { get; set; }

        public int? UpdatedByUserId { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
