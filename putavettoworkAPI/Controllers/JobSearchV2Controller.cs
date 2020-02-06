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
    [Route("api/v/{version:apiVersion}/nationalParks")]
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class JobSearchController : ControllerBase
    {
        private iNationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public JobSearchController(iNationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<JobSearchDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalParks() 
        {
            var objList = _npRepo.GetNationalParks();

            var objDto = new List<JobSearchDto>();

            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<JobSearchDto>(obj));
            }

            return Ok(objDto);
        }

    }
}