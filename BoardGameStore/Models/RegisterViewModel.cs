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
        [MinLength(5, ErrorMessage = "Not long enough")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
