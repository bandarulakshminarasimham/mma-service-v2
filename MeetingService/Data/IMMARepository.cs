using MeetingService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MeetingService.Data
{
    public interface IMMARepository
    {

        // General  save db cahgnes
        Task<bool> SaveChangesAsync();

        // retrive meetings informtion
        Task<Meeting[]> GetAllMeetingsAsync();

        // retrive meeting by id
        Task<Meeting> GetMeetingAsync(int id);

        // retrive user by name
        Task<User> GetUser(string name);
        // create meeting
        void AddMeeting(Meeting meeting);

        // retrive attendees
        Task<Attendee[]> GetAllAttendeeAsync();

        // create meeting
        void AddAttendee(Attendee attendee);

    }
}