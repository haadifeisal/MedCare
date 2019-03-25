using MedCare.Models.Database;
using MedCare.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedCare.Data.Repositories.Interface
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllDoctors();
        Task<Doctor> GetDoctorByID(int id);
        Task<Doctor> GetDoctorByUsername(string username);
        Task<Boolean> IsAdmin(string username);
        Task<Boolean> CheckIfDoctorExistsById(int id);
        Task<Boolean> CheckIfDoctorExistsByEmail(string email);
        Task<Boolean> CheckIfDoctorExistsByUsername(string username);
        Task<Boolean> DeleteDoctor(int id);
        Task<Boolean> EditDoctor(int id, EditDoctorDTO editDto);
        Task<Doctor> GetDoctorByEmail(string email);
        Task<List<Doctor>> GetSpecificDoctors(string username);
        Task<Boolean> CheckIfDoctorAndPatientMatchId(int patientId, int doctorId);
    }
}
