using System;
using Nightingale.Core.Entities;
using Nightingale.App.Models;
using AutoMapper;
using Nightingale.Core.Identity;

namespace Nightingale.App.Mapper
{
    public static class NightingaleMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<NightingaleDtoMapper>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }

    public class NightingaleDtoMapper : Profile
    {
        public NightingaleDtoMapper()
        {
            CreateMap<Message, MessageModel>()
                .ForMember(dest => dest.Receiver,
                    opt => opt.MapFrom(src => NightingaleMapper.Mapper.Map<UserModel>(src.Receiver)))
                .ForMember(dest => dest.Sender,
                    opt => opt.MapFrom(src => NightingaleMapper.Mapper.Map<UserModel>(src.Sender)));

            CreateMap<User, UserModel>()
                .ForMember(dest=> dest.UserName,
                opt => opt.MapFrom(src => src.UserName));
        }
    }
}