using AutoMapper;
using MedCare.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCare.Models.DTO
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<DoctorForRegisterDTO, Doctor>();
            CreateMap<Doctor, DoctorForRegisterDTO>();
            CreateMap<Doctor, GetDoctorDTO>();
            CreateMap<Patient, CreatePatientDTO>();
            CreateMap<CreatePatientDTO, Patient>();
            CreateMap<EditDoctorDTO, Doctor>();
            CreateMap<Doctor, EditDoctorDTO>();
            CreateMap<CreateRecordDTO, Record>();
            CreateMap<Record, CreateRecordDTO>();
            CreateMap<SendMessageDTO, Message>();
            CreateMap<Message, GetMessageDTO>();
            CreateMap<Patient, PatientForAdminDTO>();
            CreateMap<Record, GetRecordDTO>();
        }
    }
}
