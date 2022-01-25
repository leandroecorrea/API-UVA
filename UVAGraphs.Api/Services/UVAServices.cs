namespace UVAGraphs.Api.Services;
using System.Collections.Generic;
using UVAGraphs.Api.Dtos;
using UVAGraphs.Api.Repositories;

public class UVAServices : IUVAServices
{
    private readonly IUVARepository _repository;

    public UVAServices(IUVARepository repository)
    {
        _repository = repository;
    }
    public List<UVADto> GetAll()
    {
        return _repository.GetAll();
    }

    public DateTime GetDateFromValue(float value)
    {
        return _repository.GetDateFromValue(value);
    }

    public float GetRise(DateTime begin, DateTime end)
    {
        return _repository.GetRise(begin, end);
    }

    public float GetValueFromDate(DateTime uvaDate)
    {        
        float value;            
        value = _repository.GetValueFromDate(uvaDate);
        return value;
    }
}