using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using putavettoworkAPI.Dtos;
using putavettoworkAPI.Models;
using putavettoworkAPI.Models.Dtos;
using putavettoworkAPI.Repository.iRepository;

namespace putavettoworkAPI.Controllers
{
    [Route("api/v/{version:apiVersion}/jobs")]
    //[Route("api/Jobs")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "putavettoworkOpenAPISpec")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class JobsController : ControllerBase
    {
        private iJobsRepository _jsRepo;
        private readonly IMapper _mapper;

        public JobsController(iJobsRepository jsRepo, IMapper mapper)
        {
            _jsRepo = jsRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<JobDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetJob() 
        {
            var objList = _jsRepo.GetJobs();

            var objDto = new List<JobDto>();

            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<JobDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get list of trail
        /// </summary>
        /// <param name="JobId"> The ID of the trail </param>
        /// <returns></returns>
        //get request with required arguments
        [HttpGet("{JobId:int}", Name = "GetJob") ]
        [ProducesResponseType(200, Type = typeof(JobDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetJob(int JobId) 
        {
            var obj = _jsRepo.GetJob(JobId);
            if (obj == null) 
            {
                return NotFound();
            }
            var objDto = _mapper.Map<JobDto>(obj);
            return Ok(objDto);

        }
        /// <summary>
        /// attribute routing example
        /// </summary>
        /// <param name="jobSearchId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{JobId:int}")]
        [ProducesResponseType(200, Type = typeof(JobDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetJobInJobSearch(int jobSearchId)
        {
            var objList = _jsRepo.GetJobsInJobSearch(jobSearchId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<JobDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<JobDto>(obj));
            }
            return Ok(objDto);

        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(JobDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateJob([FromBody] JobCreateDto JobDto) 
        {
            if (JobDto == null) 
            {
                return BadRequest(ModelState);
            }

            if (_jsRepo.JobExists(JobDto.Name)) 
            {
                ModelState.AddModelError("", "job Exists!");
                return StatusCode(404, ModelState);
            }

            var JobObj = _mapper.Map<Job>(JobDto);

            if (!_jsRepo.CreateJob(JobObj)) 
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {JobObj.Name}");
                return StatusCode(500, ModelState);

            }
            return CreatedAtRoute("GetJob", new {JobId = JobObj.Id, JobObj});
        }

        [HttpPatch("{JobId:int}", Name = "UpdateJob")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateJob(int JobId, [FromBody] JobUpdateDto JobDto) 
        {
            if (JobDto == null || JobId != JobDto.Id)
            {
                return BadRequest(ModelState);
            }

            var JobObj = _mapper.Map<Job>(JobDto);

            if (!_jsRepo.UpdateJob(JobObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {JobObj.Name}");
                return StatusCode(500, ModelState);

            }
            return NoContent();  
        }

        [HttpDelete("{JobId:int}", Name = "DeleteJob")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteJob(int JobId)
        {
            if (_jsRepo.JobExists(JobId))
            {
                return NotFound();
            }

            var JobObj = _jsRepo.GetJob(JobId);
            if (!_jsRepo.DeleteJob(JobObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {JobObj.Name}");
                return StatusCode(500, ModelState);

            }
            return NoContent();
        }

    }
}