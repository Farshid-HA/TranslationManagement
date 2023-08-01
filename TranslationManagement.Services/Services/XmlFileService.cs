using FormatConverter.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Xml.Linq;
using TranslationManagement.Domain.IServices;

namespace TranslationManagement.Services.Services
{
    public class XmlFileService : IFileService
    {
        public Document ReadFile(IFormFile file)
        {
            var document = new Document();
            var reader = new StreamReader(file.OpenReadStream());
            string content;
            var xdoc = XDocument.Parse(reader.ReadToEnd());
            content = xdoc.Root.Element("Content").Value;
            var customer = xdoc.Root.Element("Customer").Value.Trim();
            document.Content = content;
            document.Customer = customer;
            return document;
        }
    }
}

