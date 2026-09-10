using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using GradebookApi.Models;

//Creating the app builder
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//In memory storage
var grades = new List<StudentGrade>();
var gradesLock = new object();

//Handling for the data folder, for save/load to JSON
var dataDir = Path.Combine(app.Environment.ContentRootPath, "data");

    ///// The following line was written by Copilot on 9/9/2026 at 8:30 PM CST
    var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "data"));

var dataFile = Path.Combine(dataDir, "grades.json");
Directory.CreateDirectory(dataDir);

//Validation Helper
static bool TryValidate<T>(T model, out Dictionary<string, string[]> errors){
    var ctx = new ValidationContext(model!);
    var results = new List<ValidationResult>();
    var isValid = Validator.TryValidateObject(model!, ctx, results, validateAllProperties: true);

    errors = results
        .GroupBy(r => r.MemberNames.FirstOrDefault() ?? string.Empty)
        .ToDictionary(
            g => string.IsNullOrEmpty(g.Key) ? "_model" : g.Key,
            g => g.Select(r => r.ErrorMessage ?? "Invalid").ToArray()
            );


    //required return statement
    return isValid;
}

//Set JSON Options
var jsonOpts = new JsonSerializerOptions{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

// --- ENDPOINTS ---
//app.MapGet("/", () => "Hello World!");    //Default Hello World Endpoint

// GET /grades -> List All Grades
app.MapGet("/grades", ()=>{
    lock (gradesLock) {
        //retur a copy so callers cannot modify the list
        return Results.Json(grades.ToArray(), jsonOpts);
    }
});

// POST /grades -> Validate and Add a Grade
app.MapPost("/grades", (StudentGrade grade) => {
    if (!TryValidate(grade, out var errors)) return Results.BadRequest(new {errors});
    //if it passes validation, lock and add the grade
    lock (gradesLock) {
        grades.Add(new StudentGrade{
            Student = grade.Student.Trim(),
            Score = grade.Score
        });
    }
    //return the created result
    return Results.Created($"/grades", grade);
});

// POST /save -> Write to data/grades.json
app.MapPost("/save", () => {
    lock (gradesLock){
        File.WriteAllText(dataFile, JsonSerializer.Serialize(grades, jsonOpts));
    }
    return Results.Ok(new{saved = true, file = $"data{Path.DirectorySeparatorChar}grades.json"});
});

// POST /load -> Load from data/grades.json if it exists
app.MapPost("/load", () => {
    if (!File.Exists(dataFile)) return Results.NotFound(new {error = "No saved file found."});
    
    //load the file
    List<StudentGrade>? loaded;
    try {
        loaded = JsonSerializer.Deserialize<List<StudentGrade>>(File.ReadAllText(dataFile), jsonOpts) ?? [];
    } catch {
        return Results.BadRequest(new {error = "Failed to read or parse grades.json."});
    }

    //validate the file
    foreach (var g in loaded){
        if (!TryValidate(g, out var err)) Results.BadRequest(new {error = "File contains invalid data.", details = err});
    }

    lock (gradesLock){
        grades.Clear();
        grades.AddRange(loaded);
    }

    //return ok if grades are loaded successfully
    return Results.Ok(new { loaded = grades.Count });

});


//Finally run the application
app.Run();