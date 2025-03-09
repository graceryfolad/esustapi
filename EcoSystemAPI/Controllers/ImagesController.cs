using System.Diagnostics;
using esust.Models;
using esust.Repositories;
using Microsoft.AspNetCore.Mvc;
using QuizFramework.Controllers;

namespace esust.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ImagesController : MasterController
    {
       ImageSliderRepo imageSliderRepo;
        NewsRepo newsRepo;
        EventRepo eventRepo;

        public ImagesController(ImageSliderRepo sliderRepo, NewsRepo _newsRepo, EventRepo _eventRepo)
        {
            imageSliderRepo = sliderRepo;
            newsRepo = _newsRepo;
            eventRepo = _eventRepo;
        }

        [HttpGet("Picture")]
        public IActionResult Index([FromQuery] int id, string location)
        {
            string path = string.Empty;

            if (location == "news")
            {
                var news = newsRepo.SearchFor(n => n.Id == id).SingleOrDefault();

                path = news.DefaultImageUrl;
            }
            else if(location == "events")
            {
                var news = eventRepo.SearchFor(n => n.Id == id).SingleOrDefault();

                path = news.ImageUrl;
            }
            else if (location == "sliders")
            {
                var news = imageSliderRepo.SearchFor(n => n.Id == id).SingleOrDefault();

                path = news.Image;
            }



            if (!System.IO.File.Exists(path))
                return NotFound();

            var fileStream = System.IO.File.OpenRead(path);

            return File(fileStream, "image/jpeg");
        }

       
    }
}
