using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserApiService.Models;

namespace UserApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{id}/roles")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound(id);
            }
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPut("{id}/roles")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> PutUserRole(string id, UserAddRemoveRole param)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound(id);
            }
            try {
                await _userManager.AddToRoleAsync(user, param.RoleName);
            } catch(Exception e) {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpDelete("{id}/roles")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteUserRole(string id, UserAddRemoveRole param)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return NotFound(id);
            }
            try {
                await _userManager.RemoveFromRoleAsync(user, param.RoleName);
            } catch(Exception e) {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}