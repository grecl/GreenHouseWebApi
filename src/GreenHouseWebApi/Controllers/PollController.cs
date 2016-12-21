using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenHouseWebApi.BusinessService.Operations;
using Microsoft.AspNetCore.Mvc;

namespace GreenHouseWebApi.Controllers
{
    [Route("api/poll/{deviceId:int}")]
    public class PollController : Controller
    {
        private readonly IDevicePollingService _devicePollingService;

        public PollController(IDevicePollingService devicePollingService)
        {
            _devicePollingService = devicePollingService;
        }

        [HttpPost]
        public IActionResult ExecuteDevicePolling(int deviceId)
        {
            //Todo: poll the device with the id
            _devicePollingService.PollDevice();

            return Ok();
        }
    }
}
