using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameStore.Models
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(5, ErrorMessage = "Username isn't long enough")]
        public string UserName { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "Password should be at least 8 characters long")]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
