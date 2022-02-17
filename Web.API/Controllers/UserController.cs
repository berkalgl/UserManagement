using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Domain.Dtos;
using UserManagement.Domain.Services;
using UserManagement.WebApi.Extensions;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public ServiceResult<UserDto> GetUserById(int id)
        {
            return new ServiceResult<UserDto>()
            {
                Result = _userService.GetUserById(id),
                State = ServiceResultStates.SUCCESS,
                Message = "Success"
            };
        }
    }
}
