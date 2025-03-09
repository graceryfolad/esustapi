using System.Diagnostics;
using Azure.Core;
using DataAccess.Helpers;
using esust.Models;
using esust.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizFramework.Controllers;

namespace esust.Controllers
{

    [Route("api/")]
    [ApiController]
    [Authorize]
    public class EventsController : MasterController
    {
        EventRepo EventRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventsController(EventRepo eventRepo, IHttpContextAccessor contextAccessor)
        {
           this.EventRepo = eventRepo;
            this._httpContextAccessor = contextAccessor;
        }

        [AllowAnonymous]
        [HttpGet("EventList")]
        public IActionResult EventList()
        {
            var events = this.EventRepo.GetAll();
            if (events != null && events.Count > 0)
            {
                List<EventTbl> eventsTbl = new List<EventTbl>();
                var request = _httpContextAccessor.HttpContext.Request;

                // Construct absolute URL          


                foreach (var item in events)
                {
                    item.ImageUrl = $"{request.Scheme}://{request.Host}/api/Picture?id={item.Id}&location=events";
                    eventsTbl.Add(item);
                }

                customResponse.Data = eventsTbl;
                customResponse.StatusCode = 200;
                return Ok(customResponse);
            }

            customResponse.Data = events;
            
            return Ok(customResponse);
        }

        [AllowAnonymous]
        [HttpGet("Event/{eventid}")]
        public IActionResult EventDetials(int eventid)
        {
            var events = this.EventRepo.SearchFor(n=>n.Id == eventid).SingleOrDefault();
            if(events == null)
                return Ok(customResponse);


            var request = _httpContextAccessor.HttpContext.Request;
            var imageUrl = $"{request.Scheme}://{request.Host}/api/Picture?id={events.Id}&location=events";
            events.ImageUrl = imageUrl ;

            customResponse.Data = events;
            customResponse.StatusCode = 200;
            return Ok(customResponse);
        }

        [HttpPost("CreateEvent")]
        public async Task<IActionResult> CreateEventAsync(CreateEvent createEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          

            var (isValid, message) = await FileValidator.UploadDocumentAsync(createEvent.ImageUrl, "Events");
            if (!isValid)
                return BadRequest(message);

            var evt = EventRepo.Insert(new EventTbl()
            {
                Body = createEvent.Body,
                Title = createEvent.Title,
                ImageUrl = message,
                CreatedDate = DateTime.Now,
                EventDate = createEvent.EventDate,
            });

            if (evt)
            {
                customResponse.Data = evt;
                customResponse.StatusCode=200;

                return Ok(customResponse);
            }

            customResponse.Message = "Create Event failed";
            customResponse.Data = false;

            return Ok(customResponse);
        }


    }
}
