using Microsoft.AspNetCore.Mvc;
using UVAGraphs.Api.Dtos;
using UVAGraphs.Api.Model;

namespace UVAGraphs.Api.Repositories;

public interface IUVARepository
{
    public List<UVADto> GetAll();
    float? GetValueFromDate(DateTime uvaDate);    
    (float?, float?) GetValuesForRise(DateTime begin, DateTime end);    
}