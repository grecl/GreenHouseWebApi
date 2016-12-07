using System;
using System.Collections.Generic;
using System.Linq;
using GreenHouseWebApi.Dto;
using GreenHouseWebApi.Model;
using GreenHouseWebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GreenHouseWebApi.Controllers
{
    [Route("api/devicesetup/{deviceId:int}/[controller]")]
    public class WateringAreaController : Controller
    {
       private readonly IDeviceSetupRepository _deviceSetupRepository;

        public WateringAreaController(IDeviceSetupRepository deviceSetupRepository)
        {
            _deviceSetupRepository = deviceSetupRepository;
        }

        [HttpGet]
        public IActionResult GetAllWateringAreas(int deviceId)
        {
            DeviceConfiguration device = _deviceSetupRepository.GetSingle(deviceId);

            if (device == null)
            {
                return NotFound();
            }

            ICollection<WateringArea> wateringAreas = device.WateringAreas;

            if (wateringAreas == null || !wateringAreas.Any())
            {
                return NotFound();
            }

            var mappedItems = wateringAreas.Select(AutoMapper.Mapper.Map<WateringAreaDto>);
            
            return Ok(mappedItems);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetSingleWateringArea(int deviceId, int id)
        {
            DeviceConfiguration device = _deviceSetupRepository.GetSingle(deviceId);

            if (device == null)
            {
                return NotFound();
            }

            ICollection<WateringArea> wateringAreas = device.WateringAreas;

            if (wateringAreas == null || !wateringAreas.Any())
            {
                return NotFound();
            }

            var wateringArea = wateringAreas.Single(w => w.Id == id);

            if (wateringArea == null)
            {
                return NotFound();
            }
            
            return Ok(AutoMapper.Mapper.Map<WateringAreaDto>(wateringArea));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateDeviceSetup(int deviceId, int id, [FromBody] WateringAreaDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ids do not match");
            }

            DeviceConfiguration device = _deviceSetupRepository.GetSingle(deviceId);

            if (device == null)
            {
                return NotFound();
            }

            ICollection<WateringArea> wateringAreas = device.WateringAreas;

            if (wateringAreas == null || !wateringAreas.Any())
            {
                return NotFound();
            }

            var wateringArea = wateringAreas.Single(w => w.Id == id);

            if (wateringArea == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedItem = _deviceSetupRepository.UpdateWateringArea(device.Id, AutoMapper.Mapper.Map<WateringArea>(dto));
            return Ok(AutoMapper.Mapper.Map<WateringAreaDto>(updatedItem));
        }
    }
}
