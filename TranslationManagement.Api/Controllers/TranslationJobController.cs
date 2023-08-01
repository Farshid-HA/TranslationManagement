using Domain.Dtos;
using Domain.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranslationManagement.Domain.Dtos;
using TranslationManagement.Domain.Enums;
using TranslationManagement.Domain.IServices;
using TranslationManagement.Services.Services;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/TranslationJob/[action]")]
    public class TranslationJobController : ControllerBase
    {
        private readonly ILogger<TranslationJobController> _logger;
        private readonly ITranslationJobServices translationJobServices;

        public TranslationJobController(ILogger<TranslationJobController> logger, ITranslationJobServices translationJobServices)
        {
            _logger = logger;
            this.translationJobServices = translationJobServices;
        }

        [HttpGet]
        public ResultDto<PaginationDto<TranslationJobDto>> GetJobs(string search = "", int page = 1)
        {
            var model = translationJobServices.GetJobs(search, page);
            return new ResultDto<PaginationDto<TranslationJobDto>>(model);
        }

        [HttpPost]
        public async Task<ResultDto<bool>> CreateJob(TranslationJobDto jobDto)
        {
            return await translationJobServices.CreateJob(jobDto);
        }

        [HttpPost]
        public async Task<ResultDto<bool>> CreateJobWithFile([FromForm] string customer, IFormFile file)
        {

            var ext = System.IO.Path.GetExtension(file.FileName).Replace(".", "");
            if (!Constant.Files.FileTypes.ContainsKey(ext))
            {
                return new ResultDto<bool>(false, MessageCodes.UnsupportedFile);
            }
            IFileService fileService = (IFileService)Activator.CreateInstance(Constant.Files.FileTypes[ext]);
            return await translationJobServices.CreateJobWithFile(file, customer, fileService);
        }

        [HttpPost]
        public async Task<ResultDto<bool>> UpdateJobStatus(UpdateJobStatus model)
        {
            _logger.LogInformation("Job status update request received: " + model.newStatus + " for job " + model.jobId.ToString() + " by translator " + model.translatorId);
            return await translationJobServices.UpdateJobStatus(model.jobId, model.translatorId, (int)model.newStatus);
        }
    }
}