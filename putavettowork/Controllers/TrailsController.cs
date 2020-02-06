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
    public class TrailsController : Controller
    {
        private readonly IJobSearchRepository _npRepo;
        private readonly ITrailRepository _trailRepo;


        public TrailsController(IJobSearchRepository npRepo, ITrailRepository trailRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<JobSearch> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //true for insert or create
                return View(objVM);
            }
            objVM.Trails = await _trailRepo.GetAsync(SD.TrailAPIPath,id.GetValueOrDefault());
            if (objVM.Trails == null) 
            {
                //update
                return NotFound();
            }
            return View(objVM);
        }

        public async Task<IActionResult> GetAllTrail() 
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailAPIPath) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj) 
        {
            if (ModelState.IsValid) 
            {

                if (obj.Trails.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailAPIPath, obj.Trails);
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailAPIPath + obj.Trails.Id, obj.Trails);
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
            var status = await _trailRepo.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful"  });
            }
            return Json(new { success = true, message = "Delete Not Successful" });
        }
    }
}