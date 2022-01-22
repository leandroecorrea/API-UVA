namespace UVAGraphs.Model;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System;
public class CsvToUVA
{
    private static CsvToUVA instance;

    private CsvToUVA()
    {
        
    }

    public static CsvToUVA GetInstance()
    {
        if(instance == null)
            instance = new CsvToUVA();
        return instance;
    }
    
    
    public List<UVA> GetUVAs()
    {
        var path = "./estadis.csv";
        List<UVA> UVAs = new List<UVA>();
        using(TextFieldParser csvParser = new TextFieldParser(path))
        {
            csvParser.SetDelimiters(new string[] { ";" });
            csvParser.ReadLine();            
            var culture = new CultureInfo("en-US");    
            while (!csvParser.EndOfData)
            {             
             string[]? fields = csvParser.ReadFields();
             float value;
             DateTime date;
             if(DateTime.TryParse(fields[0], culture, DateTimeStyles.None, out date) && Single.TryParse(fields[1]?.Replace('.', ','), out value))
             {                
                UVAs.Add(new UVA{
                    Date = date,
                    Value = value
                });               
             }
            }            
        }
        return UVAs;
    }
}