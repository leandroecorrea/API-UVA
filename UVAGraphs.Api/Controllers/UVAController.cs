using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UVAGraphs.Api.Repositories;
using UVAGraphs.Api.Dtos;

namespace UVAGraphs.Api._Controllers
{    
    [ApiController]
    public class UVAController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IUVARepository _repository;

        public UVAController(ILogger<UVAController> logger, IUVARepository repository)
        {
            _logger= logger;
            _repository = repository;
        }

        [HttpGet("[controller]")]
        public IEnumerable<UVADto> Get()
        {
            return _repository.GetAll();
        }

        [HttpGet("[controller]/filter")]
        public ActionResult<float?> GetValue([FromQuery(Name = "fecha")]string date)
        {
            DateTime dateTime;              
            if(!DateTime.TryParse(date, out dateTime) || !ModelState.IsValid) return BadRequest(ModelState);            
            // return _repository.GetValueFromDate(dateTime);
            float? result = _repository.GetValueFromDate(dateTime);            
            return result != null ? Ok(result) : NotFound(result);
        }

        [HttpGet("[controller]/interes")]
        public ActionResult<float?> GetRise([FromQuery(Name = "desde")]string beginDate, [FromQuery(Name = "hasta")] string endDate)
        {   
            DateTime begin;
            DateTime end;
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if(!(DateTime.TryParse(beginDate, out begin)) || !(DateTime.TryParse(endDate, out end))) return BadRequest(ModelState);                        
            if(begin > end) return BadRequest("La fecha de inicio es posterior a la de finalización");
            (float? firstValue, float? secondValue) = _repository.GetValuesForRise(begin, end);        
            return RiseValidator(firstValue, secondValue);
        }

        private ActionResult<float?> RiseValidator(float? first, float? second)
        {
            if(first == null || second == null)
                return NotFound(null);                              
            return Ok(RiseCalculator((float)first, (float)second));
            
        }
        private float RiseCalculator(float first, float second)
        {
            return (second - first) / first * 100;
        }
    }
}