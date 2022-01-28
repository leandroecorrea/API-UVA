namespace UVAGraphs.Api.Utils;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System;
using UVAGraphs.Api.Model;

public class FileParser : IDisposable
{    
    public static readonly string FILENAME = "Estadis.csv";

    public List<UVA> ParseFile()
    {        
        List<UVA> UVAs = new List<UVA>();                     
        try
        {
            using (TextFieldParser csvParser = new TextFieldParser(Scraper.DOWNLOAD_PATH + FILENAME))
            {
                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.ReadLine();
                var culture = new CultureInfo("en-US");
                while (!csvParser.EndOfData)
                {
                    string[]? fields = csvParser.ReadFields();
                    float value;
                    DateTime date;
                    if (DateTime.TryParse(fields?[0], culture, DateTimeStyles.None, out date) && Single.TryParse(fields[1], out value))
                    {
                        UVAs.Add(new UVA
                        {
                            Date = date,
                            Value = value
                        });
                    }
                }                
                csvParser.Close();
            }
        }
        catch (Exception e)
        {
            System.Console.WriteLine("Exception thrown: " + e.Message + "///" + e.StackTrace);            
        }    
        return UVAs;
    }

    protected virtual bool IsFileBeingUsed(FileInfo file)
    {
        try
        {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (IOException)
        {            
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}