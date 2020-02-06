using putavettoworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Repository.iRepository
{
    public interface iJobsRepository
    {
        ICollection<Jobs> GetJobs();
        ICollection<Jobs> GetJobsInJobSearch(int jsId);
        Jobs GetJob(int JobId);
        bool JobExists(string name);
        bool JobExists(int id);
        bool CreateJob(Jobs Job);
        bool UpdateJob(Jobs Job);
        bool DeleteJob(Jobs Job);
        bool Save();
    }
}
