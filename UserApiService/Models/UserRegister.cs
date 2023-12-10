using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiService.Models
{
     public class UserRegister
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber {get; set;}
        public string? Country {get; set;}
        public string? City {get; set;}
        public string? StreetName {get; set;}
        public string? StreetNumber {get; set;}

    }
}