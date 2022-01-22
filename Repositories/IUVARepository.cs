using UVAGraphs.Dtos;

namespace UVAGraphs.Repositories;

public interface IUVARepository : IDisposable
{
    public List<UVADto> GetAll();
}