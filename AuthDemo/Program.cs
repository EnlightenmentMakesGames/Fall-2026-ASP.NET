using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using AuthDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Configure cookie authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

//Error Handling
app.UseExceptionHandler("/Error");

//Seed the first user
var dataDir = Path.Combine(app.Environment.ContentRootPath, "data");
var usersFile = Path.Combine(dataDir, "users.json");
Directory.CreateDirectory(dataDir);

//Create User "admin" and Password "Password123" and hash it
if (!File.Exists(usersFile)) {
    var hash = Convert.ToHexString(
        SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes("Password123"))
    );
    var user = new[] {new User { Username = "admin", PasswordHash = hash }};
    File.WriteAllText(usersFile, JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true }));
}

app.Run();
