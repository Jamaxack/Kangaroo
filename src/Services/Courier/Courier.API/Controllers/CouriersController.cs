using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Courier.API.DataTransferableObjects;
using Courier.API.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Courier.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CouriersController : ControllerBase
    {
        private readonly ICourierService _courierService;

        public CouriersController(ICourierService courierService)
        {
            _courierService = courierService ?? throw new ArgumentNullException(nameof(courierService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CourierDto>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _courierService.GetCouriersAsync());
        }

        [Route("{courierId}")]
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CourierDto), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetByIdAsync(Guid courierId)
        {
            return Ok(await _courierService.GetCourierByIdAsync(courierId));
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        public async Task<IActionResult> CreateAsync(CourierDtoSave courier)
        {
            await _courierService.InsertCourierAsync(courier);
            return Accepted();
        }

        [HttpPut]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        public async Task<IActionResult> UpdateAsync(CourierDtoSave courier)
        {
            await _courierService.UpdateCourierAsync(courier);
            return Accepted();
        }

        [Route("{courierId}/CurrentLocation")]
        [HttpGet]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(CourierLocationDto), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrentLocationByCourierIdAsync(Guid courierId)
        {
            return Ok(await _courierService.GetCurrentCourierLocationByCourierIdAsync(courierId));
        }
    }
}