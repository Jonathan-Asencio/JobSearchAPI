using putavettoworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Repository.iRepository
{
    public interface iJobSearchRepository
    {
        ICollection<JobSearch> GetJobSearch();
        JobSearch GetJobSearch(int jobSearchId);
        bool JobSearchExists(string name);
        bool JobSearchExists(int id);
        bool CreateJobSearch(JobSearch jobSearch);
        bool UpdateJobSearch(JobSearch jobSearch);
        bool DeleteJobSearch(JobSearch jobSearch);
        bool Save();
    }
}
