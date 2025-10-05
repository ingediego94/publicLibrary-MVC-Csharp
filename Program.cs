using Microsoft.EntityFrameworkCore;
using publicLibrary.Data;

var builder = WebApplication.CreateBuilder(args);



// ------------------------------
// CONNECTION STRING OF DATABASE: (it's mandatoty to set on appsettings.json)

// 1) Getting the conection string from appsettings.json:
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2) Injecting the AppDbContext on the services container:
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
    optionsBuilder.UseMySql(
        connectionString,       // we will use the safe connection from appsettings.json
        ServerVersion.AutoDetect(connectionString)
    )
);

// ------------------------------



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Client}/{action=Index}/{id?}");

app.Run();
