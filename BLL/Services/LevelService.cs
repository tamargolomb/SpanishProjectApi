using AutoMapper;
using BLL.Exceptions;
using DAL;
using DAL.Repositories;
using DTO;
using DTO.Categories;
using DTO.Lesson;
using DTO.Levels;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class LevelService: ILevelService
    {
        private readonly ILevelsRepository _LevelsRepository;
        private readonly IMapper _mapper;
        public LevelService(ILevelsRepository levelsRepository, IMapper mapper)
        {
            _LevelsRepository = levelsRepository;
            _mapper = mapper;
        }
        public ShowLevelDTO AddLevel(AddLessonDTO dto)
        {
            var level = _mapper.Map<Levels>(dto);
            _LevelsRepository.Add(level);
            return _mapper.Map<ShowLevelDTO>(level);
        }
        public ShowLevelDTO UpdateLevel(int id, ShowLevelDTO dto)
        {
            var existingLevel = _LevelsRepository.GetById(id);
            if (existingLevel == null)
                return null;

            _mapper.Map(dto, existingLevel);
            _LevelsRepository.Update(existingLevel);

            return _mapper.Map<ShowLevelDTO>(existingLevel);
        }
        public List<ShowLevelDTO> GetAllLevels()
        {
            var levels = _LevelsRepository.GetAll();
            return _mapper.Map<List<ShowLevelDTO>>(levels);
        }
        public ShowLevelDTO GetLevelById(int id)
        {
            var level = _LevelsRepository.GetById(id);

            if (level == null)
                throw new NotFoundException($"level with id {id} was not found");

            return _mapper.Map<ShowLevelDTO>(level);
        }
    }
}
