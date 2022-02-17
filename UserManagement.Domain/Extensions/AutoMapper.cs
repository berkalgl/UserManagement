using UserManagement.Data.Models;
using UserManagement.Domain.Dtos;

namespace UserManagement.Domain.Extensions
{
    public static class AutoMapper
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto() { 
                Id = user.Id,
                Name = user.Name,
                Birthday = user.Birthday,
                CreatedByUserId = user.CreatedByUserId,
                CreationDate = user.CreationDate,
                Fullname = user.Fullname,
                IsDeleted = user.IsDeleted,
                UpdatedByUserId = user.UpdatedByUserId,
                UpdateDate = user.UpdateDate,
                Mail = user.Mail,
                Surname = user.Surname
            };
        }

        public static User ToEntity(this UserDto userDto)
        {
            return new User()
            {
                Id = userDto.Id,
                Name = userDto.Name,
                Birthday = userDto.Birthday,
                Mail = userDto.Mail,
                Surname = userDto.Surname
            };
        }
    }
}
