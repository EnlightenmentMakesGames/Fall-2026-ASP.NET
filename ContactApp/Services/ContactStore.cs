using System.Text.Json;
using ContactApp.Models;

namespace ContactApp.Services;
public class ContactStore {
    //Variables to handle I/O
    private readonly string _dataDir;
    private readonly string _dataFile;
    private readonly object _lock = new object();
    private readonly JsonSerializerOptions _json = new() { WriteIndented = true };

    //List of Contacts to hold form submissions in RAM
    private List<Contact> _cache = new();

    //Constructor to handle working in the web environment
    public ContactStore(IWebHostEnvironment env) {
        //Set the pathing
        _dataDir = Path.Combine(env.ContentRootPath, "data");
        _dataFile = Path.Combine(_dataDir, "contacts.json");

        //Create the directory (if it doesn't exist yet)
        Directory.CreateDirectory(_dataDir);


        //Load all form responses
        LoadFromDisk();
    }

    //Method to get all form submissions
    public IReadOnlyList<Contact> GetAll() {
        lock (_lock) return _cache.ToList();
    }
    //Void to add a Contact
    public void Add(Contact c) {
        lock (_lock) {  //Prevents the readers writers problem
            _cache.Add(c);  //Adds the contact to the list of contacts
            SaveToDisk();   //Immediately saves to the Json file
        }
    }

    //Void to load from the disk
    private void LoadFromDisk() {
        //Handle if the Json doesn't exist
        if (!File.Exists(_dataFile)) { _cache = new(); return; }

        //Check if it's valid; if not, create an empty list
        try {
            var text = File.ReadAllText(_dataFile);
            _cache = JsonSerializer.Deserialize<List<Contact>>(text) ?? new();
        } catch {
            _cache = new(); //if corrupted, start fresh
        }

    }

    //Void to save to disk
    private void SaveToDisk() {
        var text = JsonSerializer.Serialize(_cache, _json);
        File.WriteAllText(_dataFile, text);
    }
}