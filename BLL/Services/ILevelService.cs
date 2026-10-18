using DAL;
using DTO;
using DTO.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface ILevelService
    {
        void Add(Levels level);
        void Update(Levels level);
        List<ShowLevelDTO> GetAllLevels();
    }
}
