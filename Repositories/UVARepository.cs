using UVAGraphs.Dtos;
using UVAGraphs.Model;

namespace UVAGraphs.Repositories;
using System.Linq;

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
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}