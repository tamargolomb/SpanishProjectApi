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
        private readonly IFileService _fileService;
        private readonly ICategoriesRepository _categoryRepository;
        private readonly ILevelsRepository _levelRepository;
        private readonly IUsersRepository _userRepository;

        public LessonService(ILessonsRepository lessonsRepository, IMapper mapper, IImageService imageService, ICategoriesRepository categoryRepository, ILevelsRepository levelRepository,IFileService fileService, IUsersRepository userRepository)
        {
            _LessonsRepository = lessonsRepository;
            _mapper = mapper;
            _imageService = imageService;
            _fileService = fileService;
            _categoryRepository = categoryRepository;
            _levelRepository = levelRepository;
            _userRepository = userRepository;
        }
        public ShowLessonDTO AddLesson(AddLessonDTO lessonDTO, int userId)
        {
            if (!_categoryRepository.Exists(lessonDTO.CategoryId))
                throw new BusinessRuleException("The selected category does not exist");

            if (!_levelRepository.Exists(lessonDTO.LevelId))
                throw new BusinessRuleException("The selected level does not exist");

            var lesson = _mapper.Map<Lesson>(lessonDTO);
            lesson.UserId = userId;

            try
            {
                if (lessonDTO.image != null)
                {
                    lesson.image = _imageService.Save(lessonDTO.image);
                }
                else//אם המשתמש לא העלה תמונה , נשתמש בתמונה ברירת מחדל
                {
                    lesson.image = "default-lesson.png";
                }

                if (lessonDTO.File != null)
                {
                    lesson.FileUrl = _fileService.Save(lessonDTO.File);
                }

                _LessonsRepository.Add(lesson);
            }
            catch
            {
                //למנוע מחיקה של תמונת הברירת מחדל
                    if (!string.IsNullOrEmpty(lesson.image) &&
                        lesson.image != "default-lesson.png")
                    {
                        _imageService.Delete(lesson.image);
                    }


                if (lesson.FileUrl != null)
                    _fileService.Delete(lesson.FileUrl);

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
        public bool UpdateLesson(int id)
        {
            var lesson = _LessonsRepository.GetById(id);
            if (lesson == null)
                return false;

            lesson.ViewCount++;

            _LessonsRepository.Update(lesson);
            return true;
        }
        public List<ShowLessonDTO> GetLessons(int? levelId = null, int? categoryId = null)
        {
            var lessons = _LessonsRepository.GetAll();

            if (levelId.HasValue)
                lessons = lessons.Where(l => l.LevelId == levelId.Value).ToList();

            if (categoryId.HasValue)
                lessons = lessons.Where(l => l.CategoryId == categoryId.Value).ToList();

            return _mapper.Map<List<ShowLessonDTO>>(lessons);
        }
        public bool DeleteLesson(int id, int userId)
        {
            Lesson? lesson=null;
            var user = _userRepository.GetById(userId);
            if (user?.Role == "Admin")
            {
                 lesson = _LessonsRepository.GetById(id);
            }
            else
            {
                lesson = _LessonsRepository.GetOwned(id, userId);
            }

            if (lesson is null)
                return false;

            var imageName = lesson.image;
            var fileName = lesson.FileUrl;

            bool deleted = _LessonsRepository.Delete(id);

            if (deleted)
            {
                //למנוע מחיקה של תמונת הברירת מחדל
                if (!string.IsNullOrEmpty(lesson.image) &&
                    lesson.image != "default-lesson.png")
                {
                    _imageService.Delete(lesson.image);
                }

                if (!string.IsNullOrEmpty(fileName))
                    _fileService.Delete(fileName);
            }

            return deleted;
        }
        public List<ShowLessonDTO> GetMine(int userId)
        {
            var lessons = _LessonsRepository.GetAll()
                .Where(l => l.UserId == userId)
                .ToList();

            return _mapper.Map<List<ShowLessonDTO>>(lessons);
        }
    }
}
