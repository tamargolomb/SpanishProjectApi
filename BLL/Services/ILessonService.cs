using DAL;
using DTO;
using DTO.Lesson;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface ILessonService
    {
        ShowLessonDTO AddLesson(AddLessonDTO lessonDTO);
        ShowLessonDTO GetLessonById(int id);
        ShowLessonDTO UpdateLesson(int id, ShowLessonDTO dto);
        List<ShowLessonDTO> GetAllLessons();
        bool DeleteLesson(int id, int userId);
    }
}
