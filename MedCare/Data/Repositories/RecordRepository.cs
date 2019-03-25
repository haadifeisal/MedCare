using MedCare.Data.Repositories.Interface;
using MedCare.Models.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Data.Repositories
{
    public class RecordRepository : IRecordRepository
    {

        public readonly DataContext _context;

        public RecordRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Boolean> CreateRecord(Record record)
        {
            if (record != null)
            {
                await _context.AddAsync(record);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<IEnumerable<Record>> GetRecordsFromPatient(int id)
        {
            var records = await _context.Records.Where(x => x.PatientId == id).ToListAsync();
            if (records == null)
            {
                return null;
            }
            return records;
        }

        public async Task<Record> GetRecordForPatient(int patientId, int id)
        {
            var record = await _context.Records.FirstOrDefaultAsync(x => x.Id == id && x.PatientId == patientId);
            if (record == null)
            {
                return null;
            }
            return record;
        }

        public async Task<IEnumerable<Record>> GetRecordsFromDoctor(int id)
        {
            var records = await _context.Records.Where(x => x.DoctorId == id).ToListAsync();
            if (records == null)
            {
                return null;
            }
            return records;
        }

        public async Task<Record> GetRecordForDoctor(int doctorId, int id)
        {
            var record = await _context.Records.FirstOrDefaultAsync(x => x.Id == id && x.DoctorId==doctorId);
            if (record == null)
            {
                return null;
            }
            return record;
        }

        public async Task<bool> CheckIfDoctorIdExistsInDoctorPatients(int patientId, int doctorId)
        {
            var doctorPatients = await _context.DoctorPatients.FirstOrDefaultAsync(x => x.DoctorId == doctorId && x.PatientId == patientId);
            if (doctorPatients == null)
            {
                return false;
            }
            return true;
        }
    }
}
