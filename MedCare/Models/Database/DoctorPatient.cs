using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.Database
{
    public class DoctorPatient
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
