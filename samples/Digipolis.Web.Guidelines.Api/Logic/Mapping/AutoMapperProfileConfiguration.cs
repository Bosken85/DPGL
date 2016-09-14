using System.Collections.Generic;
using AutoMapper;
using Digipolis.Web.Guidelines.Api.Data.Entiteiten;
using Digipolis.Web.Guidelines.Api.Models;
using Digipolis.Web.Guidelines.Models;

namespace Digipolis.Web.Guidelines.Api.Logic.Mapping
{
    public class AutoMapperProfileConfiguration : Profile
    {
        protected override void Configure()
        {
            CreateMap<Value, ValueDto>().ReverseMap();
            CreateMap<ValueType, ValueTypeDto>().ReverseMap();
            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>));
            CreateMap(typeof(IEnumerable<>), typeof(PagedResult<>)).ForMember("Data", x => x.MapFrom(m => m));

            CreateMap<File, FileDto>().ConstructUsing(x=> new FileDto { Id =  x.Id, ValueId = x.ValueId, Stream = x.Stream}).ReverseMap();

        }
    }
}