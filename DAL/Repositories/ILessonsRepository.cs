using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface ILessonsRepository
    {
        void Add(Lesson Lesson);
        List<Lesson> GetAll();
        Lesson GetById(int id);
        void Update(Lesson Lesson);
        void Delete(int id);
    }
}
