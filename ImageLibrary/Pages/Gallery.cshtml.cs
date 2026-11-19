using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using ImageLibrary.Models;

namespace ImageLibrary.Pages {
    public class GalleryModel : PageModel {
        //Environment
        private readonly IWebHostEnvironment _env;

        //Constructor with a singleton for the environment
        public GalleryModel(IWebHostEnvironment env) {
            _env = env;
        }

        //List of image records
        public List<ImageRecord> Images { get; set; } = new();

        //Method to handle viewing images
        public void OnGet() {
            //Set up the data path
            var dataDir = Path.Combine(_env.ContentRootPath, "data");
            var jsonPath = Path.Combine(dataDir, "images.json");

            //Create a new list if there are no images
            if (!System.IO.File.Exists(jsonPath)) {
                Images = new List<ImageRecord>();
            }

            //Try Catch to read all the files
            try {
                var json = System.IO.File.ReadAllText(jsonPath);
                Images = JsonSerializer.Deserialize<List<ImageRecord>>(json) ?? new List<ImageRecord>();
            } catch {
                Images = new List<ImageRecord>();
            }
        }
    }
}