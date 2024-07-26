using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EntitiesLayer.Models;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using EntitiesLayer.DTOs.Meeting;
using BusinessLayer.Services;
using System.Security.Cryptography;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingsController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        // GET: api/meetings
        [HttpGet]
        public IActionResult GetMeetings()
        {
            var meetings = _meetingService.GetAllMeetings();
            return Ok(meetings);
        }

        // GET: api/meetings/{id}
        [HttpGet("{id}")]
        public IActionResult GetMeeting(int id)
        {
            var meeting = _meetingService.GetMeetingById(id);
            if (meeting == null)
            {
                return NotFound();
            }
            return Ok(meeting);
        }

        // POST: api/meetings
        [HttpPost]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Toplantı oluştur")]
        public async Task<IActionResult> CreateMeeting([FromForm] MeetingDto meetingDto, [FromForm] IFormFile meetingDocument)
        {
            if (meetingDto == null)
            {
                return BadRequest("Toplantı bilgileri geçersiz.");
            }
            string meetingDocumentPath = null;
            if (meetingDocument != null && meetingDocument.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads"); Directory.CreateDirectory(uploadsFolder);

                meetingDocumentPath= Path.Combine(uploadsFolder, meetingDocument.FileName);
                using (var stream = new FileStream(meetingDocumentPath, FileMode.Create))
                {
                    await meetingDocument.CopyToAsync(stream);
                }

            }
            
            var createdMeeting = new Meeting {
                Name = meetingDto.Namee,
                StartDate = meetingDto.StartDate,
                EndDate = meetingDto.EndDate,
                Description = meetingDto.Description
            };
            _meetingService.CreateMeeting(createdMeeting);
            

            if (createdMeeting == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Toplantı oluşturulamadı.");
            }

            return CreatedAtAction(nameof(GetMeeting), new { id = createdMeeting.Id }, createdMeeting);
        }

        // PUT: api/meetings/{id}
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Toplantıyı güncelleme")]
        public async Task<IActionResult> UpdateMeeting(int id, [FromForm] EditMeetingDto updatedMeetingDto, [FromForm] IFormFile updateDocument)
        {
            if (updatedMeetingDto == null)
            {
                return BadRequest("Güncellenmiş toplantı bilgileri eksik.");
            }

            var existingMeeting = _meetingService.GetMeetingById(id);
            if (existingMeeting == null)
            {
                return NotFound();
            }

            // DTO verilerini mevcut toplantıya kopyalayın
            existingMeeting.Name = updatedMeetingDto.Namee ?? existingMeeting.Name;
            existingMeeting.StartDate = updatedMeetingDto.StartDate;
            existingMeeting.EndDate = updatedMeetingDto.EndDate;
            existingMeeting.Description = updatedMeetingDto.Description ?? existingMeeting.Description;

            // UpdateMeeting method
            if (updateDocument != null && updateDocument.Length > 0)
            {
                // API katmanındaki dizin
                var apiUploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(apiUploadsFolder);

                var documentPath = Path.Combine(apiUploadsFolder, updateDocument.FileName);
                using (var stream = new FileStream(documentPath, FileMode.Create))
                {
                    await updateDocument.CopyToAsync(stream);
                }

                existingMeeting.DocumentPath = documentPath;
            }

            _meetingService.UpdateMeeting(id, existingMeeting);

            return NoContent();
        }




        // DELETE: api/meetings/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteMeeting(int id)
        {
            var existingMeeting = _meetingService.GetMeetingById(id);
            if (existingMeeting == null)
            {
                return NotFound();
            }

            _meetingService.DeleteMeeting(id);
            return NoContent();
        }

    }
}
