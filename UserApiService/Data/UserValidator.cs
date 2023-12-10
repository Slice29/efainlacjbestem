using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UserApiService.Models;

namespace UserApiService.Data
{
    public class UserValidator : UserValidator<User>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            return IdentityResult.Success;
        }
    }
}