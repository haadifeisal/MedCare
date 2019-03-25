using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MedCare.Data.Repositories.Interface;
using MedCare.Models.Database;
using MedCare.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedCare.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMessageRepository _msgRepo;
        private readonly IDoctorRepository _docRepo;
        private readonly IPatientRepository _patRepo;

        public MessageController(IMapper mapper, IMessageRepository msgRepo, IDoctorRepository docRepo, IPatientRepository patRepo)
        {
            _mapper = mapper;
            _msgRepo = msgRepo;
            _docRepo = docRepo;
            _patRepo = patRepo;
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO msgDTO)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            int loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id

            //Check if logged in user is a patient
            if (await _patRepo.CheckIfPatientExistsByUsername(loggedInUser))
            {
                //Check if doctor recipient exists
                if (await _docRepo.CheckIfDoctorExistsByEmail(msgDTO.To))
                {
                    var doctor = await _docRepo.GetDoctorByEmail(msgDTO.To);

                    Message message = new Message
                    {
                        To = doctor.Email,
                        Subject = msgDTO.Subject,
                        Content = msgDTO.Content,
                        From = loggedInUser,
                        Read = false,
                        TimeSent = DateTime.Now
                    };

                    if (await _msgRepo.SendMessage(message))
                    {
                        return Ok("Message has been sent");
                    }
                    else
                    {
                        return NotFound(new { message = "Failed to send message" });
                    }
                }
                else
                {
                    return BadRequest("A doctor with that email doesn't exist!");
                }
            }
            //Check if user is a doctor
            else if (await _docRepo.CheckIfDoctorExistsByUsername(loggedInUser))
            {
                var doctor = await _docRepo.GetDoctorByID(loggedInUserId);
                //Check if logged in user is an admin
                if (!doctor.IsAdmin)
                {
                    //Check if patient recipient exists
                    if (await _patRepo.CheckIfPatientExistsByEmail(msgDTO.To))
                    {
                        var patient = await _patRepo.GetPatientByEmail(msgDTO.To);

                        Message message = new Message
                        {
                            To = patient.Email,
                            Subject = msgDTO.Subject,
                            Content = msgDTO.Content,
                            From = doctor.Email,
                            Read = false,
                            TimeSent = DateTime.Now
                        };

                        if (await _msgRepo.SendMessage(message))
                        {
                            return Ok("Message has been sent");
                        }
                        else
                        {
                            return NotFound(new { message = "Failed to send message" });
                        }
                    }
                    else
                    {
                        return BadRequest("A doctor with that email doesn't exist!");
                    }
                }
                else
                {
                    return Json(new { url = "http://localhost:4200" });
                }
            }
            else
            {
                return Json(new { url = "http://localhost:4200" });
            }
        }

        [HttpGet("getReceivedMessages")]
        public async Task<IActionResult> GetReceivedMessages()
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); //logged in user Id

            var patient = await _patRepo.GetPatientByUsername(loggedInUser);
            var doctor = await _docRepo.GetDoctorByUsername(loggedInUser);
            //Check if logged in user is a patient
            if (patient!=null)
            {
                var messages = await _msgRepo.GetReceivedMessages(patient.Email);
                var mappedMessages = _mapper.Map<IEnumerable<GetMessageDTO>>(messages);
                return Ok(mappedMessages);
            }
            //Check if logged in user is a doctor
            if (doctor!=null)
            {
                var messages = await _msgRepo.GetReceivedMessages(doctor.Email);
                var mappedMessages = _mapper.Map<IEnumerable<GetMessageDTO>>(messages);
                return Ok(mappedMessages);
            }
            else
            {
                return Json(new { url = "http://localhost:4200" });
            }
        }

        [HttpGet("getMessage/{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value;
            var user = await _docRepo.GetDoctorByUsername(loggedInUser);
            var patient = await _patRepo.GetPatientByUsername(loggedInUser);
            var doctor = await _docRepo.GetDoctorByUsername(loggedInUser);
            //Check if logged in user is a patient
            if (patient != null)
            {
                var message = await _msgRepo.GetMessage(id, patient.Email);
                if (message == null)
                {
                    return NotFound(new { message = "Message not found" });
                }
                var mappedMessage = _mapper.Map<GetMessageDTO>(message);
                return Ok(mappedMessage);
            }
            //Check if logged in user is a doctor
            if (doctor != null && !doctor.IsAdmin)
            {
                var message = await _msgRepo.GetMessage(id, doctor.Email);
                if (message == null)
                {
                    return NotFound(new { message = "Message not found" });
                }
                var mappedMessage = _mapper.Map<GetMessageDTO>(message);
                return Ok(mappedMessage);
            }
            else
            {
                return Json(new { url = "http://localhost:4200" });
            }
        }

        [HttpDelete("deleteMessage/{id}")]
        public async Task<IActionResult> deleteMessage(int id)
        {
            var loggedInUser = User.FindFirst(ClaimTypes.Name).Value; 
            var user = await _docRepo.GetDoctorByUsername(loggedInUser);
            var patient = await _patRepo.GetPatientByUsername(loggedInUser);
            var doctor = await _docRepo.GetDoctorByUsername(loggedInUser);
            //Check if logged in user is a patient
            if (patient != null)
            {
                var messages = await _msgRepo.GetReceivedMessages(patient.Email);
                foreach(var m in messages)
                {
                    if (m.Id == id)
                    {
                        var delete = await _msgRepo.DeleteMessage(id);
                        return Ok("Message deleted");
                    }
                }
                return NotFound(new { message = "Failed To Delete Message" });
            }
            //Check if logged in user is a doctor
            if (doctor != null)
            {
                var messages = await _msgRepo.GetReceivedMessages(doctor.Email);
                foreach (var m in messages)
                {
                    if (m.Id == id)
                    {
                        var delete = await _msgRepo.DeleteMessage(id);
                        return Ok("Message deleted");
                    }
                }
                return NotFound(new { message = "Failed To Delete Message" });
            }
            else
            {
                return Json(new { url = "http://localhost:4200" });
            }
        }

        /*private async Task<List<GetMessageDTO>> MessageMapperForPatient(IEnumerable<Message> messages)
        {
            List<GetMessageDTO> mappedMessages = new List<GetMessageDTO>();
            foreach (var message in messages)
            {
                var fromDoctor = await _docRepo.GetDoctorByID(message.From);

                GetMessageDTO getMessage = new GetMessageDTO
                {
                    Content = message.Content,
                    From = fromDoctor.Email,
                    Read = message.Read,
                    Subject = message.Subject,
                    TimeSent = String.Format("{0:HH':'mm':'ss ddd, dd'-'MMM'-'yyyy}", message.TimeSent)
                };

                mappedMessages.Add(getMessage);
            }

            return mappedMessages;
        }

        private async Task<List<GetMessageDTO>> MessageMapperForDoctor(IEnumerable<Message> messages)
        {
            List<GetMessageDTO> mappedMessages = new List<GetMessageDTO>();
            foreach (var message in messages)
            {
                var fromPatient = await _patRepo.GetPatientByID(message.From);

                GetMessageDTO getMessage = new GetMessageDTO
                {
                    Content = message.Content,
                    From = fromPatient.Email,
                    Read = message.Read,
                    Subject = message.Subject,
                    TimeSent = String.Format("{0:HH':'mm':'ss ddd, dd MMM yyyy}", message.TimeSent)
                };

                mappedMessages.Add(getMessage);
            }

            return mappedMessages;
        }*/
    }
}
