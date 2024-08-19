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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiLayer.Controllers
{
 [Route("api/[controller]")]
[ApiController]
public class MeetingReportsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MeetingReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/MeetingReports
    [HttpGet]
    public async Task<IActionResult> GetReports()
    {
        var reports = await _context.MeetingReports
            .Include(r => r.Meeting) // İlişkili toplantıyı da yükle
            .ToListAsync();

        return Ok(reports);
    }

    // POST: api/MeetingReports/generate
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateReport([FromBody] int meetingId)
    {
        var meeting = await _context.Meetings.FindAsync(meetingId);
        if (meeting == null)
        {
            return NotFound();
        }

        // Rapor oluşturma işlemi
        var reportUrl = await CreateReport(meeting);

        var report = new MeetingReport
        {
            MeetingId = meetingId,
            ReportUrl = reportUrl,
            CreatedAt = DateTime.UtcNow
        };

        _context.MeetingReports.Add(report);
        await _context.SaveChangesAsync();

        return Ok(new { reportUrl });
    }

    private Task<string> CreateReport(Meeting meeting)
    {
        // Rapor oluşturma ve URL döndürme işlemi
        // Burada PDF rapor oluşturup bir URL döndürmelisiniz
        // Örneğin:
        var reportUrl = "http://example.com/reports/meeting_report.pdf";
        return Task.FromResult(reportUrl);
    }
}


}
