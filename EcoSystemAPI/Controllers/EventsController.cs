using System.Diagnostics;
using esust.Models;
using esust.Repositories;
using Microsoft.AspNetCore.Mvc;
using QuizFramework.Controllers;

namespace esust.Controllers
{
    [Route("api/")]
    public class EventsController : MasterController
    {
        EventRepo EventRepo;

        public EventsController(EventRepo eventRepo)
        {
           this.EventRepo = eventRepo;
        }

        [HttpGet("EventList")]
        public IActionResult EventList()
        {
            var events = this.EventRepo.GetAll();
            customResponse.Data = events;
            customResponse.StatusCode = 200;
            return Ok(customResponse);
        }
        [HttpGet("Event/{eventid}")]
        public IActionResult EventDetials(int eventid)
        {
            var events = this.EventRepo.SearchFor(n=>n.Id == eventid);
            customResponse.Data = events;
            customResponse.StatusCode = 200;
            return Ok(customResponse);
        }


    }
}
