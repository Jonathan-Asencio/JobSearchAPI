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
        private readonly IJobSearchRepository _npRepo;

        public JobSearchController(IJobSearchRepository npRepo)
        {
            _npRepo = npRepo;
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
            obj = await _npRepo.GetAsync(SD.NationalParkAPIPath,id.GetValueOrDefault());
            if (obj == null) 
            {
                //update
                return NotFound();
            }
            return View(obj);
        }

        public async Task<IActionResult> GetAllNationalPark() 
        {
            return Json(new { data = await _npRepo.GetAllAsync(SD.NationalParkAPIPath) });
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
                    var objFromDb = await _npRepo.GetAsync(SD.NationalParkAPIPath, obj.Id);
                    obj.Picture = objFromDb.Picture;
                }
                if (obj.Id == 0)
                {
                    await _npRepo.CreateAsync(SD.NationalParkAPIPath, obj);
                }
                else
                {
                    await _npRepo.UpdateAsync(SD.NationalParkAPIPath + obj.Id, obj);
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
            var status = await _npRepo.DeleteAsync(SD.NationalParkAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful"  });
            }
            return Json(new { success = true, message = "Delete Not Successful" });
        }
    }
}