using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.DTO
{
    public class DoctorForRegisterDTO
    {
        [Required]
        public Boolean IsAdmin { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(20, MinimumLength =4, ErrorMessage ="Password must be between 4 and 20 characters long")]
        public string Password { get; set; }

        [Required]
        public string SpecialistIn { get; set; }

        [Required]
        public string Description { get; set; }
        
    }
}
