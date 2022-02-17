using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.Dtos
{
    public class UserDto : BaseDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Fullname { get; set; }

        public string Mail { get; set; }

        public DateTime Birthday { get; set; }

        public List<PhoneDto> Phones { get; set; }
    }
}
