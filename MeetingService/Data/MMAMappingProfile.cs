using AutoMapper;
using MeetingService.Data.Entities;
using MeetingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MeetingService.Data
{
    public class MMAMappingProfile : Profile
    {
        public MMAMappingProfile()
        {
            CreateMap<Attendee, AttendeeModel>()
                .ReverseMap();

            CreateMap<Meeting, MeetingModel>()
                .ReverseMap();

            CreateMap<Meeting_Attendees_Map, Meeting_Attendees_MapModel>()
                .ReverseMap();
                
        }
    }
}