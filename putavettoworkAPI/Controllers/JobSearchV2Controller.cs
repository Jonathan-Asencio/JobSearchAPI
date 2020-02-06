using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using putavettoworkAPI.Dtos;
using putavettoworkAPI.Models;
using putavettoworkAPI.Repository.iRepository;

//demo of version control
namespace putavettoworkAPI.Controllers
{
    [Route("api/v/{version:apiVersion}/jobSearch")]
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class JobSearchController : ControllerBase
    {
        private iJobSearchRepository _jsRepo;
        private readonly IMapper _mapper;

        public JobSearchController(iJobSearchRepository jsRepo, IMapper mapper)
        {
            _jsRepo = jsRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<JobSearchDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetJobSearch() 
        {
            var objList = _jsRepo.GetJobSearch();

            var objDto = new List<JobSearchDto>();

            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<JobSearchDto>(obj));
            }

            return Ok(objDto);
        }

    }
}