using AutoMapper;
using MeetingService.App_Start;
using MeetingService.Data;
using MeetingService.Data.Entities;
using MeetingService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MeetingService.Controllers
{
    [BasicAuthenticationFilter(true)]
    [RoutePrefix("api/attendee")]
    public class AttendeeController : ApiController
    {
        private readonly IMMARepository _repository;
          private readonly IMapper _mapper;

          public AttendeeController(IMMARepository repository, IMapper mapper)
          {
              _repository = repository;
              _mapper = mapper;

          }

          [Route()]
          public async Task<IHttpActionResult> Get()
          {
              try
              {
                  var result = await _repository.GetAllMeetingsAsync();

                  // Mapping 
                  var meetings = _mapper.Map<IEnumerable<MeetingModel>>(result);

                  return Ok(meetings);
              }
              catch (Exception ex)
              {
                  // TODO Add Logging
                  return InternalServerError(ex);
              }
          }
          [Route()]
          public async Task<IHttpActionResult> Post(AttendeeModel model)
          {
              try
              {
                  if (ModelState.IsValid)
                  {
                      var attdendee = _mapper.Map<Attendee>(model);

                      _repository.AddAttendee(attdendee);

                      if (await _repository.SaveChangesAsync())
                      {
                          var newModel = _mapper.Map<AttendeeModel>(attdendee);
                          return Ok(newModel);
                      }
                  }
                  return BadRequest(ModelState);
              }
              catch (Exception ex)
              {
                  return InternalServerError(ex);
              }
          }
    }
}