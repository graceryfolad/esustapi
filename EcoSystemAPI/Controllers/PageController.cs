using System.Diagnostics;
using esust.Models;
using esust.Repositories;
using ESUSTAPI.Models;
using Microsoft.AspNetCore.Mvc;
using QuizFramework.Controllers;

namespace esust.Controllers
{
    [Route("api/")]
    public class PageController : MasterController
    {
       PageRepo prepo;
        MenuRepo menuRepo;

        public PageController(PageRepo pageRepo, MenuRepo _menuRepo)
        {
            prepo = pageRepo;
            this.menuRepo = _menuRepo;
        }

        [HttpGet("Page/{menuid}")]
        public IActionResult Index(int menuid)
        {
            if (menuid > 0)
            {
              var mnu =  menuRepo.GetById(menuid);
                if (mnu != null)
                {
                    var apge = prepo.SearchFor(p=>p.MenuID == menuid).SingleOrDefault();

                    PageView pageView = new PageView()
                    {
                        Title = apge.Title,
                        Description = apge.Description,
                        Created = apge.CreatedDate,
                        Id = apge.Id,
                    }

                    ;

                    customResponse.Data = pageView;
                    customResponse.StatusCode = 200;
                }

                
            }
            return Ok(customResponse);
        }

        [HttpPost("PageEdit")]
        public IActionResult Editpage(PageView pagevw)
        {
            if (pagevw.Id > 0)
            {
                var mnu = menuRepo.GetById(pagevw.Id);
                if (mnu != null)
                {
                    var apge = prepo.SearchFor(p => p.MenuID == pagevw.Id).SingleOrDefault();

                    apge.Description = pagevw.Description;
                    apge.Title = pagevw.Title;

                  var resp = prepo.Update(apge);
                    if (resp)
                    {
                        mnu.Label = apge.Title;
                        menuRepo.Update(mnu);

                        customResponse.Data = resp;
                        customResponse.StatusCode = 200;
                    }
                    else
                    {
                        customResponse.Message = "Page update failed";
                    }
                   
                }


            }
            return Ok(customResponse);
        }
        [HttpGet("Page/ByTitle/{title}")]
        public IActionResult ByTitle(string title)
        {
            if (title.Length > 0)
            {
                var mnu = menuRepo.SearchFor(m=>m.Label == title).SingleOrDefault();
                if (mnu != null)
                {
                    var apge = prepo.SearchFor(p => p.Title == title).SingleOrDefault();

                    PageView pageView = new PageView()
                    {
                        Title = apge.Title,
                        Description = apge.Description,
                        Created = apge.CreatedDate,
                        Id = apge.Id,
                    }

                    ;

                    customResponse.Data = pageView;
                    customResponse.StatusCode = 200;
                }


            }
            return Ok(customResponse);
        }

        [HttpPost("SubmitLecturerEvaluation")]
        public IActionResult SubmitLecturerEval(LecturerEvaluation evaluation)
        {

            return Ok(customResponse);
        }


    }
}
