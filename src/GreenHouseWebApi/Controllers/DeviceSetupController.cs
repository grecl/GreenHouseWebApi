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
            ICollection<DeviceSetup> devices = _deviceSetupRepository.GetAll();

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
    }
}
