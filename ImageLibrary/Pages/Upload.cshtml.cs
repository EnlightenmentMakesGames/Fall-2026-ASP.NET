using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using ImageLibrary.Models;

namespace ImageLibrary.Pages {
    public class UploadModel : PageModel {
        //Private Readonly Environment
        private readonly IWebHostEnvironment _env;

        //Constructor with a singleton
        public UploadModel(IWebHostEnvironment env) {
            _env = env;
        }

        //Class Variables
        [BindProperty]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public IFormFile? Image { get; set; }

        public string? Message { get; set; }

        private const long MaxFileSiBytes = 2 * 1024 * 1024; //2 MB

        //Leave this alone:
        public void OnGet(){ }

        //Task to handle uploading
        public async Task<IActionResult> OnPostAsync() {
            //Basic validation: title required
            if (string.IsNullOrWhiteSpace(Title)) {
                ModelState.AddModelError(nameof(Title), "Title is required.");
            }

            //Validate the image
                //Validate that something was uploaded
            if (Image == null || Image.Length == 0) {
                ModelState.AddModelError(nameof(Image), "Please choose an image file.");
            } else {
                //Validate the file size
                if (Image.Length > MaxFileSiBytes) {
                    ModelState.AddModelError(nameof(Image), "File must be less than 2 MB.");
                }
                //Validate the extensions
                var extension = Path.GetExtension(Image.FileName).ToLowerInvariant();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png") {
                    //(extension not in [".jpg", ".jpeg", ".png"]) might also work
                    ModelState.AddModelError(nameof(Image), "Image uploaded must be either a .jpg or .png file.");
                }
            }
            //Validate against the annotations
            if (!ModelState.IsValid) {
                return Page();
            }
            //Ensure the uploads folder exists under wwwroot
            var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);

            var ext = Path.GetExtension(Image!.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var filePath = Path.Combine(uploadsPath, fileName);
            
            //Save the file
            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await Image.CopyToAsync(stream);
            }

            //Save the metadata to data/images.json
            var dataDir = Path.Combine(_env.ContentRootPath, "data");
            Directory.CreateDirectory(dataDir);
            var jsonPath = Path.Combine(dataDir, "images.json");

            List<ImageRecord> images;
            if (System.IO.File.Exists(jsonPath)) {
                try {
                    var json = await System.IO.File.ReadAllTextAsync(jsonPath);
                    images = JsonSerializer.Deserialize<List<ImageRecord>>(json) ?? new List<ImageRecord>();
                } catch {
                    images = new List<ImageRecord>();
                }
            } else {
                images = new List<ImageRecord>();
            }

            //Add the image to the list
            images.Add(new ImageRecord {
                FileName = fileName,
                Title = Title.Trim()
            });

            //Save the updates to the JSON
            var options = new JsonSerializerOptions { WriteIndented = true };
            var updatedJson = JsonSerializer.Serialize(images, options);
            await System.IO.File.WriteAllTextAsync(jsonPath, updatedJson);

            Message = "Image uploaded successfully!";

            return RedirectToPage("/Gallery");
            // return Page(); //this would redirect to Upload
        }
    }
}