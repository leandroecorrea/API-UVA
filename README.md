# API-UVA
Simple API built using dotnet CLI & VSCode.
Database is populated by scraping BCRA webpage, first by parsing its HTML and, if it doesn't suceed, by downloading a .csv report and reading the content.

Endpoints:
/uva
/uva?date="DD/MM/YYYY"
/uva/interes?desde="DD/MM/YYYY"&hasta="DD/MM/YYYY"

The UI consists on a simple view built in Angular, drawing a line graph using ChartJS.

# Installation guide
* Clone this repository
* Download .NET 6.0 SDK and Runtime (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* Make sure your ChromeDriver version is supported (version used 97.0.4692.71)

# Database issues
For a fresh database start (this triggers the update) run
* dotnet ef database drop
* clear /UVAGraphs.API/Migrations
* dotnet ef migrations add InitialCreate
* dotnet ef database update
