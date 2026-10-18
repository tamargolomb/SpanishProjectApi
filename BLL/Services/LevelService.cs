using AutoMapper;
using DAL;
using DAL.Repositories;
using DTO;
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
        public void Add(Levels level)
        {
            _LevelsRepository.Add(level);
        }
        public void Update(Levels level)
        {
            _LevelsRepository.Update(level);
        }
        public List<ShowLevelDTO> GetAllLevels()
        {
            var levels = _LevelsRepository.GetAll();
            return _mapper.Map<List<ShowLevelDTO>>(levels);
        }
    }
}
