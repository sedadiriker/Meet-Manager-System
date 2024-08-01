using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using BusinessLayer.Interfaces;
using EntitiesLayer.Models;

namespace BusinessLayer.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Meeting> _meetingRepository;

        public ReportService(IRepository<Meeting> meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        public byte[] GenerateMeetingReport(int meetingId)
        {
            // Toplantı bilgilerini veri tabanından al
            var meeting = _meetingRepository.GetById(meetingId);

            if (meeting == null)
                throw new Exception("Toplantı bulunamadı.");

            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdfDocument = new PdfDocument(writer);
                var document = new Document(pdfDocument);

                document.Add(new Paragraph(meeting.Name)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetBold());

                document.Add(new Paragraph($"Date: {meeting.StartDate.ToString("dd.MM.yyyy")}")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(12)
                    .SetItalic());


                document.Close();

                return stream.ToArray();
            }
        }
    }
}
