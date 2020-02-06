using putavettoworkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace putavettoworkAPI.Repository.iRepository
{
    public interface iNationalParkRepository
    {
        ICollection<JobSearch> GetNationalParks();
        JobSearch GetNationalPark(int nationalParkId);
        bool NationalParkExists(string name);
        bool NationalParkExists(int id);
        bool CreateNationalPark(JobSearch nationalPark);
        bool UpdateNationalPark(JobSearch nationalPark);
        bool DeleteNationalPark(JobSearch nationalPark);
        bool Save();
    }
}
