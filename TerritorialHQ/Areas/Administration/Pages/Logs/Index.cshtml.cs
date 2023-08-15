using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using MySql.Data.MySqlClient;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Logs
{
    [Authorize(Roles = "Administrator")]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly LogFileService _logFileService;

        public IndexModel(IWebHostEnvironment env, IConfiguration config, LogFileService logFileService)
        {
            _env = env;
            _config = config;
            _logFileService = logFileService;
        }

        public List<string> TthqFiles { get; set; } = new List<string>();
        public List<string> ApisFiles { get; set; } = new List<string>();

        public async Task<IActionResult> OnGet()
        {
            var dir = _env.WebRootPath + "/Data/Logs";
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            var files = System.IO.Directory.GetFiles(dir);
            foreach (var file in files)
            {
                TthqFiles.Add(System.IO.Path.GetFileName(file));
            }
            TthqFiles = TthqFiles.OrderByDescending(o => o).ToList();

            ApisFiles = await _logFileService.GetFilesAsync() ?? new();
            return Page();
        }

        public async Task<IActionResult> OnGetLocalDownload(string fileName)
        {
            var path = _env.WebRootPath + "/Data/Logs/" + fileName;
            if (!System.IO.File.Exists(path))
                return NotFound();

            try
            {
                var content = await System.IO.File.ReadAllBytesAsync(path);
                new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);

                return File(content, contentType);
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnGetDownload(string fileName)
        {
            var fileBytes = await _logFileService.GetFile(fileName);

            if (fileBytes == null)
                return NotFound();

            return File(fileBytes, "text/plain");
        }

        public async Task<IActionResult> OnGetSqlBackup()
        {
            var fileBytes = await _logFileService.GetSqlBackup();

            if (fileBytes == null)
                return NotFound();

            return File(fileBytes, "application/octet-stream", "tthq-backup-" + DateTime.Now.Ticks + ".sql");
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
