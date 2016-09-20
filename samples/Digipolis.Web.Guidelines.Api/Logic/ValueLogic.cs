using System;
using System.Collections.Generic;
using AutoMapper;
using Digipolis.Web.Guidelines.Api.Data;
using Digipolis.Web.Guidelines.Api.Data.Entiteiten;
using Digipolis.Web.Guidelines.Api.Models;
using Digipolis.Web.Guidelines.Error;
using Digipolis.Web.Guidelines.Models;

namespace Digipolis.Web.Guidelines.Api.Logic
{
    public class ValueLogic : IValueLogic
    {
        private readonly IMapper _mapper;
        private readonly IValueRepository _valueRepository;
        private readonly IErrorManager _errorManager;

        public ValueLogic(IMapper mapper, IValueRepository valueRepository, IErrorManager errorManager)
        {
            _mapper = mapper;
            _valueRepository = valueRepository;
            _errorManager = errorManager;
        }

        public IEnumerable<ValueDto> GetAll(Query queryOptions, out int total)
        {
            var result = _valueRepository.GetAll(queryOptions, out total);
            return _mapper.Map<IEnumerable<Value>, IEnumerable<ValueDto>>(result);
        }

        public ValueDto GetById(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException();
            return _mapper.Map<Value, ValueDto>(_valueRepository.GetById(id));
        }

        public ValueDto Add(ValueDto value)
        {
            if (value == null) throw new ArgumentNullException();
            var entity = _mapper.Map<ValueDto, Value>(value);
            value = _mapper.Map<Value, ValueDto>(_valueRepository.Add(entity));
            return value;
        }

        public ValueDto Update(int id, ValueDto value)
        {
            if (id < 0) throw new ArgumentOutOfRangeException();
            if (value == null) throw new ArgumentNullException();

            var entity = _mapper.Map<ValueDto, Value>(value);
            value = _mapper.Map<Value, ValueDto>(_valueRepository.Update(id, entity));
            return value;
        }

        public void Delete(int id)
        {
            if (id < 0) throw new ArgumentOutOfRangeException();
            _valueRepository.Delete(id);
        }
    }
}