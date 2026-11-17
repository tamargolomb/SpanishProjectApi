using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface IFileService
    {
        string Save(IFormFile file);
        string GetFilePath(string fileName);
        //הזרמה
        Stream GetFileStream(string fileName);
        //קבלת סוג הקובץ לפי סיומת
        string GetContentType(string fileName);
        void Delete(string fileName);
    }
}
