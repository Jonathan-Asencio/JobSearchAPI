using Microsoft.EntityFrameworkCore;
using putavettoworkAPI.Data;
using putavettoworkAPI.Models;
using putavettoworkAPI.Repository.iRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Repository
{
    public class JobsRepository : iJobsRepository
    {

        private readonly ApplicationDbContext _db;

        public JobsRepository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public bool CreateJob(Jobs Job)
        {
            _db.Jobs.Add(Job);
            return Save();
        }

        public bool DeleteJob(Jobs Job)
        {
            _db.Jobs.Remove(Job);
            return Save();
        }

        public Jobs GetJob(int JobId)
        {
            return _db.Jobs.Include(c => c.JobSearch).FirstOrDefault(a => a.Id ==JobId);
        }

        public ICollection<Jobs> GetJobs()
        {
            return _db.Jobs.Include(c => c.JobSearch).OrderBy(a => a.Name).ToList();
        }

        public bool JobExists(string name)
        {
            bool value = _db.Jobs.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }
        
        public bool JobExists(int id)
        {
            return _db.Jobs.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateJob(Jobs Job)
        {
            _db.Jobs.Update(Job);
            return Save();
        }

        public ICollection<Jobs> GetJobsInJobSearch(int jsId)
        {
            return _db.Jobs.Include(c=>c.JobSearch).Where(c=>c.JobSearchId == jsId).ToList();
        }
    }
}
