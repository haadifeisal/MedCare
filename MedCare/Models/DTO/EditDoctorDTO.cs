using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.DTO
{
    public class EditDoctorDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [StringLength(15, MinimumLength = 3, ErrorMessage = "Username should be between 3 and 15 characters")]
        public string Username { get; set; }

        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password should be between 5 and 20 characters")]
        public string Password { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string SpecialistIn { get; set; }

        public string Description { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}
