namespace BusinessLayer.Interfaces
{
    public interface IReportService
    {
        byte[] GenerateMeetingReport(int meetingId);
    }
}
