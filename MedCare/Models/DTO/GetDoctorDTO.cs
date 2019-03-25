using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.DTO
{
    public class GetDoctorDTO
    {
        public int Id { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
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
        public string SpecialistIn { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
