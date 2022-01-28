namespace UVAGraphs.Api.Repositories;
using UVAGraphs.Api.Dtos;
using UVAGraphs.Api.Model;
using System;
using System.Linq;

public class UVARepository : IUVARepository, IUpdatable
{
    private UVAContext _context;

    public UVARepository(UVAContext context)
    {
        _context = context;
    }

    public List<UVADto> GetAll()
    {
        return _context.Set<UVA>().Select(u => u.asDto()).ToList();
    }

    public (float?, float?) GetValuesForRise(DateTime begin, DateTime end)
    {
        return (this.GetValueFromDate(begin), this.GetValueFromDate(end));
    }
    //El test debería verificar qué devuelve si el Find devuelve null (expected = null)
    public float? GetValueFromDate(DateTime uvaDate)
    {
        return _context.Set<UVA>().Find(uvaDate)?.Value ?? null;
    }
    public void Save()
    {
        _context.SaveChanges();
    }
    public void InsertUVA(UVA uva)
    {
        if (_context.Set<UVA>().Where(u => u.Date == uva.Date) != null) _context.Set<UVA>().Add(uva);
    }

    public DateTime? LatestRecord()
    {
        if(_context.Set<UVA>().Count() > 0) 
            return _context.Set<UVA>().Max(u => u.Date);        
        return null;
        
    }
}