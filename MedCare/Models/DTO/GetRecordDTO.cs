using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.DTO
{
    public class GetRecordDTO
    {
        public int Id { get; set; }
        [Required]
        public int PatientId { get; set; }
        [Required]
        public int Pulse { get; set; }
        [Required]
        public int StressLevel { get; set; }
        [Required]
        public int OxygenLevel { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
