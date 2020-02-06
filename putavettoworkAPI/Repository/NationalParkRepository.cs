using putavettoworkAPI.Data;
using putavettoworkAPI.Models;
using putavettoworkAPI.Repository.iRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Repository
{
    public class NationalParkRepository : iNationalParkRepository
    {

        private readonly ApplicationDbContext _db;

        public NationalParkRepository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public bool CreateNationalPark(JobSearch nationalPark)
        {
            _db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(JobSearch nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public JobSearch GetNationalPark(int nationalParkId)
        {
            return _db.NationalParks.FirstOrDefault(a => a.Id ==nationalParkId);
        }

        public ICollection<JobSearch> GetNationalParks()
        {
            return _db.NationalParks.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = _db.NationalParks.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }
        
        public bool NationalParkExists(int id)
        {
            return _db.NationalParks.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(JobSearch nationalPark)
        {
            _db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
