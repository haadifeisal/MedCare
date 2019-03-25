using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedCare.Data.Repositories.Interface;
using MedCare.Models.Database;
using MedCare.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MedCare.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IAuthRepository auth_repo;
        private readonly IDoctorRepository _docRepo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController(IAuthRepository repo, IMapper mapper, IConfiguration config, IDoctorRepository docRepo)
        {
            auth_repo = repo;
            _mapper = mapper;
            _config = config;
            _docRepo = docRepo;
        }

        [HttpPost("doctorlogin")]
        public async Task<IActionResult> DoctorLogin([FromBody] UserForLoginDTO userForLoginDto)
        {
            var user = await auth_repo.DoctorLogin(userForLoginDto.Username, userForLoginDto.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler(); //Handle our token
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new { tokenString });
        }

        [HttpPost("patientlogin")]
        public async Task<IActionResult> PatientLogin([FromBody] UserForLoginDTO userForLoginDto)
        {
            var user = await auth_repo.PatientLogin(userForLoginDto.Username, userForLoginDto.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler(); //Handle our token
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new { tokenString });
        }

        [HttpPost("registerPatient")]
        public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDTO createPatientDTO)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value; //logged in user Username
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id
            var doc = await _docRepo.GetDoctorByUsername(loggedInUser);

            if (doc == null || doc.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }

            if (!ModelState.IsValid)
            { //check if theres any errors in createPatientDTO. for ex. Required errors
                return BadRequest(ModelState);
            }

            var mappedPatient = _mapper.Map<Patient>(createPatientDTO);
            var user = await auth_repo.RegisterPatient(loggedInUserId, createPatientDTO.Password, mappedPatient);

            if (user == null)
            {
                return NotFound(new { message = "Patient username already exists" });
            }

            return Ok(user.Username + " registered");
        }

        [HttpPost("addExistingPatient")]
        public async Task<IActionResult> AddExistingPatient([FromBody] ExistingPatientDTO eDTO)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value; //logged in user Username
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id
            var doc = await _docRepo.GetDoctorByUsername(loggedInUser);

            if (doc == null || doc.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            var addPatient = await auth_repo.AddExistingPatient(doc.Id, eDTO.username);
            if (!addPatient)
            {
                return BadRequest(new { message = "Failed to add patient" });
            }
            return Ok(new { succesful = "Patient added" });
        }

        [HttpPost("registerDoctor")]
        public async Task<IActionResult> CreateDoctor([FromBody] DoctorForRegisterDTO docDto)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var doc = await _docRepo.GetDoctorByUsername(loggedInUser);
            if (doc == null || !doc.IsAdmin)
            {
                return Json(new { url = "http://localhost:4200" });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mappedDoctor = _mapper.Map<Doctor>(docDto);
            var user = await auth_repo.RegisterDoctor(docDto.Password, mappedDoctor);
            if (user == null)
            {
                return BadRequest("Doctor username already exists");
            }
            return Ok(user.Username + " registered");
        }

    }
}