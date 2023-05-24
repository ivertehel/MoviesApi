using ApiApplication.Components.CreateShowtime;
using ApiApplication.Database.Entities;
using AutoMapper;

namespace ApiApplication
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MovieEntity, CreateShowtimeCommandResult>();
        }
    }
}