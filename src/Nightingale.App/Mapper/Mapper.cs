using System;
using Nightingale.Core.Entities;
using Nightingale.App.Models;
using AutoMapper;

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
            CreateMap<Message, MessageModel>();
        }
    }
}