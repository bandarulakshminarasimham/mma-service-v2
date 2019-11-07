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
    [RoutePrefix("api/meeting")]
    public class MeetingController : ApiController
    {
          private readonly IMMARepository _repository;
          private readonly IMapper _mapper;

          public MeetingController(IMMARepository repository, IMapper mapper)
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

          public async Task<IHttpActionResult> Get(int id)
          {
              try
              {
                  var result = await  _repository.GetMeetingAsync(id);
                  var meetings = _mapper.Map<IEnumerable<MeetingModel>>(result);

                  return Ok(meetings);
              }
              catch (Exception ex)
              {
                  return InternalServerError(ex);
              }
          }

          [Route()]
          public async Task<IHttpActionResult> Post(MeetingModel model)
          {
              try
              {
                  if (ModelState.IsValid)
                  {
                      model.Attendees.All(t =>
                      {
                          model.Meeting_Attendees_Map.Add(new Meeting_Attendees_MapModel { AttendeeId = t });
                          return true;
                      });
                      var meeting = _mapper.Map<Meeting>(model);

                      _repository.AddMeeting(meeting);

                      if (await _repository.SaveChangesAsync())
                      {
                          var newModel = _mapper.Map<MeetingModel>(meeting);
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

            [Route()]
          public async Task<IHttpActionResult> Put(int id, MeetingModel model)
          {
              try
              {
                  var meeting = await _repository.GetMeetingAsync(id);
                  if (meeting == null) return NotFound();

                  _mapper.Map(model, meeting);

                  if (await _repository.SaveChangesAsync())
                  {
                      return Ok(_mapper.Map<MeetingModel>(meeting));
                  }
                  else
                  {
                      return InternalServerError();
                  }
              }
              catch (Exception ex)
              {
                  return InternalServerError(ex);
              }
          }
    }
}
