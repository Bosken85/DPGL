using System.Collections.Generic;
using AutoMapper;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Guidelines.Paging;
using StarterKit.SwashBuckle.Api.Data.Entiteiten;
using StarterKit.SwashBuckle.Api.Models;

namespace StarterKit.SwashBuckle.Api.Logic.Mapping
{
    public class AutoMapperProfileConfiguration : Profile
    {
        protected override void Configure()
        {
            CreateMap<Value, ValueDto>().ReverseMap();
            CreateMap<ValueType, ValueTypeDto>().ReverseMap();
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
            CreateMap(typeof(IEnumerable<>), typeof(PagedResult<>))
                .ForMember("Data", x => x.MapFrom(m => m));
        }
    }
}