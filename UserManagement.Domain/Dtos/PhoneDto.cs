using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.Dtos
{
    public class PhoneDto
    {
        public int UserId { get; set; }

        public string Number { get; set; }

        public int Type { get; set; }
    }
}
