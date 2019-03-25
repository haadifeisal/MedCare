using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.Database
{
    public class Record
    {
        [Key]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int Pulse { get; set; }
        public int StressLevel { get; set; }
        public int OxygenLevel { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
