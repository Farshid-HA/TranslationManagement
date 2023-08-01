using Domain.Dtos;
using Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TranslationManagement.Domain.Dtos;

namespace TranslationManagement.Api.Controlers
{
    [ApiController]
    [Route("api/TranslatorManagement/[action]")]
    public class TranslatorManagementController : ControllerBase
    {
        private readonly ILogger<TranslatorManagementController> _logger;
        private readonly ITranslatorManagementService translatorManagementService;

        public TranslatorManagementController(ILogger<TranslatorManagementController> logger, ITranslatorManagementService translatorManagementService)
        {
            _logger = logger;
            this.translatorManagementService = translatorManagementService;
        }

        [HttpGet]
        public ResultDto<PaginationDto<TranslatorModelDto>> GetTranslators(string search = "", int page = 1)
        {
            var model = translatorManagementService.GetTranslators(search, page);
            return new ResultDto<PaginationDto<TranslatorModelDto>>(model);
        }

        [HttpGet]
        public TranslatorModelDto[] GetTranslatorsByName(string name)
        {
            var model = translatorManagementService.GetTranslatorsByName(name).ToArray();
            return model;
        }

        [HttpPost]
        public async Task<ResultDto<bool>> AddTranslator(TranslatorModelDto translatorModelDto)
        {
            return await translatorManagementService.AddTranslator(translatorModelDto);
        }

        [HttpPost]
        public async Task<ResultDto<bool>> UpdateTranslatorStatus(int translator, int newStatus = 0)
        {
            _logger.LogInformation("User status update request: " + newStatus + " for user " + translator.ToString());
            return await translatorManagementService.UpdateTranslatorStatus(translator, newStatus);

        }
    }
}