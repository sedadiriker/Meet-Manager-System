using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EntitiesLayer.Models;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using EntitiesLayer.DTOs.Meeting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;

namespace ApiLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingService _meetingService;
        private readonly IEmailService _emailService;
        private readonly ILogger<MeetingsController> _logger;
        private readonly IUserService _userService; // Eklenen servis

        public MeetingsController(IMeetingService meetingService, IEmailService emailService, ILogger<MeetingsController> logger, IUserService userService)
        {
            _meetingService = meetingService;
            _logger = logger;
            _emailService = emailService;
            _userService = userService; // Kullanıcı servisini ekleyin
        }

        // GET: api/meetings
        [HttpGet]
        [SwaggerOperation(Summary = "Tüm toplantıları getir")]
        public IActionResult GetMeetings()
        {
            var meetings = _meetingService.GetAllMeetings();
            return Ok(meetings);
        }

        // GET: api/meetings/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Belirli bir toplantıyı getir")]
        public IActionResult GetMeeting(int id)
        {
            var meeting = _meetingService.GetMeetingById(id);
            if (meeting == null)
            {
                return NotFound("Toplantı bulunamadı.");
            }
            return Ok(meeting);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting([FromForm] Meeting meeting, IFormFile documentPath)
        {
            if (meeting == null)
            {
                return BadRequest("Toplantı bilgileri geçersiz.");
            }
            _logger.LogInformation("CreateMeeting method called with meeting: {@Meeting}", meeting);

            // var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            // if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            // {
            //     return Unauthorized("Kullanıcı kimliği geçersiz.");
            // }

            // meeting.UserId = userId;

            if (documentPath != null && documentPath.Length > 0)
            {
                try
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var filePath = Path.Combine(uploadFolder, documentPath.FileName);

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await documentPath.CopyToAsync(stream);
                    }

                    meeting.DocumentPath = $"/uploads/{documentPath.FileName}";
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Dosya işlemi hatası: {ex.Message}");
                }
            }

            var createdMeeting = _meetingService.CreateMeeting(meeting);

            if (createdMeeting == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Toplantı oluşturulamadı.");
            }

            return CreatedAtAction(nameof(GetMeeting), new { id = createdMeeting.Id }, createdMeeting);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMeeting(int id, [FromForm] EditMeetingDto editMeetingDto)
        {
            if (editMeetingDto == null)
            {
                _logger.LogError("Güncelleme bilgileri eksik.");
                return BadRequest("Güncelleme bilgileri eksik.");
            }

            var existingMeeting = _meetingService.GetMeetingById(id);
            if (existingMeeting == null)
            {
                return NotFound("Toplantı bulunamadı.");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (userId == null || existingMeeting.UserId != int.Parse(userId))
            {
                return Unauthorized("Yetkisiz erişim.");
            }

            existingMeeting.Name = editMeetingDto.Name;
            existingMeeting.StartDate = editMeetingDto.StartDate;
            existingMeeting.EndDate = editMeetingDto.EndDate;
            existingMeeting.Description = editMeetingDto.Description;

            if (editMeetingDto.DocumentPath != null && editMeetingDto.DocumentPath.Length > 0)
            {
                try
                {
                    var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var filePath = Path.Combine(uploadFolder, editMeetingDto.DocumentPath.FileName);

                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await editMeetingDto.DocumentPath.CopyToAsync(stream);
                    }

                    existingMeeting.DocumentPath = $"http://localhost:5064/uploads/{editMeetingDto.DocumentPath.FileName}";
                }
                catch (Exception ex)
                {
                    _logger.LogError("Dosya yükleme hatası: {Message}", ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError, "Dosya güncelleme hatası.");
                }
            }

            var updatedMeeting = _meetingService.UpdateMeeting(id, existingMeeting);
            _logger.LogInformation("Güncellenmiş toplantı: {UpdatedMeeting}", updatedMeeting);

            return Ok(updatedMeeting);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMeeting(int id)
        {
            var existingMeeting = _meetingService.GetMeetingById(id);
            if (existingMeeting == null)
            {
                _logger.LogError($"Toplantı bulunamadı: {id}");
                return NotFound("Toplantı bulunamadı.");
            }

            // var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            // if (userId == null || existingMeeting.UserId != int.Parse(userId))
            // {
            //     return Unauthorized("Yetkisiz erişim.");
            // }

            _meetingService.DeleteMeeting(id);
            _logger.LogInformation($"Toplantı başarıyla silindi: {id}");

            return NoContent();
        }
    }
}
