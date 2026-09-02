using System.Text.Json;

//Creating the Buider and the ASP.NET app
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Middleware: add customer header
app.Use(async (context, next) => {
    context.Response.Headers.Append("X-App-Name", "JournalApi");
    await next();
});

//Endpoints:
// GET/ (the default GET response) -> return plain text
app.MapGet("/", () => "Hello ASP.NET!");

// GET/time -> return JSON with UTC Time
app.MapGet("/time", () => {
    //Get the UTC Datetime
    var result = new { utc = DateTime.UtcNow };
    //return the UTC Date Time
    return Results.Json(result);
});

// GET/echo?message=... -> Return JSON {message, length}
// If no message, return 400
app.MapGet("/echo", (string ? msg) => {
    //Check if no message given
    if (string.IsNullOrWhiteSpace(msg)) {
        return Results.BadRequest(new { error = "Query parameter 'msg' is required." });
    }
    //Otherwise Echo Message and return length
    var result = new { message = msg, length = msg.Length };
    return Results.Json(result);
});

//Runs the application
app.Run();