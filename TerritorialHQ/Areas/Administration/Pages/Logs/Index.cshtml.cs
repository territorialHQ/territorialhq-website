using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO.Compression;

namespace TerritorialHQ.Areas.Administration.Pages.Logs
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public IndexModel(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        public List<string> Files { get; set; } = new List<string>();

        public void OnGet()
        {
            var dir = _env.WebRootPath + "/Data/Logs";

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            var files = System.IO.Directory.GetFiles(dir);
            foreach (var file in files)
            {
                Files.Add(System.IO.Path.GetFileName(file));
            }

            Files = Files.OrderByDescending(o => o).ToList();
        }

        public IActionResult OnGetDbBackup()
        {
            var db_server = ConfigurationBinder.GetValue<string>(_config, "DB_SERVER");
            var db_port = ConfigurationBinder.GetValue<string>(_config, "DB_PORT") ?? "3306";
            var db_cat = ConfigurationBinder.GetValue<string>(_config, "DB_CATALOGUE");
            var db_user = ConfigurationBinder.GetValue<string>(_config, "DB_USER");
            var db_pw = ConfigurationBinder.GetValue<string>(_config, "DB_PASSWORD");

            var connectionString = $"Server={db_server};Port={db_port};Database={db_cat};Uid={db_user};Pwd={db_pw};";

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            var fileName = "territorialhq-db-" + DateTime.Now.Ticks.ToString() + ".sql";
            var tempFilePath = System.IO.Path.GetTempPath() + fileName;

            using (var cmd = new MySqlCommand())
            {
                cmd.Connection = conn;
                using var mb = new MySqlBackup(cmd);
                mb.ExportToFile(tempFilePath);
            }

            var fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);

            return File(fileBytes, "application/octet-stream", fileName);
        }

        public IActionResult OnGetFileBackup()
        {
            var folder = _env.WebRootPath + "/Data/";

            string zipFileName = "territorialhq-files-" + DateTime.Now.Ticks.ToString() + ".zip";
            string tempFilePath = Path.GetTempPath() + zipFileName;

            using (var zipArchive = ZipFile.Open(tempFilePath, ZipArchiveMode.Create))
            {
                AddFilesToZip(zipArchive, folder, string.Empty);
            }

            var fileBytes = System.IO.File.ReadAllBytes(tempFilePath);
            System.IO.File.Delete(tempFilePath);

            return File(fileBytes, "application/octet-stream", zipFileName);
        }

        private void AddFilesToZip(ZipArchive zipArchive, string sourceFolderPath, string relativePath)
        {
            DirectoryInfo directory = new(sourceFolderPath);
            FileInfo[] files = directory.GetFiles();
            DirectoryInfo[] subDirectories = directory.GetDirectories();

            foreach (FileInfo file in files)
            {
                string entryName = Path.Combine(relativePath, file.Name);
                zipArchive.CreateEntryFromFile(file.FullName, entryName);
            }

            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                string newRelativePath = Path.Combine(relativePath, subDirectory.Name);
                AddFilesToZip(zipArchive, subDirectory.FullName, newRelativePath);
            }
        }
    }
}
