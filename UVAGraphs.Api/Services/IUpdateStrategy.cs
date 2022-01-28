using UVAGraphs.Api.Model;

namespace UVAGraphs.Api.Services;
public interface IUpdateStrategy
{
    bool Execute(DateTime? lastUpdate, ref List<UVA> list);
    int DaysForUpdate { get; }
}