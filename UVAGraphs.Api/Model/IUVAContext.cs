using Microsoft.AspNetCore.Mvc;

namespace UVAGraphs.Api.Model;
public interface IUVAContext
{   
    public List<UVA> GetUVAs();
    public float GetValue(DateTime date);
    DateTime GetDate(float value);
    float GetRise(DateTime begin, DateTime end);
}