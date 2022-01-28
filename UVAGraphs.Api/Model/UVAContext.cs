using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UVAGraphs.Api.Model;

public class UVAContext : DbContext
{
    public DbSet<UVA>? Uvas { get; set; }
    public string DbPath { get; private set; }

    public UVAContext(DbContextOptions options) : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}uvagraphs.db";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}