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
        private readonly IImageService _imageService;

        public LessonService(ILessonsRepository lessonsRepository, IMapper mapper, IImageService imageService)
        {
            _LessonsRepository = lessonsRepository;
            _mapper = mapper;
            _imageService = imageService;
        }
        public ShowLessonDTO AddLesson(AddLessonDTO lessonDTO)
        {
            var lesson = _mapper.Map<Lesson>(lessonDTO);
            if (lessonDTO.FileUrl != null)
                lesson.FileUrl = _imageService.Save(lessonDTO.image);
            _LessonsRepository.Add(lesson);
            return _mapper.Map<ShowLessonDTO>(lesson);

          
           
        }
        public ShowLessonDTO GetLessonById(int id)
        {
            var lesson = _LessonsRepository.GetById(id);

            if (lesson == null)
                throw new Exception($"Lesson with id {id} was not found");

            return _mapper.Map<ShowLessonDTO>(lesson);
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
        public bool DeleteLesson(int id, int userId)
        {
            var lesson = _LessonsRepository.GetOwned(id, userId);
            if (lesson is null)
                return false;

            _LessonsRepository.Delete(id);
            return true;
        }

    }
}
