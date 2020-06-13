using Microsoft.AspNetCore.Mvc;
using Pricing.API.DataTransferableObjects;
using Pricing.API.Infrastucture.Services;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Pricing.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
        }

        [HttpPost]
        [ProducesResponseType(typeof(PriceDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CalculatePriceAsync(CalculatePriceDto calculatePriceDto)
            => Ok(await _pricingService.CalculatePriceAsync(calculatePriceDto));
    }
}
