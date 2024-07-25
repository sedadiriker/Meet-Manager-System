using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EntitiesLayer.Models;
using BusinessLayer.Services;
using BusinessLayer.Interfaces;
using System.Collections.Generic;

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
        public IActionResult CreateMeeting([FromBody] Meeting meeting)
        {
            if (meeting == null)
            {
                return BadRequest();
            }
            var createdMeeting = _meetingService.CreateMeeting(meeting);
            return CreatedAtAction(nameof(GetMeeting), new { id = createdMeeting.Id }, createdMeeting);
        }

        // PUT: api/meetings/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateMeeting(int id, [FromBody] Meeting meeting)
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
            var report = new byte[0]; 
            return File(report, "application/pdf", "meeting-report.pdf");
        }
    }
}
