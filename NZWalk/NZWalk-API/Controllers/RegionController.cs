﻿#region using


using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalk_API.CustomActionFilter;
using NZWalk_API.Model.Domain;
using NZWalk_API.Model.DTO;
using NZWalk_API.Repositories.Interface;
#endregion
namespace NZWalk_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : Controller
    {
        #region CTOR

        private readonly IRegionRepository _regionRepository;
        private readonly IMapper mapper;

        public RegionController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            this.mapper = mapper;
        }
        #endregion

        #region Get all Region
        [HttpGet]
        [Authorize(Roles = "Writer,Reader")]

        public async Task<IActionResult> GetAll()
        {
            //get data from database - Domain Model 
            var regions = await _regionRepository.GetAllAsync();
            //Map domain model to DTO
            var RegionDto = mapper.Map<List<RegionDto>>(regions);
            return Ok(RegionDto);
        }
        #endregion

        #region By ID
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]

        public async Task<IActionResult> GetById(Guid id)
        {
            var Region = await _regionRepository.GetByIdAsync(id);
            if (Region is not null)
            {
                return Ok(mapper.Map<RegionDto>(Region));
            }
            else
            {
                var error = id + " Not Found ";
                return NotFound(error);

            }

        }
        #endregion

        #region Create
        [HttpPost]
        [ValidateModel]// Coustom Validater 
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {

            if (addRegionRequestDTO is null)
            {

                return BadRequest(addRegionRequestDTO);
            }

            var RegionDomainModel = mapper.Map<RegionDto>(addRegionRequestDTO);
            RegionDomainModel = await _regionRepository.CreateAsync(RegionDomainModel);
            var RegionDto = (mapper.Map<RegionDto>(RegionDomainModel));
            return Ok(RegionDto);


        }
        #endregion

        #region Update
        [HttpPut]
        [Route("{id:Guid}")]
        [ActionName("Update")]
        [ValidateModel]// Coustom Validater 
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {

            //Map DTO To Domain Model
            var RegionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);

            await _regionRepository.UpdateAsync(id, RegionDomainModel);

            //Convert Domain TO DTO
            var regionDTO = mapper.Map<RegionDto>(RegionDomainModel);
            return Ok(regionDTO);

        }
        #endregion

        #region Delete
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var ExistingRegion = await _regionRepository.DeleteAsync(id);
            if (ExistingRegion != null)
            {
                //return Delete Model back
                //map Domain Model To DTO
                var regionDTO = mapper.Map<RegionDto>(ExistingRegion);
                return Ok(regionDTO);
            }
            return BadRequest();
        }
        #endregion
    }
}
