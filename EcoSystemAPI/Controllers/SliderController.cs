using System.Diagnostics;
using esust.Models;
using esust.Repositories;
using Microsoft.AspNetCore.Mvc;
using QuizFramework.Controllers;

namespace esust.Controllers
{
    public class SliderController : MasterController
    {
       ImageSliderRepo imageSliderRepo;

        public SliderController(ImageSliderRepo sliderRepo)
        {
            imageSliderRepo = sliderRepo;
        }

        [HttpGet("HomeSlider")]
        public IActionResult Index()
        {
            var sliders = imageSliderRepo.GetAll();
            customResponse.Data = sliders;
            customResponse.StatusCode = 200;
            return Ok();
        }

       
    }
}
