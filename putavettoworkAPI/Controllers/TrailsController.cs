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
    [Route("api/v/{version:apiVersion}/Trails")]
    //[Route("api/Trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "putavettoworkOpenAPISpec")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private iTrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(iTrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTrails() 
        {
            var objList = _trailRepo.GetTrails();

            var objDto = new List<TrailDto>();

            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get list of trail
        /// </summary>
        /// <param name="TrailId"> The ID of the trail </param>
        /// <returns></returns>
        //get request with required arguments
        [HttpGet("{TrailId:int}", Name = "GetTrail") ]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int TrailId) 
        {
            var obj = _trailRepo.GetTrail(TrailId);
            if (obj == null) 
            {
                return NotFound();
            }
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);

        }
        /// <summary>
        /// attribute routing example
        /// </summary>
        /// <param name="nationalParkId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{TrailId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepo.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDto);

        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto TrailDto) 
        {
            if (TrailDto == null) 
            {
                return BadRequest(ModelState);
            }

            if (_trailRepo.TrailExists(TrailDto.Name)) 
            {
                ModelState.AddModelError("", "trail Exists!");
                return StatusCode(404, ModelState);
            }

            var TrailObj = _mapper.Map<Trail>(TrailDto);

            if (!_trailRepo.CreateTrail(TrailObj)) 
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {TrailObj.Name}");
                return StatusCode(500, ModelState);

            }
            return CreatedAtRoute("GetTrail", new {TrailId = TrailObj.Id, TrailObj});
        }

        [HttpPatch("{TrailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int TrailId, [FromBody] TrailUpdateDto TrailDto) 
        {
            if (TrailDto == null || TrailId != TrailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var TrailObj = _mapper.Map<Trail>(TrailDto);

            if (!_trailRepo.UpdateTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {TrailObj.Name}");
                return StatusCode(500, ModelState);

            }
            return NoContent();  
        }

        [HttpDelete("{TrailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int TrailId)
        {
            if (_trailRepo.TrailExists(TrailId))
            {
                return NotFound();
            }

            var TrailObj = _trailRepo.GetTrail(TrailId);
            if (!_trailRepo.DeleteTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {TrailObj.Name}");
                return StatusCode(500, ModelState);

            }
            return NoContent();
        }

    }
}