using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedCare.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MedCare.Data.Repositories.Interface;
using System;
using MedCare.Models.Database;

namespace MedCare.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PatientController : Controller
    {
        private readonly IAuthRepository _authRepo;
        private readonly IPatientRepository _patRepo;
        private readonly IDoctorRepository _docRepo;
        private readonly IRecordRepository _recRepo;
        private readonly IMapper _mapper;

        public PatientController(IAuthRepository repo, IMapper mapper, IPatientRepository patientRepo,
            IDoctorRepository docRepo, IRecordRepository recRepo)
        {
            _authRepo = repo;
            _mapper = mapper;
            _patRepo = patientRepo;
            _docRepo = docRepo;
            _recRepo = recRepo;
        }

        [HttpGet("getAllPatients")]
        public async Task<IActionResult> GetPatients()
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var doctorFromRepo = await _docRepo.GetDoctorByUsername(loggedInUser);
            if (doctorFromRepo == null)
            {
                return Unauthorized();
            }
            var patients = await _patRepo.GetAllPatients();
            if (patients == null)
            {
                return NotFound("No patients found!");
            }

            // Maps doctor-object list to getDoctorDTO object
            var mappedPatients = _mapper.Map<IEnumerable<GetPatientDTO>>(patients);

            return Ok(mappedPatients);
        }

        [HttpGet("getSpecificPatients")]
        public async Task<IActionResult> GetSpecificPatients()
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var user = await _docRepo.GetDoctorByUsername(loggedInUser);

            if (user == null || user.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }

            var patients = await _patRepo.GetSpecificPatients(loggedInUser);

            if (patients == null)
            {
                return NotFound("No patients found!");
            }

            var mappedPatients = _mapper.Map<IEnumerable<GetPatientDTO>>(patients);
            return Ok(mappedPatients);
        }

        [HttpGet("getPatient/{id}")]
        public async Task<IActionResult> GetPatientByID(int id)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var doctorFromRepo = await _docRepo.GetDoctorByUsername(loggedInUser);
            var patient = await _patRepo.GetPatientByID(id);
            if (doctorFromRepo == null)
            {
                return Unauthorized();
            }
            if (patient == null)
            {
                return NotFound(new { message = "Doctor not found!" });
            }
            // Maps doctor-object list to getDoctorDTO object
            var mappedPatient = _mapper.Map<GetPatientDTO>(patient);

            return Ok(mappedPatient);
        }

        [HttpGet("getPatientByUsername/{username}")]
        public async Task<IActionResult> GetPatientByUsername(string username)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var patient = await _patRepo.GetPatientByUsername(username);
            if (patient == null)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            // Maps doctor-object list to getDoctorDTO object
            var mappedPatient = _mapper.Map<GetPatientDTO>(patient);

            return Ok(mappedPatient);
        }

        [HttpPost("createRecord")]
        public async Task<IActionResult> CreateRecord([FromBody] CreateRecordDTO recordDto)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id
            var user = await _patRepo.GetPatientByID(loggedInUserId);
            var doctor = await _docRepo.GetDoctorByID(recordDto.DoctorId);
            var doctorPatient = await _recRepo.CheckIfDoctorIdExistsInDoctorPatients(loggedInUserId, recordDto.DoctorId);
            if (user == null || doctor == null || !doctorPatient)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var mappedRecord = _mapper.Map<Record>(recordDto);
            mappedRecord.PatientId = loggedInUserId;
            mappedRecord.DateCreated = DateTime.Now;
            if (await _recRepo.CreateRecord(mappedRecord))
            {
                return Ok("Record created");
            }
            else
            {
                return NotFound(new { message = "Failed to create Record" });
            }

        }

        [HttpGet("patientRecords")]
        public async Task<IActionResult> GetAllRecordsForPatient()
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id
            var user = await _patRepo.GetPatientByID(loggedInUserId);
            if (user == null)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var records = await _recRepo.GetRecordsFromPatient(loggedInUserId);
            var mappedRecord = _mapper.Map<IEnumerable<CreateRecordDTO>>(records);
            return Ok(mappedRecord);
        }

        [HttpGet("patientRecord/{id}")]
        public async Task<IActionResult> GetRecordForPatient(int id)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id
            var user = await _patRepo.GetPatientByID(loggedInUserId);
            if (user == null)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var record = await _recRepo.GetRecordForPatient(loggedInUserId, id);
            if (record == null)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var mappedRecord = _mapper.Map<CreateRecordDTO>(record);
            return Ok(mappedRecord);
        }

        [HttpGet("checkPatient/{username}")]
        public async Task<IActionResult> CheckIfPatientExists(string username)
        {
            var patient = await _patRepo.GetPatientByUsername(username);
            if (patient == null)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            else
            {
                return Ok(true);
            }
        }

    }
}
