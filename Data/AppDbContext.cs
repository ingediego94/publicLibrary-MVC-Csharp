using Microsoft.EntityFrameworkCore;
using publicLibrary.Models;

namespace publicLibrary.Data;

public class AppDbContext : DbContext
{

    // Constructor: it receives the options that were configured on Program.cs
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    // To create tables on the database with EF:
    public DbSet<Client> clients { get; set; }
    public DbSet<Book> books { get; set; }
    public DbSet<Loan> loans { get; set; }
    public DbSet<LoanDetails> loansDetails { get; set; }
}

// Installing by terminal o as Nuggets Packages, all of them version: 9.0.0
// dotnet add package microsoft.EntityFrameworkCore
// dotnet add package microsoft.EntityFrameworkCore.design
// dotnet add package Pomelo.EntityFrameworkCore.MySql
// dotnet tool install --global dotnet-ef

// To start the migration:
// dotnet ef migrations add InitialCreate
// dotnet ef database update