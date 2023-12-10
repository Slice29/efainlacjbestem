using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApiService.Models;
using ValidationLib;


//* Class used to generate JWT using ValidationLib class
namespace UserApiService.JWT;
public class JwtHelper
{
    private readonly TokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public JwtHelper(TokenService tokenService, UserManager<User> userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

   public async Task<string> GenerateTokenAsync(User user)
{
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Email)
        // Add other claims as needed
    };

    if (await _userManager.IsInRoleAsync(user, "Admin"))
    {
        claims.Add(new Claim("admin", "true"));
    }
     if (await _userManager.IsInRoleAsync(user, "PromoUser"))
    {
        claims.Add(new Claim("promouser", "true"));
    }
    var token = _tokenService.GenerateToken(claims);

    return token;
}
}
