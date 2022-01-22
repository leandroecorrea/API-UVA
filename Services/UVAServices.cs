namespace UVAGraphs.Services;
using System.Collections.Generic;
using UVAGraphs.Dtos;
using UVAGraphs.Repositories;

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
}