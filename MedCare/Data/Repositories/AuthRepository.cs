using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MedCare.Data.Repositories.Interface;
using MedCare.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace MedCare.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Doctor> DoctorLogin(string username, string password)
        {
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        public async Task<Patient> PatientLogin(string username, string password)
        {
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }
            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<Doctor> RegisterDoctor(string password, Doctor doctor)
        {
            if (await CheckIfDoctorExists(doctor.Username, doctor.Email))
            {
                return null;
            }
            byte[] PasswordHash, PasswordSalt;
            HashAndSaltPassword(password, out PasswordHash, out PasswordSalt);
            doctor.PasswordHash = PasswordHash;
            doctor.PasswordSalt = PasswordSalt;
            await _context.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return doctor;

        }

        public async Task<Patient> RegisterPatient(int doctorId, string password, Patient patient)
        {
            if (await CheckIfPatientExists(patient.Username, patient.Email))
            {
                return null;
            }
            byte[] PasswordHash, PasswordSalt;
            HashAndSaltPassword(password, out PasswordHash, out PasswordSalt);
            patient.PasswordHash = PasswordHash;
            patient.PasswordSalt = PasswordSalt;
            await _context.AddAsync(patient);
            await _context.SaveChangesAsync();

            DoctorPatient docPatient = new DoctorPatient();
            docPatient.DoctorId = doctorId;
            var prevPatient = await _context.Patients.Where(x => x.Username == patient.Username).FirstOrDefaultAsync();
            docPatient.PatientId = prevPatient.Id;

            await _context.AddAsync(docPatient);
            await _context.SaveChangesAsync();

            patient.Doctors.Add(docPatient);
            _context.Update(patient);
            await _context.SaveChangesAsync();

            return patient;

        }

        private void HashAndSaltPassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> CheckIfDoctorExists(string username, string email)
        {
            var user = await _context.Doctors.Where(x => x.Username == username || x.Email == email).AnyAsync();
            if (user)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckIfPatientExists(string username, string email)
        {
            var user = await _context.Patients.Where(x => x.Username == username || x.Email == email).AnyAsync();
            if (user)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddExistingPatient(int doctorId, string username)
        {
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Username == username);
            if (patient == null)
            {
                return false;
            }
            var docPat = await _context.DoctorPatients.FirstOrDefaultAsync(x => x.DoctorId == doctorId && x.PatientId == patient.Id);
            if (docPat != null)
            {
                return false;
            }
            var doctorPatient = new DoctorPatient
            {
                DoctorId = doctorId,
                PatientId = patient.Id
            };
            await _context.AddAsync(doctorPatient);
            await _context.SaveChangesAsync();
            return true;
        }

        //Task<Doctor> IAuthRepository.DoctorLogin(string username, string password)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<Patient> IAuthRepository.PatientLogin(string username, string password)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
