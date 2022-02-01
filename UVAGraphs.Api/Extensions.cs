using UVAGraphs.Api.Dtos;
using UVAGraphs.Api.Model;
public static class Extensions
{
    public static string ToFixedString(this DateTime dateTime)
    {
        string month = Convert.ToString(dateTime.Month);
        month = month.Length < 2? "0" + month : month;
        string day = Convert.ToString(dateTime.Day);
        day = day.Length < 2? "0" + day : day;
        string year = Convert.ToString(dateTime.Year);
        string slash = "%2f";        
        return $"{day}{slash}{month}{slash}{year}";
    }

    public static string TrimNumberForFloat(this string number)
    {
        char floatingPointChar = '.';
        int totalDecimals = 2;
        if(number.Contains(floatingPointChar))
        {
            var index = number.IndexOf(floatingPointChar);
            if(index > -1)
            {
                number = number.Substring(0, index + totalDecimals);
            }
        }
        return number;
    }
    public static UVADto asDto(this UVA uva)
    {
        return new UVADto{
            Date = uva.Date,
            Value = uva.Value
        };
    }
}