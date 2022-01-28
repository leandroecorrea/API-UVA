using UVAGraphs.Api.Model;

namespace UVAGraphs.Api.Repositories
{
    public interface IUpdatable
    {
        DateTime? LatestRecord();
        void InsertUVA(UVA uva);
        void Save();
    }
}