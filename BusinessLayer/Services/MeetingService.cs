using DataAccessLayer.Data;
using EntitiesLayer.Models;
using System.Collections.Generic;
using BusinessLayer.Interfaces;
using System.Linq;

namespace BusinessLayer.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly ApplicationDbContext _context;

        public MeetingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Meeting> GetAllMeetings()
        {
            return _context.Meetings.ToList();
        }

        public Meeting GetMeetingById(int id)
        {
            return _context.Meetings.Find(id);
        }

        public Meeting CreateMeeting(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
            _context.SaveChanges();
            return meeting;
        }

        public void UpdateMeeting(int id, Meeting meeting)
        {
            var existingMeeting = _context.Meetings.Find(id);
            if (existingMeeting != null)
            {
                existingMeeting.Name = meeting.Name;
                existingMeeting.StartDate = meeting.StartDate;
                existingMeeting.EndDate = meeting.EndDate;
                existingMeeting.Description = meeting.Description;
                existingMeeting.DocumentPath = meeting.DocumentPath;

                _context.Meetings.Update(existingMeeting);
                _context.SaveChanges();
            }
        }

        public void DeleteMeeting(int id)
        {
            var meeting = _context.Meetings.Find(id);
            if (meeting != null)
            {
                _context.Meetings.Remove(meeting);
                _context.SaveChanges();
            }
        }
    }
}
