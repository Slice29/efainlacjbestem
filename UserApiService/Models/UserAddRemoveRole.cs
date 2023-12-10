using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserApiService.Models
{
     public class UserAddRemoveRole
    {
        public string RoleName { get; set; } = null!;
    }
}