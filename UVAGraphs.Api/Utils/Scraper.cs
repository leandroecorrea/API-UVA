namespace UVAGraphs.Api.Utils;

using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using UVAGraphs.Api.Model;
using System.Collections.Generic;

public abstract class Scraper : IDisposable
{    
    protected static readonly string URL_WITHOUT_FILTERS = "http://www.bcra.gov.ar/Estadisticas/EstadisSitiopublico/Proceso.aspx?selecciones=7913%3b7913%3b%7c%3bnotasMetodologicas%3bnotasDatos%3bnotasLinks&";
    protected static readonly string WEBDRIVER_RUNNING_MODE = "headless";
    protected ChromeDriver browser;
    protected ChromeOptions options;
    protected static readonly DateTime FIRST_UVA_HISTORIC_VALUE_DATE = new DateTime(2016, 3, 31);

    

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
    protected (DateTime from, DateTime to) SetDates(DateTime? beginDate)
    {
        DateTime from;
        if (beginDate == null)
            from = FIRST_UVA_HISTORIC_VALUE_DATE;
        else
            from = (DateTime)beginDate;
        DateTime to = DateTime.Today;
        return (from, to);
    }

    protected string SetUrl(DateTime beginDate, DateTime endDate)
    {
        var filterEndpointString = $"fechaDesde={beginDate.ToFixedString()}&fechaHasta={endDate.ToFixedString()}";
        return URL_WITHOUT_FILTERS + filterEndpointString;
    }
}