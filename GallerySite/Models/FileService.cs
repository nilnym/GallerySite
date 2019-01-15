using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GallerySite.Models
{
    public class FileService
    {
        readonly string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        /// <summary>
        /// Strips file name of invalid characters and creates an unique file name if the file already exists.
        /// </summary>
        /// <param name="fileName">File name to process.</param>
        public string GetFileName(string fileName)
        {
            if (Path.GetExtension(fileName) == string.Empty)
                return null;

            fileName = Path.GetFileName(fileName);

            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

            if (File.Exists(Path.Combine(basePath, fileName)))
                fileName = MakeUniqueFileName(fileName);

            return fileName;
        }

        /// <summary>
        /// Saves a file from a form to disk.
        /// </summary>
        /// <param name="file">File uploaded from a form</param>
        /// <param name="fileName">Name of file</param>
        public async Task<bool> SaveFileFromFormAsync(IFormFile file, string fileName)
        {
            try
            {
                using (var stream = new FileStream(
                    Path.Combine(basePath, fileName),
                    FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Downloads a file from provided URL and saves it to disk.
        /// </summary>
        /// <param name="imageUrl">URL of file</param>
        /// <param name="fileName">Name of file</param>
        public async Task<bool> SaveFileFromUrlAsync(string imageUrl, string fileName)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var image = await client.GetByteArrayAsync(imageUrl);
                    await File.WriteAllBytesAsync(
                        Path.Combine(basePath, fileName), image);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        string MakeUniqueFileName(string file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file);
            var fileExtension = Path.GetExtension(file);

            return $"{fileName}{DateTime.Now.Ticks}{fileExtension}";
        }
    }
}
