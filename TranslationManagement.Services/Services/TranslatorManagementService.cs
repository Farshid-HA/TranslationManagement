using AutoMapper;
using DataAccess.Entities;
using Domain.Dtos;
using Domain.Enums;
using Domain.IServices;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranslationManagement.Domain.Dtos;
using TranslationManagement.Domain.Enums;

namespace TranslationManagement.Services.Services
{
    public class TranslatorManagementService : ITranslatorManagementService
    {
        private readonly IGenericRepository<TranslatorModel> translatorModelRepository;
        private readonly IMapper mapper;
        private readonly int pageSize = 5;

        public TranslatorManagementService(IGenericRepository<TranslatorModel> translatorModelRepository, IMapper mapper)
        {
            this.translatorModelRepository = translatorModelRepository;
            this.mapper = mapper;
        }

        public PaginationDto<TranslatorModelDto> GetTranslators(string search = "", int page = 1)
        {
            var translators = translatorModelRepository.GetEntities();
            if (!string.IsNullOrEmpty(search))
            {
                translators = translators.Where(x => x.Name.StartsWith(search));

            }
            var translatorDtos = mapper.Map<List<TranslatorModelDto>>(translators);
            var count = translatorDtos.Count();
            var skip = Math.Min((page - 1) * pageSize, count - 1);
            translatorDtos = translatorDtos.OrderByDescending(x => x.Id).Skip(skip).Take(pageSize).ToList();

            var result = new PaginationDto<TranslatorModelDto>
            {
                Data = translatorDtos,
                PageCount = (int)Math.Ceiling(((double)count / pageSize)),
                ItemsCount = count,
                Page = page
            };
            return result;

        }


        public List<TranslatorModelDto> GetTranslatorsByName(string name)
        {
            var model = translatorModelRepository.GetEntities().Where(t => t.Name.StartsWith(name));
            return mapper.Map<List<TranslatorModelDto>>(model.ToList());
        }


        public async Task<ResultDto<bool>> AddTranslator(TranslatorModelDto translatorDto)
        {
            try
            {
                if (translatorDto == null) return new ResultDto<bool>(false, MessageCodes.BadRequest);
                var translatorModel = mapper.Map<TranslatorModel>(translatorDto);
                await translatorModelRepository.AddEntity(translatorModel);
                await translatorModelRepository.SaveChange();
                return new ResultDto<bool>(true, MessageCodes.Success);
            }
            catch (Exception e)
            {
                return new ResultDto<bool>(false, MessageCodes.BadRequest);
            }
        }


        public async Task<ResultDto<bool>> UpdateTranslatorStatus(int Translator, int newStatus = 0)
        {
            try
            {
                if (!Enum.IsDefined(typeof(TranslatorStatusEnum), newStatus))
                {
                    return new ResultDto<bool>(false, MessageCodes.InvalidStatus);
                }

                var job = translatorModelRepository.GetEntities().SingleOrDefault(j => j.Id == Translator);
                if (job == null)
                {
                    return new ResultDto<bool>(false, MessageCodes.NotFound);
                }
                job.Status = (TranslatorStatusEnum)newStatus;

                translatorModelRepository.UpdateEntity(job);
                await translatorModelRepository.SaveChange();


                return new ResultDto<bool>(true, MessageCodes.Success);
            }
            catch (Exception e)
            {
                return new ResultDto<bool>(false, MessageCodes.UnHandleException);
            }
        }


    }
}
