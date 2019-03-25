using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MedCare.Data;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;

namespace MedCare.Models.Database
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }

        public Boolean IsAdmin { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public string SpecialistIn { get; set; }

        public string Description { get; set; }

        public List<DoctorPatient> Patients { get; set; }
    }
}