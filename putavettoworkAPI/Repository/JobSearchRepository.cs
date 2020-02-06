using putavettoworkAPI.Data;
using putavettoworkAPI.Models;
using putavettoworkAPI.Repository.iRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Repository
{
    public class JobSearchRepository : iJobSearchRepository
    {

        private readonly ApplicationDbContext _db;

        public JobSearchRepository(ApplicationDbContext db) 
        {
            _db = db;
        }
        public bool CreateJobSearch(JobSearch jobSearch)
        {
            _db.JobSearch.Add(jobSearch);
            return Save();
        }

        public bool DeleteJobSearch(JobSearch jobSearch)
        {
            _db.JobSearch.Remove(jobSearch);
            return Save();
        }

        public JobSearch GetJobSearch(int jobSearchId)
        {
            return _db.JobSearch.FirstOrDefault(a => a.Id ==jobSearchId);
        }

        public ICollection<JobSearch> GetJobSearch()
        {
            return _db.JobSearch.OrderBy(a => a.Name).ToList();
        }

        public bool JobSearchExists(string name)
        {
            bool value = _db.JobSearch.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }
        
        public bool JobSearchExists(int id)
        {
            return _db.JobSearch.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateJobSearch(JobSearch jobSearch)
        {
            _db.JobSearch.Update(jobSearch);
            return Save();
        }
    }
}
