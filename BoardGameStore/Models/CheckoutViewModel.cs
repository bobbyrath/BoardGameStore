using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameStore.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string ContactEmail { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string ShippingStreet { get; set; }

        [Required]
        public string ShippingCity { get; set; }

        [Required]
        public string ShippingState { get; set; }

        [Required]
        public string ShippingPostalCode { get; set; }

    }
}
