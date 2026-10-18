using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public interface ILevelsRepository
    {
        void Add(Levels Level);
        List<Levels> GetAll();
        void Update(Levels Level);
     
    }
}
