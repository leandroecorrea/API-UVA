namespace UVAGraphs.Model;
public class UVAContext : IUVAContext
{    
    public List<UVA> GetUVAs()
    {
        return CsvToUVA.GetInstance().GetUVAs();
    }
}