using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using putavettoworkAPI.Dtos;
using putavettoworkAPI.Models;
using putavettoworkAPI.Repository.iRepository;

namespace putavettoworkAPI.Controllers
{
    [Route("api/v{version:apiVersion}/jobsearch")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "putavettoworkOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class JobSearchController : ControllerBase
    {
        private readonly iJobSearchRepository _jsRepo;
        private readonly IMapper _mapper;

        public JobSearchController(iJobSearchRepository jsRepo, IMapper mapper)
        {
            _jsRepo = jsRepo;
            _mapper = mapper;
        }


        /// <summary>
        /// Get list of available jobs.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<JobSearchDto>))]
        public IActionResult GetJobSearch()
        {
            var objList = _jsRepo.GetJobSearch();
            var objDto = new List<JobSearchDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<JobSearchDto>(obj));
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get individual Jobs
        /// </summary>
        /// <param name="jobSearchId"> The Id of the job </param>
        /// <returns></returns>
        [HttpGet("{jobSearchId:int}", Name = "GetJobSearch")]
        [ProducesResponseType(200, Type = typeof(JobSearchDto))]
        [ProducesResponseType(404)]
        [Authorize]
        [ProducesDefaultResponseType]
        public IActionResult GetJobSearch(int jobSearchId)
        {
            var obj = _jsRepo.GetJobSearch(jobSearchId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<JobSearchDto>(obj);
            //var objDto = new JobSearchDto()
            //{
            //    Created = obj.Created,
            //    Id = obj.Id,
            //    Name = obj.Name,
            //    State = obj.State,
            //};
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
            return CreatedAtRoute("GetJobSearch", new { jobSearchId = jobSearchObj.Id }, jobSearchObj);
        }

        [HttpPatch("{jobSearchId:int}", Name = "UpdateJobSearch")]
        [ProducesResponseType(204)]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletejobSearch(int jobSearchId)
        {
            if (!_jsRepo.JobSearchExists(jobSearchId))
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