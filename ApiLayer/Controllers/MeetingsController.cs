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
        public IActionResult CreateMeeting([FromForm] Meeting meeting, IFormFile meetingDocument)
        {
            if (meeting == null)
            {
                return BadRequest("Toplantı bilgileri geçersiz.");
            }

            if (meetingDocument != null && meetingDocument.Length > 0)
            {
                // API katmanındaki dizin
                var apiFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", meetingDocument.FileName);

                // Dosya yükleme API katmanında
                using (var stream = new FileStream(apiFilePath, FileMode.Create))
                {
                    meetingDocument.CopyTo(stream);
                }

                // Sunum katmanındaki dizin
                var presentationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "PresentationLayer", "wwwroot", "uploads", meetingDocument.FileName);

                // Dosya kopyalama
                System.IO.File.Copy(apiFilePath, presentationFilePath, overwrite: true);


                // Toplantı nesnesine dosya yolunu ekleyin
                meeting.DocumentPath = $"/uploads/{meetingDocument.FileName}";
            }

            // Toplantıyı veritabanına kaydedin
            var createdMeeting = _meetingService.CreateMeeting(meeting);

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
        public async Task<IActionResult> UpdateMeeting(int id, [FromForm] UpdateMeetingDto updatedMeetingDto, [FromForm] IFormFile updateDocument)
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

            if (updateDocument != null && updateDocument.Length > 0)
            {
                // API katmanındaki dizin
                var apiUploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(apiUploadsFolder); // Ensure directory exists

                var apiFilePath = Path.Combine(apiUploadsFolder, updateDocument.FileName);

                // Dosya yükleme API katmanında
                using (var stream = new FileStream(apiFilePath, FileMode.Create))
                {
                    await updateDocument.CopyToAsync(stream);
                }

                // Sunum katmanındaki dizin
                var presentationUploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "..", "PresentationLayer", "wwwroot", "uploads");
                Directory.CreateDirectory(presentationUploadsFolder); // Ensure directory exists

                var presentationFilePath = Path.Combine(presentationUploadsFolder, updateDocument.FileName);

                // Dosya kopyalama
                System.IO.File.Copy(apiFilePath, presentationFilePath, overwrite: true);

                // Toplantı nesnesine dosya yolunu ekleyin
                existingMeeting.DocumentPath = $"/uploads/{updateDocument.FileName}";
            }
            else
            {
                // Dosya yüklenmemişse, mevcut döküman yolunu koruyun
                existingMeeting.DocumentPath = existingMeeting.DocumentPath;
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
