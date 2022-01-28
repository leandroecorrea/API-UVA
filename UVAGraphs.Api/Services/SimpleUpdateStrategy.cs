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
    // TODO: este método podría delegarse a la clase DatabaseAnalyst 
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
    //Y si paso por referencia al Analyst la databaseStatus y me devuelve el should update?
    public bool ShouldUpdate(DateTime? lastUpdate)
    {
        SetDatabaseStatus(lastUpdate);
        return _databaseStatus == DatabaseStatus.Empty || _databaseStatus == DatabaseStatus.Outdated;
    }
    
    public bool Execute(DateTime? lastUpdate, ref List<UVA> list)
    {
        bool result = true;        
        if (ShouldUpdate(lastUpdate))
        {
            result = ExecuteWithScraper(lastUpdate);
            if (result)
            {
                list = ParseScrappedCsv();
            }
        }
        return result;
    }
    private bool ExecuteWithScraper(DateTime? lastUpdate)
    {
        int downloadAttempts = 0;
        bool successfullDownload = false;
        using (Scraper s = new Scraper())
        {
            while (downloadAttempts < maxDownloadAttempts && !successfullDownload)
            {
                //Si lastUpdate es null, el Scraper usa la fecha de inicio histórica
                successfullDownload = s.DownloadFile(lastUpdate);
            }
            downloadAttempts++;
        }
        return successfullDownload;
    }
    private List<UVA> ParseScrappedCsv()
    {
        using (FileParser f = new FileParser())
        {
            return f.ParseFile();
        }
    }
}