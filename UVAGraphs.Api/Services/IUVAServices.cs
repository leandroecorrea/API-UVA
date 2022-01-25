using Microsoft.AspNetCore.Mvc;
using UVAGraphs.Api.Dtos;

namespace UVAGraphs.Api.Services;
public interface IUVAServices
{
    public List<UVADto> GetAll();
    float GetValueFromDate(DateTime date);
    DateTime GetDateFromValue(float value);
   float GetRise(DateTime begin, DateTime end);
}