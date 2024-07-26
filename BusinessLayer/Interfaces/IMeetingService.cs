using EntitiesLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces
{
    public interface IMeetingService
    {
        IEnumerable<Meeting> GetAllMeetings();
        Meeting GetMeetingById(int id);
        Meeting CreateMeeting(Meeting meeting);
        Meeting UpdateMeeting(int id, Meeting meeting);
        void DeleteMeeting(int id);
    }
}
