using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MedCare.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MedCare.Data.Repositories.Interface;

namespace MedCare.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DoctorController : Controller
    {

        private readonly IDoctorRepository _docRepo;
        private readonly IPatientRepository _patRepo;
        private readonly IRecordRepository _recRepo;
        private readonly IMapper _mapper;

        public DoctorController(IMapper mapper, IDoctorRepository docRepo, IPatientRepository patRepo
            , IRecordRepository recRepo)
        {
            _mapper = mapper;
            _docRepo = docRepo;
            _patRepo = patRepo;
            _recRepo = recRepo;
        }

        [HttpGet("getAllDoctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value; // Get current logged in User's "Username" from jason web token
            var user = await _docRepo.GetDoctorByUsername(loggedInUser); // check if theres a doctor with the same username
            if (user == null || !user.IsAdmin) // if not, return Unauthorized, because this is a patient that is trying to access this page.
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var doctors = await _docRepo.GetAllDoctors();
            if (doctors == null)
            {
                return NotFound("No doctors found!");
            }
            // Maps doctor-object list to getDoctorDTO object
            var mappedDoctors = _mapper.Map<IEnumerable<GetDoctorDTO>>(doctors);

            return Ok(mappedDoctors);
        }

        [HttpGet("getDoctor/{id}")]
        public async Task<IActionResult> GetDoctorByID(int id)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var user = await _docRepo.GetDoctorByUsername(loggedInUser);
            if (user == null || user.Id!=id&&!user.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var doctor = await _docRepo.GetDoctorByID(id);
            if (doctor == null)
            {
                return NotFound("Doctor not found!");
            }
            var mappedDoctor = _mapper.Map<GetDoctorDTO>(doctor);
            return Ok(mappedDoctor);
        }

        [HttpGet("getDoctorForPatient/{id}")]
        public async Task<IActionResult> GetDoctorByIDForPatient(int id)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _patRepo.GetPatientByUsername(loggedInUser);
            var check = await _recRepo.CheckIfDoctorIdExistsInDoctorPatients(loggedInUserId, id);
            if (user == null || !check)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var doctor = await _docRepo.GetDoctorByID(id);
            if (doctor == null)
            {
                return NotFound("Doctor not found!");
            }
            var mappedDoctor = _mapper.Map<GetDoctorDTO>(doctor);
            return Ok(mappedDoctor);
        }

        [HttpGet("getDoctorByUsername/{username}")]
        public async Task<IActionResult> GetDoctorByUsername(string username)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _patRepo.GetPatientByUsername(loggedInUser);
            if (user == null)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var doctor = await _docRepo.GetDoctorByUsername(username);
            if (doctor == null)
            {
                return NotFound("Doctor not found!");
            }
            var check = await _docRepo.CheckIfDoctorAndPatientMatchId(loggedInUserId, doctor.Id);
            if (!check)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var mappedDoctor = _mapper.Map<GetDoctorDTO>(doctor);
            return Ok(mappedDoctor);

        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("deleteDoctor/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); 
            var user = await _docRepo.GetDoctorByUsername(loggedInUser);
            if (user == null || !user.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            if (await _docRepo.DeleteDoctor(id))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("getAllPatients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); 
            var user = await _docRepo.GetDoctorByUsername(loggedInUser);
            if (user == null || !user.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var patients = await _patRepo.GetAllPatients();
            var mappedPatients = _mapper.Map<IEnumerable<PatientForAdminDTO>>(patients);
            if (patients != null)
            {
                return Ok(mappedPatients);
            }
            return NotFound(new { message = "No patients in the system" });

        }

        [HttpDelete("deletePatient/{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id
            var user = await _docRepo.GetDoctorByUsername(loggedInUser);
            if (user == null || !user.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            if (await _patRepo.DeletePatient(id))
            {
                return Ok(new { message="Patient Deleted"});
            }
            else
            {
                return NotFound(new { message = "Patient could not be deleted" });
            }
        }

        // GET: api/Doctor
        [HttpGet("isAdmin/{username}")]
        public async Task<IActionResult> IsAdmin(string username)
        {
            var doctor = await _docRepo.IsAdmin(username);
            if (doctor)
            {
                return Ok(true);
            }
            return Json(new { url = "http://localhost:4200" });
        }

        [HttpGet("checkDoctor/{username}")]
        public async Task<IActionResult> CheckIfDoctorExists(string username)
        {
            var doctor = await _docRepo.GetDoctorByUsername(username);
            if (doctor == null || doctor.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            else
            {
                return Ok(true);
            }
        }
        // PUT: api/Doctor/5
        [HttpPut("editDoctor/{id}")]
        public async Task<IActionResult> EditDoctor(int id, [FromBody] EditDoctorDTO editDto)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id
            var user = await _docRepo.GetDoctorByUsername(loggedInUser);
            if (user == null || user.IsAdmin || loggedInUserId != id)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _docRepo.EditDoctor(loggedInUserId, editDto))
            {
                return Ok("Doctor update successful");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("doctorRecords")]
        public async Task<IActionResult> GetAllRecordsForDoctor()
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id
            var user = await _docRepo.GetDoctorByID(loggedInUserId);
            if (user == null || user.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var records = await _recRepo.GetRecordsFromDoctor(loggedInUserId);
            var mappedRecord = _mapper.Map<IEnumerable<GetRecordDTO>>(records);
            return Ok(mappedRecord);
        }

        [HttpGet("doctorRecord/{id}")]
        public async Task<IActionResult> GetRecordForDoctor(int id)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id
            var user = await _docRepo.GetDoctorByID(loggedInUserId);
            if (user == null)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var record = await _recRepo.GetRecordForDoctor(loggedInUserId, id);
            if (record == null)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var mappedRecord = _mapper.Map<GetRecordDTO>(record);
            return Ok(mappedRecord);
        }

        [HttpGet("getSpecificDoctors")]
        public async Task<IActionResult> GetSpecificDoctors()
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var user = await _patRepo.GetPatientByUsername(loggedInUser);

            if (user == null)
            {
                return Json(new { url = "http://localhost:4200" });
            }

            var doctors = await _docRepo.GetSpecificDoctors(loggedInUser);

            if (doctors== null)
            {
                return NotFound("No patients found!");
            }

            var mappedDoctors = _mapper.Map<IEnumerable<GetDoctorDTO>>(doctors);
            return Ok(mappedDoctors);
        }

    }
}
