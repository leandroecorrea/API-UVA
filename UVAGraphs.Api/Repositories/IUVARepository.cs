using Microsoft.AspNetCore.Mvc;
using UVAGraphs.Api.Dtos;

namespace UVAGraphs.Api.Repositories;

public interface IUVARepository
{
    public List<UVADto> GetAll();
    float GetValueFromDate(DateTime uvaDate);
    DateTime GetDateFromValue(float value);
    float GetRise(DateTime begin, DateTime end);
}