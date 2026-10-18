using AutoMapper;
using BLL;
using DAL;
using DTO;
using DTO.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace projectApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly SpanishDB DB;
        private IMapper mappingProfile;
        public CategoriesController(SpanishDB DB, IMapper mappingProfile)
        {
            this.DB = DB;
            this.mappingProfile = mappingProfile;
        }
        //קבלת רשימת קטגוריות
        [HttpGet]
        public List<ShowCategoriesDTO> GetCategoriesDTOs()
        {
            return mappingProfile.Map<List<ShowCategoriesDTO>>(DB.Categories);
        }
        //רישום קטגוריה חדשה
        [HttpPost]
        public ActionResult<Categories> AddCategory(AddCategoriesDTO cat)
        {
            mappingProfile.Map<AddCategoriesDTO>(DB.Categories);
            return CreatedAtAction(
                nameof(GetCategoriByID),
                new { id = Categories. },
                cat
            );
        }
        //קבלת קטגוריה לפי הקוד שלה
        [HttpGet("{id}")]
        public ActionResult<ShowCategoriesDTO> GetCategoriByID(int id)
        {

        }
    }
}
