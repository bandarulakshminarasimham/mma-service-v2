using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingService.Models
{
    public class Meeting_Attendees_MapModel
    {
        public int MapId { get; set; }
        public Nullable<int> MeetingId { get; set; }
        public Nullable<int> AttendeeId { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}