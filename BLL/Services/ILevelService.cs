using DAL;
using DTO;
using DTO.Lesson;
using DTO.Levels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface ILevelService
    {
        ShowLevelDTO AddLevel(AddLevelDTO dto);
        ShowLevelDTO? UpdateLevel(int id, UpdateLevelDTO dto);
        List<ShowLevelDTO> GetAllLevels();
        ShowLevelDTO GetLevelById(int id);
    }
}
