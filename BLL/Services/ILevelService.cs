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
        ShowLevelDTO AddLevel(AddLessonDTO dto);
        ShowLevelDTO UpdateLevel(int id, ShowLevelDTO dto);
        List<ShowLevelDTO> GetAllLevels();
        ShowLevelDTO GetLevelById(int id);
    }
}
