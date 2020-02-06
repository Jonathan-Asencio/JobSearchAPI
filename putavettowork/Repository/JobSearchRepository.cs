using putavettowork.Models;
using putavettowork.Repository.iRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace putavettowork.Repository
{
    public class JobSearchRepository : Repository<JobSearch>, IJobSearchRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public JobSearchRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
