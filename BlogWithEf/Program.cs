using BlogWithEf.Data;
using Microsoft.EntityFrameworkCore;
 
var builder = WebApplication.CreateBuilder(args);

//Handle the data directory
var dataDir = Path.Combine(builder.Environment.ContentRootPath, "data");
Directory.CreateDirectory(dataDir);
 
//Create the SQLite connection
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlite($"Data Source={Path.Combine(dataDir, "blog.db")}"));

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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
