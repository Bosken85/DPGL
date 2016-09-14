using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Digipolis.Web.Guidelines.Api.Data;
using Digipolis.Web.Guidelines.Api.Data.Entiteiten;
using Digipolis.Web.Guidelines.Models;
using Digipolis.Web.Guidelines.Api.Models;
using File = Digipolis.Web.Guidelines.Api.Data.Entiteiten.File;

namespace Digipolis.Web.Guidelines.Api.Logic
{
    public class FileLogic : IFileLogic
    {
        private readonly IMapper _mapper;
        private readonly IFileRepository _fileRepository;
        private readonly IErrorManager _errorManager;

        public FileLogic(IMapper mapper, IFileRepository fileRepository, IErrorManager errorManager)
        {
            _mapper = mapper;
            _fileRepository = fileRepository;
            _errorManager = errorManager;
        }

        public IEnumerable<FileDto> GetAll(int valueId, Query queryOptions, out int total)
        {
            var result = _fileRepository.GetAll(valueId, queryOptions, out total);
            return _mapper.Map<IEnumerable<File>, IEnumerable<FileDto>>(result);
        }

        public FileDto GetById(int valueId, int id)
        {
            try
            {
                if (id <= 0 || valueId <= 0) throw new ArgumentOutOfRangeException();
                return _mapper.Map<File, FileDto>(_fileRepository.GetById(valueId, id));
            }
            catch (ArgumentOutOfRangeException)
            {
                _errorManager.Error.AddMessage(nameof(id), "There was no id specified to retrieve the value");
                throw;
            }
        }

        public FileDto Add(int valueId, FileDto value)
        {
            try
            {
                if (value == null) throw new ArgumentNullException();
                var entity = _mapper.Map<FileDto, File>(value);
                value = _mapper.Map<File, FileDto>(_fileRepository.Add(valueId, entity));
                return value;
            }
            catch (ArgumentNullException)
            {
                _errorManager.Error.AddMessage("There was no value object specified.");
                throw;
            }
        }

        public FileDto Update(int valueId, int id, FileDto value)
        {
            try
            {
                if (id < 0) throw new ArgumentOutOfRangeException();
                if (value == null) throw new ArgumentNullException();

                var entity = _mapper.Map<FileDto, File>(value);
                value = _mapper.Map<File, FileDto>(_fileRepository.Update(valueId, id, entity));
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

        public void Delete(int valueId, int id)
        {
            try
            {
                if (id < 0) throw new ArgumentOutOfRangeException();
                _fileRepository.Delete(valueId, id);
            }
            catch (ArgumentOutOfRangeException)
            {
                _errorManager.Error.AddMessage(nameof(id), "There was no id specified to delete the value");
                throw;
            }
        }
    }
}