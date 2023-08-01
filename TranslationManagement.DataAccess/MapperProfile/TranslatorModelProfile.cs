using AutoMapper;
using DataAccess.Entities;
using Domain.Dtos;

namespace DataAccess.MapperProfile
{
    public class TranslatorModelProfile:Profile
    {
        public TranslatorModelProfile()
        {
            CreateMap<TranslatorModel,TranslatorModelDto>().ReverseMap();
        }
    }
}
