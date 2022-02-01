namespace UVAGraphs.Api.Utils;
using System.IO;
public static class Cleaner
{
    public static void Clean()
    {
        var files = Directory.GetFiles(FileDownloader.DOWNLOAD_PATH);
        files.ToList().ForEach(f => {
            File.Delete(f);
            System.Console.WriteLine(f);
            });
    }
}