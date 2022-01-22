using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UVAGraphs.Services;
using UVAGraphs.Dtos;

namespace UVAGraphs._Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UVAController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUVAServices _services;

        public UVAController(ILogger<UVAController> logger, IUVAServices services)
        {
            _logger= logger;
            _services = services;
        }

        [HttpGet]
        public IEnumerable<UVADto> Get()
        {
            return _services.GetAll();
        }
    }
}