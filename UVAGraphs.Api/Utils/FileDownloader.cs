using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace UVAGraphs.Api.Utils;
public class FileDownloader : Scraper
{
    private static readonly string DOWNLOAD_PREFERENCE = "download.default_directory";
    public static readonly string DOWNLOAD_PATH = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;

    public FileDownloader()
    {
        base.options = new ChromeOptions();
        options.PageLoadStrategy = PageLoadStrategy.Eager;
        // options.AddArgument(WEBDRIVER_RUNNING_MODE);
        options.AddArgument("no-sandbox");
        options.AddUserProfilePreference(DOWNLOAD_PREFERENCE, DOWNLOAD_PATH);
        browser = new ChromeDriver(options);  
        browser.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(10);              
        browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(10);
        Cleaner.Clean();
    }
    public bool DownloadFile(DateTime? beginDate)
    {
        try
        {
        (DateTime from, DateTime to) = SetDates(beginDate);
        var url = SetUrl(from, to);
        System.Console.WriteLine(url);
        browser.Navigate().GoToUrl(url);
        WebDriverWait wait = new WebDriverWait(browser, TimeSpan.FromMinutes(5));
        Func<IWebDriver, bool> WaitForDropdown = new Func<IWebDriver, bool>((IWebDriver b) =>
            {
                return b.FindElement(By.XPath("//*[@id='DropDownList1']")) != null;                
            });
        Func<IWebDriver, bool> WaitForBtn = new Func<IWebDriver, bool>((IWebDriver b) =>
            {
                return b.FindElement(By.XPath("//*[@id='btnAcept']")) != null;                
                
            });            
        var dropDownIsEnabled = wait.Until(WaitForDropdown);
        if (!dropDownIsEnabled)
            return false;
        var btnAcceptIsEnabled = wait.Until(WaitForBtn);
        if (!btnAcceptIsEnabled)
            return false;             
        browser.FindElement(By.XPath("//*[@id='DropDownList1']")).SendKeys("texto");
        browser.FindElement(By.XPath("//*[@id='btnAcept']")).SendKeys(Keys.Enter);
        bool fileDownloaded = CheckForFile();
        if (fileDownloaded)
            browser.Quit();
        return fileDownloaded;
        }
        catch(Exception e)
        {
            System.Console.WriteLine(e.StackTrace);
            bool fileDownloaded = CheckForFile();
            if (fileDownloaded)
                browser.Quit();
            return fileDownloaded;
        }
    }
    private bool CheckForFile()
    {
        return System.IO.File.Exists(DOWNLOAD_PATH + FileParser.FILENAME);
    }

    

}

