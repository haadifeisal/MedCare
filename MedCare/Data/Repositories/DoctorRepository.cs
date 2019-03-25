using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MedCare.Data.Repositories.Interface;
using MedCare.Models.Database;
using MedCare.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace MedCare.Data.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DataContext _context;

        public DoctorRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            var doctors = await _context.Doctors.ToListAsync();

            return doctors;
        }

        public async Task<Doctor> GetDoctorByID(int id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                return null;
            }
            return doctor;
        }

        public async Task<Doctor> GetDoctorByUsername(string username)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Username.ToUpper() == username.ToUpper());
            if (doctor == null)
            {
                return null;
            }
            return doctor;
        }

        public async Task<Boolean> IsAdmin(string username)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Username == username);
            if (doctor == null || !doctor.IsAdmin)
            {
                return false;
            }
            return true;
        }

        public async Task<Boolean> CheckIfDoctorExistsById(int id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Boolean> CheckIfDoctorExistsByEmail(string email)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Email.ToUpper() == email.ToUpper());
            if (doctor == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Boolean> CheckIfDoctorExistsByUsername(string username)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Username.ToUpper() == username.ToUpper());
            if (doctor == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Boolean> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == id);
            if (doctor == null)
            {
                return false;
            }
            var doctorpatients = await _context.DoctorPatients.Where(i => i.DoctorId == doctor.Id).ToListAsync();
            if (doctorpatients == null)
            {
                return false;
            }
            //List<Doctor> doctors = new List<Doctor>();
            foreach (var d in doctorpatients)
            {
                var temp = await GetDoctorByID(d.DoctorId);
                //doctors.Add(temp);
                _context.DoctorPatients.Remove(d);
            }
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Boolean> EditDoctor(int id, EditDoctorDTO editDto)
        {
            var doc = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == id);
            if (editDto.Password != null)
            {
                byte[] PasswordHash, PasswordSalt;
                HashAndSaltPassword(editDto.Password, out PasswordHash, out PasswordSalt);
                editDto.PasswordHash = PasswordHash;
                editDto.PasswordSalt = PasswordSalt;
            }
            if (doc != null)
            {
                _context.Entry(doc).CurrentValues.SetValues(editDto);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        private void HashAndSaltPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<Doctor> GetDoctorByEmail(string email)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Email.ToUpper() == email.ToUpper());

            return doctor;
        }

        public async Task<List<Doctor>> GetSpecificDoctors(string username)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Username.ToUpper() == username.ToUpper());
            if (patient == null)
            {
                return null;
            }
            var doctorpatients = await _context.DoctorPatients.Where(i => i.PatientId == patient.Id).ToListAsync();
            if (doctorpatients == null)
            {
                return null;
            }

            List<Doctor> doctors = new List<Doctor>();

            foreach (var p in doctorpatients)
            {
                var tempDoctor = await GetDoctorByID(p.DoctorId);
                doctors.Add(tempDoctor);
            }
            return doctors;
        }

        public async Task<Boolean> CheckIfDoctorAndPatientMatchId(int patientId, int doctorId)
        {
            var doctorpatients = await _context.DoctorPatients.FirstOrDefaultAsync(i => i.DoctorId==doctorId && i.PatientId==patientId);
            if (doctorpatients == null)
            {
                return false;
            }
            return true;
        }

    }
}