using System;
using System.Collections.Generic;
using System.Text;
namespace BLL.Services
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.StaticFiles;

    public class FileService : IFileService
    {
        private readonly string _folder;

        private static readonly Dictionary<string, long> AllowedFiles = new()
    {
        { ".mp3", 50 * 1024 * 1024L },
        { ".wav", 50 * 1024 * 1024L },
        { ".mp4", 200 * 1024 * 1024L },
        { ".webm", 200 * 1024 * 1024L },
        { ".pdf", 20 * 1024 * 1024L },
        { ".docx", 20 * 1024 * 1024L },
        { ".xlsx", 20 * 1024 * 1024L },
        { ".pptx", 20 * 1024 * 1024L }
    };

        public FileService(string folder)
        {
            _folder = folder;
        
        }

        public string Save(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedFiles.TryGetValue(ext, out var maxSize))
                throw new ArgumentException("File type is not allowed");

            if (file.Length > maxSize)
                throw new ArgumentException("File is too large");

            var fileName = Guid.NewGuid().ToString("N") + ext;
            var filePath = Path.Combine(_folder, fileName);

            using (var stream = new FileStream(
                filePath,
                FileMode.CreateNew,
                FileAccess.Write))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }

        public string GetFilePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) ||
                fileName != Path.GetFileName(fileName))
                throw new ArgumentException("Invalid file name");

            return Path.Combine(_folder, fileName);
        }

        public void Delete(string fileName)
        {
            var filePath = GetFilePath(fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        public Stream GetFileStream(string fileName)
        {
            var filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found");

            return new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );
        }

        public string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(fileName, out var contentType))
                return "application/octet-stream";

            return contentType;
        }
    }
}
