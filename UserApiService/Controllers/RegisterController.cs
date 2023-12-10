using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserApiService.Models;

namespace UserApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public RegisterController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(UserRegister newUser)
        {
            var user = _mapper.Map<User>(newUser);
            var result = await _userManager.CreateAsync(user, newUser.Password);
            if (result.Succeeded)
                return CreatedAtAction(
            actionName: nameof(UsersController.GetUser),
            controllerName: "Users",  // The name of the controller without the "Controller" suffix
            routeValues: new { id = user.Id },
            value: newUser
        );

            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }
    }
}