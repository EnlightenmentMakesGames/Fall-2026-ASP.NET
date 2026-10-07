using MicroBlog.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MicroBlog.Services {
    public class JsonBlogRepository : IBlogRepository {
        //Class variables
        private readonly string _dataDir;
        private readonly string _dataFile;
        private readonly object _lock = new();

        //Json options
        private readonly JsonSerializerOptions _json = new JsonSerializerOptions {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        //List and Id
        private List<Post> _posts = new();
        private int _nextId = 1;
        
        //Constructor
        public JsonBlogRepository(IWebHostEnvironment env) {
            _dataDir = Path.Combine(env.ContentRootPath, "data");
            _dataFile = Path.Combine(_dataDir, "posts.json");
            Directory.CreateDirectory(_dataDir);
            LoadFromDisk();
        }

        //Get All method
        public IEnumerable<Post> GetAll() {
            lock (_lock) return _posts.OrderByDescending(p => p.CreatedUtc).ToList();
        }

        //Get By Id method
        public Post? GetById(int id) {
            lock (_lock) return _posts.FirstOrDefault(p => p.Id == id);
        }

        //Add method
        public void Add(Post post) {
            lock (_lock) {
                post.Id = _nextId++;
                post.CreatedUtc = DateTime.UtcNow;
                _posts.Add(post);
                SaveToDisk(); //save on each write
            }
        }

        //Save method
        public void Save() {
            lock (_lock) SaveToDisk();
        }

        //Load From Disk method
        private void LoadFromDisk() {
            if (!File.Exists(_dataFile)) { _posts = new(); _nextId = 1; return; }
            try {
                var text = File.ReadAllText(_dataFile);
                _posts = JsonSerializer.Deserialize<List<Post>>(text, _json) ?? new();
                _nextId = _posts.Count == 0 ? 1 : _posts.Max(p => p.Id) + 1;
            } catch {
                _posts = new();
                _nextId = 1;
            }
        }

        //Save To Disk method
        private void SaveToDisk() {
            var text = JsonSerializer.Serialize(_posts, _json);
            File.WriteAllText(_dataFile, text);
        }
    }
}