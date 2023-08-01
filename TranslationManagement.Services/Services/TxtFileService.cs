using FormatConverter.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using TranslationManagement.Domain.IServices;

namespace TranslationManagement.Services.Services
{
    public class TxtFileService : IFileService
    {
        public Document ReadFile(IFormFile file)
        {
            var document = new Document();
            var reader = new StreamReader(file.OpenReadStream());
            string content;
            content = reader.ReadToEnd();
            document.Content = content;
            return document;

        }
    }
}
