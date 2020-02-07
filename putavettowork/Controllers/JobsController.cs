using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using putavettowork.Models;
using putavettowork.Models.ViewModels;
using putavettowork.Repository.iRepository;

namespace putavettowork.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobSearchRepository _jsRepo;
        private readonly IJobsRepository _jobsRepo;


        public JobsController(IJobSearchRepository jsRepo, IJobsRepository jobsRepo)
        {
            _jsRepo = jsRepo;
            _jobsRepo = jobsRepo;
        }
        public IActionResult Index()
        {
            return View(new Jobs() { });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<JobSearch> jsList = await _jsRepo.GetAllAsync(SD.JobSearchAPIPath);

            JobsVM objVM = new JobsVM()
            {
                JobSearchList = jsList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //true for insert or create
                return View(objVM);
            }
            objVM.Jobs = await _jobsRepo.GetAsync(SD.JobsAPIPath,id.GetValueOrDefault());
            if (objVM.Jobs == null) 
            {
                //update
                return NotFound();
            }
            return View(objVM);
        }

        public async Task<IActionResult> GetAllJobs() 
        {
            return Json(new { data = await _jobsRepo.GetAllAsync(SD.JobsAPIPath) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(JobsVM obj) 
        {
            if (ModelState.IsValid) 
            {

                if (obj.Jobs.Id == 0)
                {
                    await _jobsRepo.CreateAsync(SD.JobsAPIPath, obj.Jobs);
                }
                else
                {
                    await _jobsRepo.UpdateAsync(SD.JobsAPIPath + obj.Jobs.Id, obj.Jobs);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _jobsRepo.DeleteAsync(SD.JobsAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful"  });
            }
            return Json(new { success = true, message = "Delete Not Successful" });
        }
    }
}