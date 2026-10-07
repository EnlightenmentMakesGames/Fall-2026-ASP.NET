using System.Text.Json;
using System.Text.Json.Serialization;
using MicroBlog.Models;

namespace MicroBlog.Services {
    public class PostStore {
        //Class Variable
        private readonly string _dataDir;
        private readonly string _dataFile;
        private readonly object _lock = new object();

        private readonly JsonSerializerOptions _json = new() {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private List<Post> _cache = new();
        private int _nextId = 1;

        //Constructor
        public PostStore(IWebHostEnvironment env) {
            _dataDir = Path.Combine(env.ContentRootPath, "data");
            _dataFile = Path.Combine(_dataDir, "posts.json");
            Directory.CreateDirectory(_dataDir);
            LoadFromDisk();
        }

        //Get All Method
        public IReadOnlyList<Post> GetAll() {
            lock(_lock) return _cache
                    .OrderByDescending(p => p.CreatedUtc)
                    .ToList();
        }

        //Add Method
        public Post Add(Post p) {
            lock (_lock) {
                p.Id = _nextId++;
                p.CreatedUtc = DateTime.UtcNow;
                _cache.Add(p);
                SaveToDisk();
                return p;
            }
        }

        //Get Post by ID Method
        public Post? GetById(int id) {
            lock (_lock) return _cache.FirstOrDefault(p => p.Id == id);
        }

        //Load from Disk
        private void LoadFromDisk() {
            //Quit if the file doesn't exist
            if (!File.Exists(_dataFile)) { _cache = new(); _nextId = 1; return; }

            //Try/Catch to handle corruption
            try {
                var text = File.ReadAllText(_dataFile);
                _cache = JsonSerializer.Deserialize<List<Post>>(text, _json) ?? new();
                _nextId = _cache.Count == 0 ? 1 : _cache.Max(p => p.Id) + 1;
            } catch {
                _cache = new();
                _nextId = 1;
            }
        }

        //Save to Disk
        private void SaveToDisk() {
            var text = JsonSerializer.Serialize(_cache, _json);
            File.WriteAllText(_dataFile, text);
        }

    }
}