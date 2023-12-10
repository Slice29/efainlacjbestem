using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserApiService.Models;
using UserApiService.JWT;
using UserApiService.MessagePublishers;
using ValidationLib;

namespace UserApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        public LoginController (
            SignInManager<User> signInManager,
             UserManager<User> userManager,
             TokenService tokenService)
        {
            _signInManager = signInManager;

            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(UserLogin userLogin)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userLogin.Email,
                userLogin.Password,
                userLogin.RememberMe,
                false
            );
            
            if(result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(userLogin.Email);
                var JwtHelper = new JwtHelper(_tokenService, _userManager);
                var token = await JwtHelper.GenerateTokenAsync(user);

                //? Deprecated
                //var products = await _publisher.GetAllProducts(token);
                
                return Ok(token);
            }
            else return NotFound();
        }
    }
}