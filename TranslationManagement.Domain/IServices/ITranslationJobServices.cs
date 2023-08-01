using Domain.Dtos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranslationManagement.Domain.Dtos;
using TranslationManagement.Domain.IServices;

namespace Domain.IServices
{
    public interface ITranslationJobServices
    {
        PaginationDto<TranslationJobDto> GetJobs(string search = "", int page = 1);

        Task<ResultDto<bool>> CreateJob(TranslationJobDto jobDto);

        Task<ResultDto<bool>> CreateJobWithFile(IFormFile file, string customer,IFileService fileService);


        Task<ResultDto<bool>> UpdateJobStatus(int jobId, int translatorId, int newStatus = 0);

    }
}
