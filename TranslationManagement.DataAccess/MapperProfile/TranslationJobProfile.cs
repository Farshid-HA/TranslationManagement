using AutoMapper;
using DataAccess.Entities;
using Domain.Dtos;

namespace DataAccess.MapperProfile
{
    public class TranslationJobProfile:Profile
    {
        public TranslationJobProfile()
        {
            CreateMap<TranslationJob,TranslationJobDto>().ReverseMap();
        }
    }
}
