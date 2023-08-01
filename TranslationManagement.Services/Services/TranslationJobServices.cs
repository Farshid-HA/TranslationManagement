using AutoMapper;
using DataAccess.Entities;
using Domain.Dtos;
using Domain.Enums;
using Domain.IServices;
using Domain.Repositories;
using FormatConverter.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranslationManagement.Domain.Dtos;
using TranslationManagement.Domain.Enums;
using TranslationManagement.Domain.IServices;

namespace TranslationManagement.Services.Services
{
    public class TranslationJobServices : ITranslationJobServices
    {
        private readonly IGenericRepository<TranslationJob> translationJobRepository;
        private readonly IMapper mapper;
        private readonly IReliableNotificationService reliableNotificationService;
        private readonly int pageSize = 5;

        public TranslationJobServices(IGenericRepository<TranslationJob> translationJobRepository, IMapper mapper, IReliableNotificationService reliableNotificationService)
        {
            this.translationJobRepository = translationJobRepository;
            this.mapper = mapper;
            this.reliableNotificationService = reliableNotificationService;
        }

        public PaginationDto<TranslationJobDto> GetJobs(string search = "", int page = 1)
        {
            var translationJobs = translationJobRepository.GetEntities();
            if (!string.IsNullOrEmpty(search))
            {
                translationJobs = translationJobs.Where(x => x.CustomerName.StartsWith(search));

            }
            var translationJobDtos = mapper.Map<List<TranslationJobDto>>(translationJobs);
            var count = translationJobDtos.Count();
            var skip = Math.Min((page - 1) * pageSize, count - 1);
            translationJobDtos = translationJobDtos.OrderByDescending(x => x.Id).Skip(skip).Take(pageSize).ToList();

            var result = new PaginationDto<TranslationJobDto>
            {
                Data = translationJobDtos,
                PageCount = (int)Math.Ceiling(((double)count / pageSize)),
                ItemsCount = count,
                Page = page
            };
            return result;

        }
        public async Task<ResultDto<bool>> CreateJob(TranslationJobDto jobDto)
        {
            try
            {
                if (jobDto == null) { return new ResultDto<bool>(false, MessageCodes.BadRequest); }


                if (jobDto.Status != JobStatusesEnum.New)
                {
                    return new ResultDto<bool>(false, MessageCodes.InvalidStatusChange);
                }
                SetPrice(jobDto);
                var job = mapper.Map<TranslationJob>(jobDto);


                await translationJobRepository.AddEntity(job);
                await translationJobRepository.SaveChange();

                //We do not wait for this process, it's running on background
                reliableNotificationService.SendNotification("Job created: " + job.Id);
                return new ResultDto<bool>(true);
            }
            catch (Exception)
            {
                return new ResultDto<bool>(false, MessageCodes.BadRequest);
            }
        }
        public async Task<ResultDto<bool>> CreateJobWithFile(IFormFile file, string customer, IFileService fileService)
        {
            if (file == null) { return new ResultDto<bool>(false, MessageCodes.NotFound); }
            var document = fileService.ReadFile(file);
            var newJobDto = new TranslationJobDto()
            {
                OriginalContent = document.Content,
                TranslatedContent = "",
                CustomerName = document.Customer != "" ? document.Customer : customer,
            };
            SetPrice(newJobDto);
            return await CreateJob(newJobDto);
        }
        public async Task<ResultDto<bool>> UpdateJobStatus(int jobId, int translatorId, int newStatus = 0)
        {
            try
            {
                if (!Enum.IsDefined(typeof(JobStatusesEnum), newStatus))
                {
                    return new ResultDto<bool>(false, MessageCodes.InvalidStatus);
                }

                var job = translationJobRepository.GetEntities().SingleOrDefault(j => j.Id == jobId);

                if (job == null)
                {
                    return new ResultDto<bool>(false, MessageCodes.NotFound);
                }
                job.TranslatorId = translatorId;

                bool isInvalidStatusChange = (job.Status == JobStatusesEnum.New && newStatus == (int)JobStatusesEnum.Completed) ||
                                             job.Status == JobStatusesEnum.Completed || newStatus == (int)JobStatusesEnum.New;
                if (isInvalidStatusChange)
                {
                    return new ResultDto<bool>(false, MessageCodes.InvalidStatusChange);
                }

                job.Status = (JobStatusesEnum)newStatus;
                await translationJobRepository.SaveChange();
                return new ResultDto<bool>(true);
            }
            catch (Exception)
            {

                return new ResultDto<bool>(false, MessageCodes.BadRequest);
            }

        }
        private void SetPrice(TranslationJobDto jobDto)
        {
            jobDto.Price = jobDto.OriginalContent.Length * Domain.Constant.Settings.PricePerCharacter;
        }


    }
}
