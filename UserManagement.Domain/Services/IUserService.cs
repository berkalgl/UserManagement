using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Dtos;

namespace UserManagement.Domain.Services
{
    public interface IUserService
    {
        UserDto GetUserById(int id);

        int CreateUser(UserDto userDto);
    }
}
