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
    [Route("api/v/{version:apiVersion}/nationalParks")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "putavettoworkOpenAPISpec")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class JobSearchV2Controller : ControllerBase
    {
        private iNationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public JobSearchV2Controller(iNationalParkRepository npRepo, IMapper mapper)
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

        /// <summary>
        /// Get list of national park
        /// </summary>
        /// <param name="nationalParkId"> The ID of the National Park </param>
        /// <returns></returns>
        //get request with required arguments
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark") ]
        [ProducesResponseType(200, Type = typeof(JobSearchDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId) 
        {
            var obj = _npRepo.GetNationalPark(nationalParkId);
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
        public IActionResult CreateNationalPark([FromBody] JobSearchDto nationalParkDto) 
        {
            if (nationalParkDto == null) 
            {
                return BadRequest(ModelState);
            }

            if (_npRepo.NationalParkExists(nationalParkDto.Name)) 
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }

            var nationalParkObj = _mapper.Map<JobSearch>(nationalParkDto);

            if (!_npRepo.CreateNationalPark(nationalParkObj)) 
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);

            }
            return CreatedAtRoute("GetNationalPark", new {nationalParkId = nationalParkObj.Id, nationalParkObj});
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] JobSearchDto nationalParkDto) 
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<JobSearch>(nationalParkDto);

            if (!_npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);

            }
            return NoContent();  
        }

        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (_npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationalParkObj = _npRepo.GetNationalPark(nationalParkId);
            if (!_npRepo.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);

            }
            return NoContent();
        }

    }
}