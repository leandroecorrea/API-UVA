using Microsoft.AspNetCore.Mvc;

namespace UVAGraphs.Api.Model;
public class UVAContext : IUVAContext
{

    public List<UVA> GetUVAs()
    {
        return CsvToUVA.GetInstance().GetUVAs();
    }

    public float GetValue(DateTime date)
    {
        var ctx = CsvToUVA.GetInstance().GetUVAs();
        return ctx.FirstOrDefault(x=> x.Date.Equals(date)).Value;        
    }
    public DateTime GetDate(float value)
    {
        var ctx = CsvToUVA.GetInstance().GetUVAs();
        return ctx.FirstOrDefault(x=> x.Value == value).Date;
    }

    public float GetRise(DateTime begin, DateTime end)
    {
        var ctx = CsvToUVA.GetInstance().GetUVAs();
        float beginValue = this.GetValue(begin);
        float endValue = this.GetValue(end);                        
        return (beginValue - endValue) / beginValue * 100;        
    }
}