using MicroBlog.Services;
using MicroBlog.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Choose which repo to use by swapping the type
builder.Services.AddSingleton<IBlogRepository, JsonBlogRepository>();
//builder.Services.AddSingleton<IBlogRepository, InMemoryBlogRepository>();

//Bind SiteOptions from configuration
builder.Services.Configure<SiteOptions>(builder.Configuration.GetSection("SiteOptions"));

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

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
