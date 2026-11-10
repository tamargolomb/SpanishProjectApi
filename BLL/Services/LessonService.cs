using AutoMapper;
using DAL;
using DAL.Repositories;
using DTO;
using DTO.Lesson;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;
using BLL.Exceptions;

namespace BLL.Services
{
    public class LessonService: ILessonService
    {
        private readonly ILessonsRepository _LessonsRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly ICategoriesRepository _categoryRepository;
        public LessonService(ILessonsRepository lessonsRepository, IMapper mapper, IImageService imageService, ICategoriesRepository categoryRepository)
        {
            _LessonsRepository = lessonsRepository;
            _mapper = mapper;
            _imageService = imageService;
            _categoryRepository = categoryRepository;
        }
        public ShowLessonDTO AddLesson(AddLessonDTO lessonDTO)
        {

            if (!_categoryRepository.Exists(lessonDTO.CategoryId))
                throw new BusinessRuleException("The selected category does not exist");

            var lesson = _mapper.Map<Lesson>(lessonDTO);

            if (lessonDTO.image != null)
            {
                lesson.image = _imageService.Save(lessonDTO.image);
            }
            
            try
            {
                _LessonsRepository.Add(lesson);
            }
            catch
            {
                if (lesson.image != null)
                    _imageService.Delete(lesson.image);

                throw;
            }

            return _mapper.Map<ShowLessonDTO>(lesson);
        }
        
        public ShowLessonDTO GetLessonById(int id)
        {
            var lesson = _LessonsRepository.GetById(id);

            if (lesson == null)
                throw new NotFoundException($"lesson with id {id} was not found");

            return _mapper.Map<ShowLessonDTO>(lesson);
        }
        public ShowLessonDTO UpdateLesson(int id, ShowLessonDTO dto)
        {
            var existingLesson = _LessonsRepository.GetById(id);
            if (existingLesson == null)
                return null;

            _mapper.Map(dto, existingLesson);
            _LessonsRepository.Update(existingLesson);

            return _mapper.Map<ShowLessonDTO>(existingLesson);
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
