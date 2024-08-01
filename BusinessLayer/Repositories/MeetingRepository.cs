using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Interfaces;
using EntitiesLayer.Models;
using DataAccessLayer.Data;

namespace BusinessLayer.Repositories
{
    public class MeetingRepository : IRepository<Meeting>
    {
        private readonly ApplicationDbContext _context;

        public MeetingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Meeting GetById(int id)
        {
            return _context.Meetings.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Meeting> GetAll()
        {
            return _context.Meetings.ToList();
        }

        public void Add(Meeting entity)
        {
            _context.Meetings.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Meeting entity)
        {
            _context.Meetings.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var meeting = _context.Meetings.FirstOrDefault(m => m.Id == id);
            if (meeting != null)
            {
                _context.Meetings.Remove(meeting);
                _context.SaveChanges();
            }
        }
    }
}
