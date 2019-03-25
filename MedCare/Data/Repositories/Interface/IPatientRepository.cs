using MedCare.Models.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedCare.Data.Repositories.Interface
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllPatients();
        Task<List<Patient>> GetSpecificPatients(string username);
        Task<Patient> GetPatientByID(int id);
        Task<Patient> GetPatientByEmail(string email);
        Task<Patient> GetPatientByUsername(string username);
        Task<Boolean> CheckIfPatientExistsById(int id);
        Task<Boolean> CheckIfPatientExistsByUsername(string username);
        Task<Boolean> CheckIfPatientExistsByEmail(string email);
        Task<Boolean> DeletePatient(int id);
    }
}
