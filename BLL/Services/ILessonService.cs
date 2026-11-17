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
        ShowLessonDTO AddLesson(AddLessonDTO lessonDTO, int userId);
        ShowLessonDTO GetLessonById(int id);
        bool UpdateLesson(int id);
        List<ShowLessonDTO> GetLessons(int? levelId = null, int? categoryId = null);
        bool DeleteLesson(int id, int userId);
        //List<ShowLessonDTO> SearchLessons(string text, int max);
    }
}
