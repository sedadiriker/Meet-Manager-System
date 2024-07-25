using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EntitiesLayer.Models;
using System.Collections.Generic;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class MeetingsController : ControllerBase
    {
        // GET: api/meetings
        [HttpGet]
        public IActionResult GetMeetings()
        {
            // Toplantıları getirme kodu
            var meetings = new List<Meeting>(); // Örnek veri
            return Ok(meetings);
        }

        // GET: api/meetings/{id}
        [HttpGet("{id}")]
        public IActionResult GetMeeting(int id)
        {
            // Toplantıyı getirme kodu
            var meeting = new Meeting(); // Örnek veri
            return Ok(meeting);
        }

        // POST: api/meetings
        [HttpPost]
        public IActionResult CreateMeeting([FromBody] Meeting meeting)
        {
            // Toplantıyı oluşturma kodu
            return CreatedAtAction(nameof(GetMeeting), new { id = meeting.Id }, meeting);
        }

        // PUT: api/meetings/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateMeeting(int id, [FromBody] Meeting meeting)
        {
            // Toplantıyı güncelleme kodu
            return NoContent();
        }

        // DELETE: api/meetings/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteMeeting(int id)
        {
            // Toplantıyı silme kodu
            return NoContent();
        }

        // GET: api/meetings/{id}/report
        [HttpGet("{id}/report")]
        public IActionResult GetMeetingReport(int id)
        {
            var report = new byte[0]; 
            return File(report, "application/pdf", "meeting-report.pdf");
        }
    }
}
