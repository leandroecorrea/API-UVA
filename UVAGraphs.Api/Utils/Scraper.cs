namespace UVAGraphs.Api.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
public class Scraper : IDisposable
{
    private static readonly string URL_WITHOUT_FILTERS = "http://www.bcra.gov.ar/Estadisticas/EstadisSitiopublico/Proceso.aspx?selecciones=7913%3b7913%3b%7c%3bnotasMetodologicas%3bnotasDatos%3bnotasLinks&";
    private static readonly string WEBDRIVER_RUNNING_MODE = "headless";
    private static readonly string DOWNLOAD_PREFERENCE = "download.default_directory";
    public static readonly string DOWNLOAD_PATH = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar;
    private ChromeDriver browser;
    private ChromeOptions options;
    private static readonly DateTime FIRST_UVA_HISTORIC_VALUE_DATE = new DateTime(2016, 3, 31);
    

    public Scraper()
    {
        options = new ChromeOptions();
        options.PageLoadStrategy = PageLoadStrategy.Eager;
        options.AddArgument(WEBDRIVER_RUNNING_MODE);
        options.AddUserProfilePreference(DOWNLOAD_PREFERENCE, DOWNLOAD_PATH);
        browser = new ChromeDriver(options);
        Cleaner.Clean();
    }
    public bool DownloadFile(DateTime? beginDate)
    {        
        (DateTime from, DateTime to) = SetDates(beginDate);
        var url = SetUrl(from, to);
        browser.Navigate().GoToUrl(url);
        WebDriverWait wait = new WebDriverWait(browser, TimeSpan.FromMinutes(1));
        Func<IWebDriver, bool> WaitForElement = new Func<IWebDriver, bool>((IWebDriver b) =>
            {
                return b.FindElement(By.Id("DropDownList1")).Enabled;
            });
        var result = wait.Until(WaitForElement);
        if(!result)
            return false;            
        browser.FindElement(By.Id("DropDownList1")).SendKeys("Texto");
        browser.FindElement(By.Id("btnAcept")).Click();        
        bool fileDownloaded = CheckForFile();
        if(fileDownloaded) 
            browser.Quit();
        return fileDownloaded;
    }

    private bool CheckForFile()
    {         
        return System.IO.File.Exists(DOWNLOAD_PATH + FileParser.FILENAME);       
    }

    public (DateTime from, DateTime to) SetDates(DateTime? beginDate)
    {
        DateTime from;
        if(beginDate == null)
            from = FIRST_UVA_HISTORIC_VALUE_DATE;
        else
            from = (DateTime)beginDate;
        DateTime to = DateTime.Today;
        return (from, to);
    }  

    public string SetUrl(DateTime beginDate, DateTime endDate)
    {
        var filterEndpointString = $"fechaDesde={beginDate.ToFixedString()}&fechaHasta={endDate.ToFixedString()}";
        return URL_WITHOUT_FILTERS + filterEndpointString;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}