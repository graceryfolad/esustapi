using System.Diagnostics;
using System.IO;
using DataAccess.Helpers;
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

        [HttpPost("ChangeImage")]
        public async Task<IActionResult> ChangeImageAsync(ChangeImage changeImage)
        {
            if (!ModelState.IsValid)
            {
                customResponse.Error = ModelState;
                customResponse.Message = "Validation Error";
                return BadRequest(customResponse);
            }

            if (changeImage.ImageType == "news")
            {
                var news = newsRepo.SearchFor(n => n.Id == changeImage.ID).SingleOrDefault();
                if(news !=null && news.Id > 0)
                {
                    var (isValid, message) = await FileValidator.UploadDocumentAsync(changeImage.ImageUrl, "News");
                    if (!isValid)
                        return BadRequest(message);

                    // save to news table
                    news.DefaultImageUrl = message;

                    if (newsRepo.Update(news))
                    {
                        customResponse.StatusCode = 200;
                        customResponse.Data = true;

                        return Ok(customResponse);
                    }

                    customResponse.Error = newsRepo.ErrorMessage;
                    customResponse.Message = "Image could not be changed";
                }
                else
                {
                    customResponse.Message = "Invalid news ID";
                }

                
               
            }
            else if (changeImage.ImageType == "events")
            {
                var news = eventRepo.SearchFor(n => n.Id == changeImage.ID).SingleOrDefault();
                if (news != null && news.Id > 0)
                {
                    var (isValid, message) = await FileValidator.UploadDocumentAsync(changeImage.ImageUrl, "Events");
                    if (!isValid)
                        return BadRequest(message);

                    // save to news table
                    news.ImageUrl = message;

                    if (eventRepo.Update(news))
                    {
                        customResponse.StatusCode = 200;
                        customResponse.Data = true;

                        return Ok(customResponse);
                    }

                    customResponse.Error = eventRepo.ErrorMessage;
                    customResponse.Message = "Image could not be changed";
                }
                else
                {
                    customResponse.Message = "Invalid ID";
                }


            }
            else if (changeImage.ImageType == "sliders")
            {
                var news = imageSliderRepo.SearchFor(n => n.Id == changeImage.ID).SingleOrDefault();

                if (news != null && news.Id > 0)
                {
                    var (isValid, message) = await FileValidator.UploadDocumentAsync(changeImage.ImageUrl, "Sliders");
                    if (!isValid)
                        return BadRequest(message);

                    // save to news table
                    news.Image = message;

                    if (imageSliderRepo.Update(news))
                    {
                        customResponse.StatusCode = 200;
                        customResponse.Data = true;

                        return Ok(customResponse);
                    }

                    customResponse.Error = imageSliderRepo.ErrorMessage;
                    customResponse.Message = "Image could not be changed";
                }
                else
                {
                    customResponse.Message = "Invalid  ID";
                }


            }

            return Ok(customResponse);
        }
    }
}
