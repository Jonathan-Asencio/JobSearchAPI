using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using putavettowork.Models;
using putavettowork.Repository.iRepository;

namespace putavettowork.Controllers
{
    public class JobSearchController : Controller
    {
        private readonly IJobSearchRepository _jsRepo;

        public JobSearchController(IJobSearchRepository jsRepo)
        {
            _jsRepo = jsRepo;
        }
        public IActionResult Index()
        {
            return View(new JobSearch() { });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            JobSearch obj = new JobSearch();
            if (id == null)
            {
                //true for insert or create
                return View(obj);
            }
            obj = await _jsRepo.GetAsync(SD.JobSearchAPIPath,id.GetValueOrDefault());
            if (obj == null) 
            {
                //update
                return NotFound();
            }
            return View(obj);
        }

        public async Task<IActionResult> GetAllJobSearch() 
        {
            return Json(new { data = await _jsRepo.GetAllAsync(SD.JobSearchAPIPath) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(JobSearch obj) 
        {
            if (ModelState.IsValid) 
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    obj.Picture = p1;
                }
                else 
                {
                    var objFromDb = await _jsRepo.GetAsync(SD.JobSearchAPIPath, obj.Id);
                    obj.Picture = objFromDb.Picture;
                }
                if (obj.Id == 0)
                {
                    await _jsRepo.CreateAsync(SD.JobSearchAPIPath, obj);
                }
                else
                {
                    await _jsRepo.UpdateAsync(SD.JobSearchAPIPath + obj.Id, obj);
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
            var status = await _jsRepo.DeleteAsync(SD.JobSearchAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful"  });
            }
            return Json(new { success = true, message = "Delete Not Successful" });
        }
    }
}