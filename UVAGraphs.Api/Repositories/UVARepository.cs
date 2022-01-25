using UVAGraphs.Api.Dtos;
using UVAGraphs.Api.Model;

namespace UVAGraphs.Api.Repositories;

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

public class UVARepository : IUVARepository
{
    private IUVAContext _context;
    public UVARepository(IUVAContext context)
    {
        _context = context;
    }
    public List<UVADto> GetAll()
    {        
        return _context.GetUVAs().Select(x=> x.asDto()).ToList();
    }

    public DateTime GetDateFromValue(float value)
    {
        return _context.GetDate(value);
    }

    public float GetRise(DateTime begin, DateTime end)
    {
        return _context.GetRise(begin, end);
    }

    public float GetValueFromDate(DateTime uvaDate)
    {
        return _context.GetValue(uvaDate);
    }
}