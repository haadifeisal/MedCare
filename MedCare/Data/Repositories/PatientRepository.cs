using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedCare.Data.Repositories.Interface;
using MedCare.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace MedCare.Data.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DataContext _context;

        public PatientRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            var patients = await _context.Patients.ToListAsync();

            return patients;
        }

        public async Task<List<Patient>> GetSpecificPatients(string username)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Username.ToUpper() == username.ToUpper());
            if (doctor == null)
            {
                return null;
            }
            var doctorpatients = await _context.DoctorPatients.Where(i => i.DoctorId == doctor.Id).ToListAsync();
            if (doctorpatients == null)
            {
                return null;
            }

            List<Patient> patients = new List<Patient>();

            foreach (var p in doctorpatients)
            {
                var tempPatient = await GetPatientByID(p.PatientId);
                patients.Add(tempPatient);
            }
            return patients;
        }

        public async Task<Patient> GetPatientByID(int id)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (patient == null)
            {
                return null;
            }
            return patient;
        }

        public async Task<Patient> GetPatientByEmail(string email)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Email.ToUpper() == email.ToUpper());
            if (patient == null)
            {
                return null;
            }
            return patient;
        }

        public async Task<Patient> GetPatientByUsername(string username)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Username.ToUpper() == username.ToUpper());
            if (patient == null)
            {
                return null;
            }
            return patient;
        }

        public async Task<bool> CheckIfPatientExistsById(int id)
        {
            var user = await _context.Patients.Where(p => p.Id == id).AnyAsync();
            if (user)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckIfPatientExistsByUsername(string username)
        {
            var user = await _context.Patients.Where(p => p.Username.ToUpper() == username.ToUpper()).AnyAsync();
            if (user)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckIfPatientExistsByEmail(string email)
        {
            var user = await _context.Patients.Where(p => p.Email.ToUpper() == email.ToUpper()).AnyAsync();
            if (user)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeletePatient( int id)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == id);
            if (patient == null)
            {
                return false;
            }
            var doctorPatients = await _context.DoctorPatients.Where(x => x.PatientId == id).ToListAsync();
            if (doctorPatients == null)
            {
                return false;
            }
            foreach(var d in doctorPatients)
            {
                _context.DoctorPatients.Remove(d);
            }
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
