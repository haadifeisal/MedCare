using MedCare.Models.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedCare.Data.Repositories.Interface
{
    public interface IRecordRepository
    {
        Task<Boolean> CreateRecord(Record record);
        Task<IEnumerable<Record>> GetRecordsFromPatient(int id);
        Task<Record> GetRecordForPatient(int patientId, int id);
        Task<IEnumerable<Record>> GetRecordsFromDoctor(int id);
        Task<Record> GetRecordForDoctor(int doctorId, int id);
        Task<Boolean> CheckIfDoctorIdExistsInDoctorPatients(int patientId, int doctorId);
    }
}
