using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LessonsRepository: ILessonsRepository
    {
        private readonly SpanishDB _context;

        public LessonsRepository(SpanishDB context)
        {
            _context = context;
        }
        public void Add(Lesson Lesson)
        {
            _context.Lessons.Add(Lesson);
            _context.SaveChanges();
        }
        public List<Lesson> GetAll()
        {
            return _context.Lessons
              .Include(r => r.LessonId)
              .Include(r => r.LessonTitle)
                  .Include(r => r.LessonDescription)
              .Include(r => r.ContentType)
                  .Include(r => r.FileUrl)
              .Include(r => r.CategoryId)
                  .Include(r => r.LevelId)
              .Include(r => r.UserId)
                .Include(r => r.ViewCount)
                  .Include(r => r.CreatedAt)
              .ToList();
        }
        public Lesson? GetById(int id)
        {
            return _context.Lessons
              .Include(r => r.LessonId)
              .Include(r => r.LessonTitle)
                  .Include(r => r.LessonDescription)
              .Include(r => r.ContentType)
                  .Include(r => r.FileUrl)
              .Include(r => r.CategoryId)
                  .Include(r => r.LevelId)
              .Include(r => r.UserId)
                .Include(r => r.ViewCount)
                  .Include(r => r.CreatedAt)
              .FirstOrDefault(r => r.LessonId == id);
        }
        public void Update(Lesson Lesson)
        {
            _context.Lessons.Update(Lesson);
            _context.SaveChanges();
        }
    
    }
}
