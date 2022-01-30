namespace UVAGraphs.Api.Utils;

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using UVAGraphs.Api.Model;

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
        // browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
        Cleaner.Clean();
    }
    public bool DownloadFile(DateTime? beginDate)
    {
        (DateTime from, DateTime to) = SetDates(beginDate);
        var url = SetUrl(from, to);
        System.Console.WriteLine(url);
        browser.Navigate().GoToUrl(url);
        WebDriverWait wait = new WebDriverWait(browser, TimeSpan.FromMinutes(1));
        Func<IWebDriver, bool> WaitForDropdown = new Func<IWebDriver, bool>((IWebDriver b) =>
            {
                return b.FindElement(By.Id("DropDownList1")).Enabled;
            });
        Func<IWebDriver, bool> WaitForBtn = new Func<IWebDriver, bool>((IWebDriver b) =>
            {
                return b.FindElement(By.Id("DropDownList1")).Enabled;
            });

        var dropDown = wait.Until(WaitForDropdown);
        if (!dropDown)
            return false;
        var btnAccept = wait.Until(WaitForBtn);
        if (!btnAccept)
            return false;

        var table = browser.FindElement(By.Id("GrdResultados"));

        var rows = table.FindElements(By.TagName("tr"));
        var list = new List<UVA>();

        
        var indexRow = table.FindElement(By.XPath("//tr[82]/td/table/tbody/tr"));
        var indexes = indexRow.FindElements(By.TagName("td"));
        NextPage(indexes);

        browser.FindElement(By.Id("DropDownList1")).SendKeys("Texto");
        browser.FindElement(By.Id("btnAcept")).SendKeys(Keys.Enter);
        bool fileDownloaded = CheckForFile();
        if (fileDownloaded)
            browser.Quit();
        return fileDownloaded;
    }

    private bool NextPage(ReadOnlyCollection<IWebElement> pages)
    {
        var pagesList = pages.ToList();
        int? index = null;
        foreach(var page in pagesList)
        {
            if(page.FindElement(By.TagName("span")) != null)
            {
                index = pagesList.FindIndex(p => p.Location == page.Location);
            }            
        }
        System.Console.WriteLine(index == null? "null" : index);
        // var currentPage = pages.FindElement(By.TagName("span"));
        // var availablePages = pages.FindElements(By.TagName("a"));
        return true;
    }

    private bool ParseRow(IWebElement row, ref UVA uva)
    {
        var cells = row.FindElements(By.TagName("td"));
        DateTime date;
        if (cells.Count > 0 && DateTime.TryParse(cells[0].Text, out date) && Single.TryParse(cells[1].Text, out float value))
        {
            uva = new UVA{
                Date = date,
                Value = value
            };
            return true;
        }
        return false;

    }

    private bool CheckForFile()
    {
        return System.IO.File.Exists(DOWNLOAD_PATH + FileParser.FILENAME);
    }

    public (DateTime from, DateTime to) SetDates(DateTime? beginDate)
    {
        DateTime from;
        if (beginDate == null)
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