using System.Diagnostics;
using DataAccess.Helpers;
using esust.Models;
using esust.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizFramework.Controllers;

namespace esust.Controllers
{
    [Authorize]
    public class NewsController : MasterController
    {
        NewsRepo NewsRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsController(NewsRepo newsRepo, IHttpContextAccessor httpContext)
        {
            NewsRepo = newsRepo;
           _httpContextAccessor = httpContext;
        }
        [AllowAnonymous]
        [HttpGet("NewsList")]
        public IActionResult Index()
        {
          var news =  NewsRepo.GetAll();
            if (news == null || news.Count == 0)
            {
                customResponse.Message = "News is empty";
                return Ok(customResponse);
            }
            var request = _httpContextAccessor.HttpContext.Request;
            foreach (var item in news)
            {              

                // Construct absolute URL
                var imageUrl = $"{request.Scheme}://{request.Host}/api/Picture?id={item.Id}&location=news";

                item.DefaultImageUrl = imageUrl;
            }
            customResponse.Data = news;
            customResponse.StatusCode = 200;
            return Ok(customResponse);
        }

        [AllowAnonymous]
        [HttpGet("{newsid}")]
        public IActionResult Newsdetails(int newsid)
        {
            var news = NewsRepo.SearchFor(n=>n.Id==newsid).SingleOrDefault();
            if (news == null)
            {
                customResponse.Message = "Not found";
                return NotFound();
            }

            var request = _httpContextAccessor.HttpContext.Request;

            // Construct absolute URL
            var imageUrl = $"{request.Scheme}://{request.Host}/api/Picture?id={news.Id}&location=news";

            news.DefaultImageUrl = imageUrl;

            customResponse.Data = news;
            customResponse.StatusCode = 200;

            return Ok(customResponse);
        }

        [HttpPost("CreateNews")]
        public async Task<IActionResult> CreateNewsAsync([FromForm] AddNews addNews)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Process image upload
                var image = addNews.DefaultImageUrl;
                if (image == null || image.Length == 0)
                    return BadRequest("Invalid image file");

                //var (isValid, errorMessage) = FileValidator.ValidateImageFile(
                //      addNews.DefaultImageUrl,
                //      maxSizeBytes: 2 * 1024 * 1024, // 2MB
                //      maxWidth: 1920,
                //      maxHeight: 1080);

                //if (!isValid)
                //    return BadRequest(errorMessage);


                //var uploadsFolder = $"{AppDomain.CurrentDomain.BaseDirectory}\\Upload\\News";
                //if (!Directory.Exists(uploadsFolder))
                //    Directory.CreateDirectory(uploadsFolder);

                //var fileName = FileValidator.GenerateUniqueFileName(image);
                //var filePath = Path.Combine(uploadsFolder, fileName);



                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await image.CopyToAsync(stream);
                //}

                var (isValid, message) =  await FileValidator.UploadDocumentAsync(addNews.DefaultImageUrl,"News");
                if (!isValid)
                    return BadRequest(message);

                    // Here you would typically save to database
                    if (  NewsRepo.Insert(new NewsTbl()
                {
                    Body = addNews.Body,
                    Title = addNews.Title,
                    DefaultImageUrl =message,
                    CreatedDate = DateTime.Now
                }))
                {
                    customResponse.Data = NewsRepo.entry;
                    customResponse.StatusCode = 200;

                    return Ok(customResponse);
                }

                 customResponse.Error = NewsRepo.ErrorMessage;
                
                return Ok(customResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(customResponse);
        }

        [HttpPost("EditNews")]
        public IActionResult CreateNews(EditNews edit)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var news = NewsRepo.GetById(edit.NewsID);

            news.Title = edit.Title;
            news.Body = edit.Body;
            news.ModifiedDate = DateTime.Now;

            if (NewsRepo.Update(news))
            {
                customResponse.Data = news;
                customResponse.StatusCode = 200;

                return Ok(customResponse);
            }

            customResponse.Message = NewsRepo.ErrorMessage;
            
            return Ok(customResponse);
        }

        [AllowAnonymous]
        [HttpGet("Picture/{newsid}")]
        public IActionResult getPicture(int newsid)
        {
            //  customResponse.Data
            var news = NewsRepo.SearchFor(n=>n.Id == newsid).SingleOrDefault();

            string path = news.DefaultImageUrl;

            if (!System.IO.File.Exists(path))
                return NotFound();

            var fileStream = System.IO.File.OpenRead(path);

            return File(fileStream, "image/jpeg");


        }

    }
}
