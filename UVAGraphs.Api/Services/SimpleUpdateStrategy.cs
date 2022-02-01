namespace UVAGraphs.Api.Services;
using UVAGraphs.Api.Model;
using UVAGraphs.Api.Utils;

public class SimpleUpdateStrategy : IUpdateStrategy
{
    public int DaysForUpdate { get; }
    private int maxDownloadAttempts;

    public SimpleUpdateStrategy()
    {
        DaysForUpdate = 30;
        maxDownloadAttempts = 5;
    }
    
    public bool Execute(DateTime? lastUpdate, ref List<UVA> list)
    {
        bool result = true;        
        if (DatabaseStatusAnalyzer.ShouldUpdate(lastUpdate, DaysForUpdate))
        {
            ExecuteWithWebParser(lastUpdate, ref list);
            // result = ExecuteWithFileDownloader(lastUpdate);
            // if (result)
            // {
            //     list = ParseScrappedCsv();
            // }
            // else
            // {
            //     ExecuteWithWebParser(lastUpdate, ref list);
            // }            
        }
        return result;
    }
    
    private bool ExecuteWithWebParser(DateTime? lastUpdate, ref List<UVA> list)
    {
        bool result = true;
        using(WebParser wp = new WebParser())
        {
            try
            {
                list = wp.Parse(lastUpdate);
                if (list == null || list.Count <= 0)
                    result = false;
            }
            catch
            {
                result = false;
            }
        }
        return result;
    }


    private bool ExecuteWithFileDownloader(DateTime? lastUpdate)
    {
        int downloadAttempts = 0;
        bool successfullDownload = false;
        using (FileDownloader s = new FileDownloader())
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