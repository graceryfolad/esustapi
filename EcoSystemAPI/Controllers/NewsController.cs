using System.Diagnostics;
using esust.Models;
using esust.Repositories;
using Microsoft.AspNetCore.Mvc;
using QuizFramework.Controllers;

namespace esust.Controllers
{
    public class NewsController : MasterController
    {
        NewsRepo NewsRepo;

        public NewsController(NewsRepo newsRepo)
        {
            NewsRepo = newsRepo;
        }

        [HttpGet("NewsList")]
        public IActionResult Index()
        {
          var news =  NewsRepo.GetAll();
            customResponse.Data = news;
            customResponse.StatusCode = 200;
            return Ok(customResponse);
        }

        [HttpGet("News/{newsid}")]
        public IActionResult Newsdetails(int newsid)
        {
            var news = NewsRepo.SearchFor(n=>n.Id==newsid);
            customResponse.Data = news;
            customResponse.StatusCode = 200;
            return Ok(customResponse);
        }

        [HttpPost("CreateNews")]
        public IActionResult CreateNews(AddNews addNews)
        {
            
            return Ok(customResponse);
        }

        [HttpGet("CreateNews")]
        public IActionResult CreateNews()
        {

            return Ok(customResponse);
        }

    }
}
