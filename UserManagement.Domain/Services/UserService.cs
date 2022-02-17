using UserManagement.Domain.Core.Interfaces;
using UserManagement.Domain.Dtos;
using UserManagement.Domain.Extensions;

namespace UserManagement.Domain.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public UserDto GetUserById(int id)
        {
            return new UserDto() { Id = 1, Name="Berk" };
        }

        public int CreateUser(UserDto userDto)
        {
            return UnitOfWork.UserRepository.CreateAndSave(userDto.ToEntity()).Id;
        }
    }
}
