using AutoMapper;
using DAL;
using DAL.Repositories;
using DTO;
using DTO.Lesson;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class LessonService: ILessonService
    {
        private readonly ILessonsRepository _LessonsRepository;
        private readonly IMapper _mapper;
        public LessonService(ILessonsRepository lessonsRepository, IMapper mapper)
        {
            _LessonsRepository = lessonsRepository;
            _mapper = mapper;
        }
        public void Add(Lesson lesson)
        {
            _LessonsRepository.Add(lesson);
        }
        public Lesson GetById(int id)
        {
            _LessonsRepository.GetById(id);
        }
        public void Update(Lesson lesson)
        {
            _LessonsRepository.Update(lesson);
        }
        public List<ShowLessonDTO> GetAllLessons()
        {
            var lessons = _LessonsRepository.GetAll();
            return _mapper.Map<List<ShowLessonDTO>>(lessons);
        }
        
    }
}
