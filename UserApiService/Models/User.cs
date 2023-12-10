using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UserApiService.Models
{
    //* Custom class inherited for Identity user to add some extra attributes
    public class User : IdentityUser
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? StreetName { get; set; }
        public string? StreetNumber { get; set; }
    }
}