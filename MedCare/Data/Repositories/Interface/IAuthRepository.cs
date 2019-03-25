using MedCare.Models.Database;
using System.Threading.Tasks;

namespace MedCare.Data.Repositories.Interface
{
    public interface IAuthRepository
    {
        Task<Doctor> DoctorLogin(string username, string password);
        Task<Patient> PatientLogin(string username, string password);
        Task<Doctor> RegisterDoctor(string username, Doctor doctor);
        Task<Patient> RegisterPatient(int id, string password, Patient patient);
        Task<bool> CheckIfDoctorExists(string username, string email);
        Task<bool> CheckIfPatientExists(string username, string email);
        Task<bool> AddExistingPatient(int doctorId, string username);
    }
}
