using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using UVAGraphs.Api.Model;

namespace UVAGraphs.Api.Utils;

public class WebParser : Scraper
{
    private HashSet<string> parsedPages = new HashSet<string>();    

    public WebParser()
    {
        base.options = new ChromeOptions();
        options.PageLoadStrategy = PageLoadStrategy.Eager;
        // options.AddArgument(WEBDRIVER_RUNNING_MODE); 
        options.AddArgument("no-sandbox");  
        browser = new ChromeDriver(options);                
        // browser.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(10);
        // browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(10);
        Cleaner.Clean();
    }

    public List<UVA> Parse(DateTime? beginDate)
    {
        (DateTime from, DateTime to) = SetDates(beginDate);
        var url = SetUrl(from, to);
        browser.Navigate().GoToUrl(url);
        
        var parsedResult = ParseAllPages().ToList();
        browser.Quit();
        return parsedResult;
    }
    private ReadOnlyCollection<IWebElement> GetPages()
    {
        var pages = browser.FindElements(By.XPath("//tr[82]/td/table/tbody/tr/td"));
        return pages;
    }

    public IEnumerable<UVA> ParseAllPages()
    {
        var pages = GetPages();
        var list = new List<UVA>();
        for (int i = 0; i < pages.Count; i++)
        {
            pages = GetPages();
            var currentPage = pages[i];
            var text = currentPage.Text;            
            if (text == "..." && i == pages.Count -1)
            {                
                currentPage.Click();
                GetPages().ToList().ForEach(p => System.Console.WriteLine(p.Text));
                list.AddRange(ParseAllPages());
            }
            else if(text == "..." || parsedPages.Contains(text))
            {
                continue;
            }
            else
            {
                currentPage.Click();   
                parsedPages.Add(text);             
                var table = GetTable();
                var results = ParseTable(table);
                list.AddRange(results);
            } 
        }                
        return list;
    }

    private IWebElement GetTable()
    {
        var table = browser.FindElement(By.Id("GrdResultados"));        
        return table;
    }

    private IEnumerable<UVA> ParseTable(IWebElement table)
    {        
        var rows = table.FindElements(By.TagName("tr"));
        return rows.Select(row => ParseRow(row)).Where(u => u != null);
    }    
    private UVA ParseRow(IWebElement row)
    {
        var cells = row.FindElements(By.TagName("td"));
        DateTime date;
        UVA uva = null;
        if (cells.Count > 0 && DateTime.TryParse(cells[0].Text, out date) && Single.TryParse(cells[1].Text, out float value))
        {
            uva = new UVA{
                Date = date,
                Value = value
            };            
        }
        return uva;
    }    

}