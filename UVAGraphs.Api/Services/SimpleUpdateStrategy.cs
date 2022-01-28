namespace UVAGraphs.Api.Services;
using UVAGraphs.Api.Enums;
using UVAGraphs.Api.Model;
using UVAGraphs.Api.Utils;

public class SimpleUpdateStrategy : IUpdateStrategy
{
    public int DaysForUpdate { get; }
    private DatabaseStatus _databaseStatus;

    private int maxDownloadAttempts;

    public SimpleUpdateStrategy()
    {
        DaysForUpdate = 30;
        maxDownloadAttempts = 5;
    }

    private void SetDatabaseStatus(DateTime? lastUpdate)
    {
        if (lastUpdate == null)
        {
            _databaseStatus = DatabaseStatus.Empty;
            System.Console.WriteLine(_databaseStatus.ToString());
        }
        else if (lastUpdate?.AddDays(DaysForUpdate) < DateTime.Now)
        { 
            _databaseStatus = DatabaseStatus.Outdated; 
            System.Console.WriteLine(_databaseStatus.ToString());
        }
        else 
        { 
            _databaseStatus = DatabaseStatus.UpToDate; 
            System.Console.WriteLine(_databaseStatus.ToString());
        }
    }

    public bool ShouldUpdate(DateTime? lastUpdate)
    {
        SetDatabaseStatus(lastUpdate);
        return _databaseStatus == DatabaseStatus.Empty || _databaseStatus == DatabaseStatus.Outdated;
    }

    public bool Execute(DateTime? lastUpdate, ref List<UVA> list)
    {
        bool result = true;
        int downloadAttempts = 0;
        if (ShouldUpdate(lastUpdate))
        {
            bool successfullDownload = false;
            using(Scraper s = new Scraper())
            {
                while(downloadAttempts < maxDownloadAttempts && !successfullDownload)
                {
                    successfullDownload = s.DownloadFile(lastUpdate);
                }
                downloadAttempts++;
            }
            result = successfullDownload;
            var resultString =  successfullDownload? "descarga exitosa" : "error en la descarga";
            System.Console.WriteLine("Intentos de descarga realizados: " + downloadAttempts + "/" + maxDownloadAttempts + "\n" + resultString);
            if (successfullDownload)
            {
                using (FileParser f = new FileParser())
                {
                    list = f.ParseFile();
                }                
            }
        }
        return result;
    }
}