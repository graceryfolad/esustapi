using System.Diagnostics;
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
    public class SliderController : MasterController
    {
       ImageSliderRepo imageSliderRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SliderController(ImageSliderRepo sliderRepo, IHttpContextAccessor contextAccessor)
        {
            imageSliderRepo = sliderRepo;
            _httpContextAccessor = contextAccessor;
        }
        [AllowAnonymous]
        [HttpGet("HomeSlider")]
        public IActionResult Index()
        {
            var sliders = imageSliderRepo.GetAll();
            if(sliders == null || sliders.Count == 0)
            {
                customResponse.Data = false;
                customResponse.Message = "Slider is emppty";

                return Ok(customResponse);  
            }
            var request = _httpContextAccessor.HttpContext.Request;
            List<ImageSliderTbl> images = new List<ImageSliderTbl>();
            foreach (var item in sliders)
            {
                item.Image = $"{request.Scheme}://{request.Host}/api/Picture?id={item.Id}&location=sliders";
                images.Add(item);
            }

            customResponse.Data = images;
            customResponse.StatusCode = 200;
            return Ok(customResponse);

         //   return Ok(customResponse);

        }

        [HttpPost("CreateHomeSlider")]
        public async Task<IActionResult> CreateEventAsync(CreateSlider create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var (isValid, message) = await FileValidator.UploadDocumentAsync(create.Image, "Sliders");
            if (!isValid)
                return BadRequest(message);

            var evt = imageSliderRepo.Insert(new ImageSliderTbl()
            {
                Title = create.Title,
                Description = create.Description,
                Image = message,
                LinkAction = create.LinkAction,
                
            });

            if (evt)
            {
                customResponse.Data = evt;
                customResponse.StatusCode = 200;
                customResponse.Message = "Slider Created";

                return Ok(customResponse);
            }

            customResponse.Message = "Create Slider failed";
            customResponse.Data = false;

            return Ok(customResponse);
        }


    }
}
