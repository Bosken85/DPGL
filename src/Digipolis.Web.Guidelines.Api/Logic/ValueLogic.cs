using System;
using System.Collections.Generic;
using AutoMapper;
using Digipolis.Web.Guidelines;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Guidelines.Paging;
using StarterKit.SwashBuckle.Api.Data;
using StarterKit.SwashBuckle.Api.Data.Entiteiten;
using StarterKit.SwashBuckle.Api.Models;

namespace StarterKit.SwashBuckle.Api.Logic
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
            try
            {
                if(id < 0) throw new ArgumentOutOfRangeException();
                return _mapper.Map<Value, ValueDto>(_valueRepository.GetById(id));
            }
            catch (ArgumentOutOfRangeException)
            {
                _errorManager.Error.AddMessage(nameof(id), "There was no id specified to retrieve the value");
                throw;
            }
        }

        public ValueDto Add(ValueDto value)
        {
            try
            {
                if(value == null) throw new ArgumentNullException();
                var entity = _mapper.Map<ValueDto, Value>(value);
                value = _mapper.Map<Value, ValueDto>(_valueRepository.Add(entity));
                return value;
            }
            catch (ArgumentNullException)
            {
                _errorManager.Error.AddMessage("There was no value object specified.");
                throw;
            }
        }

        public ValueDto Update(int id, ValueDto value)
        {
            try
            {
                if (id < 0) throw new ArgumentOutOfRangeException();
                if (value == null) throw new ArgumentNullException();

                var entity = _mapper.Map<ValueDto, Value>(value);
                value = _mapper.Map<Value, ValueDto>(_valueRepository.Update(id, entity));
                return value;
            }
            catch (ArgumentOutOfRangeException)
            {
                _errorManager.Error.AddMessage(nameof(id), "There was no id specified to update the value");
                throw;
            }
            catch (ArgumentNullException)
            {
                _errorManager.Error.AddMessage("There was no value object specified.");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                if (id < 0) throw new ArgumentOutOfRangeException();
                _valueRepository.Delete(id);
            }
            catch (ArgumentOutOfRangeException)
            {
                _errorManager.Error.AddMessage(nameof(id), "There was no id specified to delete the value");
                throw;
            }
        }
    }
}