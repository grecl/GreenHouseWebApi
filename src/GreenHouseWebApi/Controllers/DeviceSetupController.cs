using System;
using System.Collections.Generic;
using System.Linq;
using GreenHouseWebApi.Dto;
using GreenHouseWebApi.Model;
using GreenHouseWebApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GreenHouseWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DeviceSetupController : Controller
    {
        private readonly IDeviceSetupRepository _deviceSetupRepository;

        public DeviceSetupController(IDeviceSetupRepository deviceSetupRepository)
        {
            _deviceSetupRepository = deviceSetupRepository;
        }

        [HttpGet]
        public IActionResult GetAllDeviceSetups()
        {
            ICollection<DeviceConfiguration> devices = _deviceSetupRepository.GetAll();

            var mappedItems = devices.Select(AutoMapper.Mapper.Map<DeviceSetupDto>);

            return Ok(mappedItems);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetSingleDeviceSetup(int id)
        {
            var deviceSetup = _deviceSetupRepository.GetSingle(id);

            if (deviceSetup == null)
            {
                return NotFound();
            }

            return Ok(AutoMapper.Mapper.Map<DeviceSetupDto>(deviceSetup));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateDeviceSetup(int id, [FromBody] FoodItemDto dto)
        {
            DeviceSetup deviceSetupToCheck = _deviceSetupRepository.GetSingle(id);
            if (deviceSetupToCheck == null)
            {
                return NotFound();
            }
            if (id != dto.Id)
            {
                return BadRequest("ids do not match");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedFoodItem = _deviceSetupRepository.Update(AutoMapper.Mapper.Map<DeviceSetup>(dto));
            return Ok(AutoMapper.Mapper.Map<FoodItemDto>(updatedFoodItem));
        }
    }
}
