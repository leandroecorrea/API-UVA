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
        string[] lines = File.ReadAllLines(FileDownloader.DOWNLOAD_PATH + FILENAME);
        char separatoryChar = ';';                
        foreach(var line in lines)
        {            
            var entry = line.TrimEnd(separatoryChar).Split(separatoryChar);
            if(entry.Length == 2)
            {
                DateTime date;
                float value;
                string stringDate = entry[0];
                string stringValue = entry[1].TrimNumberForFloat();
                if(DateTime.TryParse(stringDate, out date) && Single.TryParse(stringValue, out value))
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