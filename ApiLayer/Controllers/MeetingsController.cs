using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EntitiesLayer.Models;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO; 

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
        public IActionResult CreateMeeting([FromForm] Meeting meeting, IFormFile documentPath)
        {
            if (meeting == null)
            {
                return BadRequest("Toplantı bilgileri geçersiz.");
            }

            if (documentPath != null && documentPath.Length > 0)
            {
                // API katmanındaki dizin
                var apiFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", documentPath.FileName);

                // Dosya yükleme API katmanında
                using (var stream = new FileStream(apiFilePath, FileMode.Create))
                {
                    documentPath.CopyTo(stream);
                }

                // Sunum katmanındaki dizin
                var presentationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "PresentationLayer", "wwwroot", "uploads", documentPath.FileName);

                // Dosya kopyalama
                System.IO.File.Copy(apiFilePath, presentationFilePath, overwrite: true);

                
                // Toplantı nesnesine dosya yolunu ekleyin
                meeting.DocumentPath = $"/uploads/{documentPath.FileName}";
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
        public IActionResult UpdateMeeting(int id, [FromForm] Meeting meeting)
        {
            if (meeting == null || meeting.Id != id)
            {
                return BadRequest();
            }

            var existingMeeting = _meetingService.GetMeetingById(id);
            if (existingMeeting == null)
            {
                return NotFound();
            }

            _meetingService.UpdateMeeting(id, meeting);
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

        // GET: api/meetings/{id}/report
        [HttpGet("{id}/report")]
        public IActionResult GetMeetingReport(int id)
        {
            var report = new byte[0]; // Rapor verisini buradan alın
            return File(report, "application/pdf", "meeting-report.pdf");
        }
    }
}
