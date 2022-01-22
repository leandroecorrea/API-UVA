using UVAGraphs.Dtos;
using UVAGraphs.Model;
public static class Extensions
{
    public static UVADto asDto(this UVA uva)
    {
        return new UVADto{
            Date = uva.Date,
            Value = uva.Value
        };
    }
}