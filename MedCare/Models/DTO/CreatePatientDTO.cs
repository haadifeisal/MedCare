using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.DTO
{
    public class CreatePatientDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Username should be between 3 and 15 characters")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Password should be between 7 and 20 characters")]
        public string Password { get; set; }

        public string SocialNumber { get; set; }
    }
}
