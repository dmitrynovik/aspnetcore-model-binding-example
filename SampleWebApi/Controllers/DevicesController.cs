using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using SampleWebApi.Models;

namespace SampleWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ICollection<Device> _devices;

        public DevicesController()
        {
            // Generate some dummy data:
            _devices = Enumerable.Range(1, 100)
                .Select(i => new Device
                {
                    Airport = i < 60 ? "SYD" : "LHR",
                    Id = i.ToString(),
                    Terminal = (i % 60 / 20 + 1).ToString(), // 1, 2, 3 ...
                    Type = "Unit"
                })
                .ToArray();
        }

        // GET /devices/1
        /// <returns>A single resource accessed by short, immutable path, or 404 if N/A. The path/to/resource should not change</returns>
        [HttpGet("{id}")]
        public ActionResult<Device> Get(string id)
        {
            var device = _devices.FirstOrDefault(d => d.Id == id);
            if (device == null)
                return NotFound(); // 404

            return device;
        }

        // GET /devices?Airport=SYD
        // GET /devices?Airport=SYD&Terminal=2
        // GET /devices?Airport=SYD&Terminal=2&Type=Unit
        // ... any combination
        // + supports pagination e.g. /devices?Airport=SYD?page=2&pageSize=10
        /// <returns>A collection of devices filtered by multiple attributes.</returns>
        [HttpGet("")]
        public ActionResult<CollectionResponse<Device>> Query([FromQuery] DeviceRequest location)
        {
            // Limit maximum of items to prevent exploits:
            const int maxItems = 100;

            var pageSize = Math.Min(maxItems, location.PageSize);

            var q = _devices.AsQueryable();

            if (!string.IsNullOrEmpty(location.Id)) q = q.Where(d => d.Id == location.Id);
            if (!string.IsNullOrEmpty(location.Airport)) q = q.Where(d => d.Airport == location.Airport);
            if (!string.IsNullOrEmpty(location.Terminal)) q = q.Where(d => d.Terminal == location.Terminal);
            if (!string.IsNullOrEmpty(location.Type)) q = q.Where(d => d.Type == location.Type);

            return new CollectionResponse<Device>(location.Page, 
                q.Count(), 
                q.Skip((location.Page - 1) * pageSize).Take(pageSize));
        }
    }
}
