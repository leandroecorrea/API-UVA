using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UVAGraphs.Api.Services;
using UVAGraphs.Api.Dtos;

namespace UVAGraphs.Api._Controllers
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

        [HttpGet]
        public ActionResult<float> GetValue(string date)
        {
            DateTime dateTime;            
            if(!DateTime.TryParse(date, out dateTime) || !ModelState.IsValid) return BadRequest(ModelState);            
            return _services.GetValueFromDate(dateTime);
        }

        [HttpGet]
        public ActionResult<DateTime> GetDate(float value)
        {            
            if(!ModelState.IsValid) return BadRequest();
            return _services.GetDateFromValue(value);
        }

        [HttpGet]
        public ActionResult<float> GetRise(string beginDate, string endDate)
        {          
            
            DateTime begin;
            DateTime end;
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if(!(DateTime.TryParse(beginDate, out begin)) || !(DateTime.TryParse(endDate, out end))) return BadRequest(ModelState);                        
            if(begin > end) return BadRequest("La fecha de inicio es posterior a la de finalización");
            return Ok(9);
            return _services.GetRise(begin, end);
        }

    }
}