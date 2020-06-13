using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Delivery.API.Application.Queries;
using Delivery.API.Application.Queries.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientQueries _clientQueries;

        public ClientsController(IClientQueries clientQueries)
        {
            _clientQueries = clientQueries ?? throw new ArgumentNullException(nameof(clientQueries));
        }

        //GET ~/api/v1/[controller]/[?pageSize=12&pageIndex=7]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClientViewModel>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            return Ok(await _clientQueries.GetClientsAsync(pageSize, pageIndex));
        }
    }
}