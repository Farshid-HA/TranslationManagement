using System.Collections.Generic;
using System;
using TranslationManagement.Services.Services;

namespace TranslationManagement.Api.Constant
{
    public class Files
    {
        public static IDictionary<string, Type> FileTypes = new Dictionary<string, Type>()
            {
                { "txt", typeof(TxtFileService) }, {"xml",typeof(XmlFileService)}
            };
    }
}
