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
        public void Add(Lesson lesson)
        {
            var user = _context.Users.Find(lesson.UserId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            _context.Lessons.Add(lesson);

            user.Score += 10;

            _context.SaveChanges();
        }
        public List<Lesson> GetAll()
        {
            return _context.Lessons
              .Include(r => r.Category)
              .Include(r => r.Level)
              .Include(r => r.user)
              .ToList();
        }
        public Lesson? GetById(int id)
        {
            return _context.Lessons
              .Include(r => r.Category)
              .Include(r => r.Level)
              .Include(r => r.user)
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
        public List<Lesson> SearchByName(string text, int max)
        {
            return _context.Lessons
       .Where(r => r.LessonTitle.Contains(text))
       .OrderBy(r => r.LessonTitle)
       .Take(max)
       .ToList();
        }
    }
}
