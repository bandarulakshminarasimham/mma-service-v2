using MeetingService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MeetingService.Data
{
    public class MMARepository : IMMARepository
    {
        private readonly MeetingManagementEntities _context;

        public MMARepository(MeetingManagementEntities context)
        {
          _context = context;
        }
        public async System.Threading.Tasks.Task<bool> SaveChangesAsync()
        {
            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async System.Threading.Tasks.Task<Meeting[]> GetAllMeetingsAsync()
        {
            IQueryable<Meeting> query = _context.Meetings;

            // Order It
            query = query.OrderByDescending(c => c.CreatedDate);

            return await query.ToArrayAsync();
        }


        public async Task<Meeting> GetMeetingAsync(int id)
        {
            IQueryable<Meeting> query = _context.Meetings.Where(t => t.MeetingId == id);

            // Order It
            query = query.OrderByDescending(c => c.CreatedDate);

            return await query.FirstOrDefaultAsync();
        }


        public async Task<User> GetUser(string name)
        {
            IQueryable<User> query = _context.Users.Where(t => t.Username == name);

            return await query.FirstOrDefaultAsync();
        }

        public void AddMeeting(Meeting meeting)
        {
            _context.Meetings.Add(meeting);
        }


        public async Task<Attendee[]> GetAllAttendeeAsync()
        {
            IQueryable<Attendee> query = _context.Attendees;

            // Order It
            query = query.OrderByDescending(c => c.CreatedDate);

            return await query.ToArrayAsync();
        }


        public void AddAttendee(Attendee attendee)
        {
            _context.Attendees.Add(attendee);
        }
    }
}