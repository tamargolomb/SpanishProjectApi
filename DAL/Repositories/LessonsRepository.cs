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
              .Include(r => r.CategoryId)
              .Include(r => r.LevelId)
              .Include(r => r.UserId)
              .ToList();
        }
        public Lesson? GetById(int id)
        {
            return _context.Lessons
              .Include(r => r.CategoryId)
              .Include(r => r.LevelId)
              .Include(r => r.UserId)
              .FirstOrDefault(r => r.LessonId == id);
        }
        public void Update(Lesson Lesson)
        {
            _context.Lessons.Update(Lesson);
            _context.SaveChanges();
        }
        public bool Delete(int id)
        {
            var lesson = _context.Lessons.Find(id);
            if (lesson == null)
                return false;

            _context.Lessons.Remove(lesson);
            _context.SaveChanges();
            return true;
        }

        public Lesson? GetOwned(int id, int userId) =>
            _context.Lessons.FirstOrDefault(r => r.LessonId == id && r.UserId == userId);

        public List<Lesson> GetByUser(int userId) =>
            _context.Lessons.Where(r => r.UserId == userId).ToList();
    }
}
