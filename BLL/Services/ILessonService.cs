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
        void Add(Lesson lesson);
        Lesson GetById(int id);
        void Update(Lesson lesson);
        List<ShowLessonDTO> GetAllLessons();
    }
}
