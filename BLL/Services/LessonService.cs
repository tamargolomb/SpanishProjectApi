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
        public ShowLessonDTO Add(AddLessonDTO lessonDTO)
        {
            var lesson = _mapper.Map<Lesson>(lessonDTO);
            if (lessonDTO.FileUrl != null)
                lesson.FileUrl = _imageService.Save(lessonDTO.FileUrl);
            _LessonsRepository.Add(lesson);
            return _mapper.Map<ShowLessonDTO>(lesson);

          
           
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
