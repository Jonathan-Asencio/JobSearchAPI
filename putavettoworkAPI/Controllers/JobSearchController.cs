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

namespace putavettoworkAPI.Controllers
{
    [Route("api/v/{version:apiVersion}/jobSearch")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "putavettoworkOpenAPISpec")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class JobSearchV2Controller : ControllerBase
    {
        private iJobSearchRepository _jsRepo;
        private readonly IMapper _mapper;

        public JobSearchV2Controller(iJobSearchRepository jsRepo, IMapper mapper)
        {
            _jsRepo = jsRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<JobSearchDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetJobSearchs() 
        {
            var objList = _jsRepo.GetJobSearch();

            var objDto = new List<JobSearchDto>();

            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<JobSearchDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get list of jobs
        /// </summary>
        /// <param name="jobSearchId"> The ID of the Job </param>
        /// <returns></returns>
        //get request with required arguments
        [HttpGet("{jobSearchId:int}", Name = "GetJobSearch") ]
        [ProducesResponseType(200, Type = typeof(JobSearchDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetJobSearch(int jobSearchId) 
        {
            var obj = _jsRepo.GetJobSearch(jobSearchId);
            if (obj == null) 
            {
                return NotFound();
            }
            var objDto = _mapper.Map<JobSearchDto>(obj);
            return Ok(objDto);

        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(JobSearchDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateJobSearch([FromBody] JobSearchDto jobSearchDto) 
        {
            if (jobSearchDto == null) 
            {
                return BadRequest(ModelState);
            }

            if (_jsRepo.JobSearchExists(jobSearchDto.Name)) 
            {
                ModelState.AddModelError("", "Job Exists!");
                return StatusCode(404, ModelState);
            }

            var jobSearchObj = _mapper.Map<JobSearch>(jobSearchDto);

            if (!_jsRepo.CreateJobSearch(jobSearchObj)) 
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {jobSearchObj.Name}");
                return StatusCode(500, ModelState);

            }
            return CreatedAtRoute("GetJobSearch", new {jobSearchId = jobSearchObj.Id, jobSearchObj});
        }

        [HttpPatch("{jobSearchId:int}", Name = "UpdateJobSearch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateJobSearch(int jobSearchId, [FromBody] JobSearchDto jobSearchDto) 
        {
            if (jobSearchDto == null || jobSearchId != jobSearchDto.Id)
            {
                return BadRequest(ModelState);
            }

            var jobSearchObj = _mapper.Map<JobSearch>(jobSearchDto);

            if (!_jsRepo.UpdateJobSearch(jobSearchObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {jobSearchObj.Name}");
                return StatusCode(500, ModelState);

            }
            return NoContent();  
        }

        [HttpDelete("{jobSearchId:int}", Name = "DeleteJobSearch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteJobSearch(int jobSearchId)
        {
            if (_jsRepo.JobSearchExists(jobSearchId))
            {
                return NotFound();
            }

            var jobSearchObj = _jsRepo.GetJobSearch(jobSearchId);
            if (!_jsRepo.DeleteJobSearch(jobSearchObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {jobSearchObj.Name}");
                return StatusCode(500, ModelState);

            }
            return NoContent();
        }

    }
}