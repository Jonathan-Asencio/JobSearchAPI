using putavettoworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Repository.iRepository
{
    public interface iJobsRepository
    {
        ICollection<Job> GetJobs();
        ICollection<Job> GetJobsInJobSearch(int jsId);
        Job GetJob(int JobId);
        bool JobExists(string name);
        bool JobExists(int id);
        bool CreateJob(Job Job);
        bool UpdateJob(Job Job);
        bool DeleteJob(Job Job);
        bool Save();
    }
}
