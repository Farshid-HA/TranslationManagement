using FormatConverter.Models;
using Microsoft.AspNetCore.Http;

namespace TranslationManagement.Domain.IServices
{
    public interface IFileService
    {
        Document ReadFile(IFormFile file);
    }
}
