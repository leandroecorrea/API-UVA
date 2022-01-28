using UVAGraphs.Api.Model;
using UVAGraphs.Api.Repositories;
using UVAGraphs.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// var connectionString = builder.Configuration.GetConnectionString("uvagraphs") ?? "Data Source=uvagraphs.db";
// builder.Services.AddSqlite<UVAContext>(connectionString);

builder.Services.AddDbContext<UVAContext>(ServiceLifetime.Singleton);
// Add services to the container.

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<UVARepository>();
builder.Services.AddSingleton<IUpdatable>(x => x.GetRequiredService<UVARepository>());
builder.Services.AddSingleton<IUVARepository>(x => x.GetRequiredService<UVARepository>());
builder.Services.AddHostedService<UpdateServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
