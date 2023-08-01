using Domain.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranslationManagement.Domain.Dtos;

namespace Domain.IServices
{
    public interface ITranslatorManagementService
    {
        PaginationDto<TranslatorModelDto> GetTranslators(string search = "", int page = 1);
        List<TranslatorModelDto> GetTranslatorsByName(string name);
        Task<ResultDto<bool>> AddTranslator(TranslatorModelDto translatorDto);
        Task<ResultDto<bool>> UpdateTranslatorStatus(int Translator, int newStatus = 0);
    }
}
