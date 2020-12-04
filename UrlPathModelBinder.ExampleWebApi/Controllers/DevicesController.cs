using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using SampleWebApi.Models;
using UrlPathModelBinder;

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
        public ActionResult<CollectionResponse<Device>> Query([FromQuery] DeviceRequest request)
        {
            return QueryImpl(request);
        }

        [Route("search/{*args}")]
        public ActionResult<CollectionResponse<Device>> Search([ModelBinder(typeof(PathModelBinder<DeviceRequest>))] DeviceRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return QueryImpl(request);
        }

        private ActionResult<CollectionResponse<Device>> QueryImpl(DeviceRequest request)
        {
            // Limit maximum of items to prevent exploits:
            const int maxItems = 100;
            request.PageSize = Math.Min(maxItems, request.PageSize);

            var q = _devices.AsQueryable();

            if (!string.IsNullOrEmpty(request.Id)) q = q.Where(d => d.Id == request.Id);
            if (!string.IsNullOrEmpty(request.Airport)) q = q.Where(d => d.Airport == request.Airport);
            if (!string.IsNullOrEmpty(request.Terminal)) q = q.Where(d => d.Terminal == request.Terminal);
            if (!string.IsNullOrEmpty(request.Type)) q = q.Where(d => d.Type == request.Type);

            return new CollectionResponse<Device>(request, q);
        }

    }
}
