using putavettowork.Models;
using putavettowork.Repository.iRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace putavettowork.Repository
{

    public class JobsRepository : Repository<Jobs>, IJobsRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public JobsRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
