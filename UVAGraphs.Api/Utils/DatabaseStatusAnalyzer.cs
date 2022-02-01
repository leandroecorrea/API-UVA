using UVAGraphs.Api.Enums;
namespace UVAGraphs.Api.Utils;

public static class DatabaseStatusAnalyzer
{
    private static DatabaseStatus Analyze(DateTime? lastUpdate, int days)
    {
        if (lastUpdate == null) return DatabaseStatus.Empty;                    
        if (lastUpdate?.AddDays(days) < DateTime.Now) return DatabaseStatus.Outdated;
        return DatabaseStatus.UpToDate;        
    }
    public static bool ShouldUpdate(DateTime? lastUpdate, int days)
    {
        return Analyze(lastUpdate, days) != DatabaseStatus.UpToDate;
    }
}