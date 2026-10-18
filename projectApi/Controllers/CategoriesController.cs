using AutoMapper;
using BLL;
using DAL;
using DTO;
using DTO.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace projectApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
   
            private readonly SpanishDB _context;

            // מקבלים את החיבור למסד הנתונים דרך Dependency Injection.
            public CategoriesController(SpanishDB context)
            {
                _context = context;
            }

            // מחזיר את כל הקטגוריות.
            [HttpGet]
            public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
            {
                return await _context.Categories.ToListAsync();
            }

            // GET: api/Categories/3
            // מחזיר קטגוריה לפי המזהה שלה.
            [HttpGet("{id}")]
            public async Task<ActionResult<Categories>> GetCategory(int id)
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                    return NotFound("הקטגוריה לא נמצאה");

                return category;
            }

            // POST: api/Categories
            // מוסיף קטגוריה חדשה למסד הנתונים.
            [HttpPost]
            public async Task<ActionResult<Categories>> CreateCategory(
                Categories category)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetCategory),
                    new { id = category.CategoryId },
                    category);
            }

            // PUT: api/Categories/3
            // מעדכן קטגוריה קיימת.
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateCategory(
                int id, Categories category)
            {
                if (id != category.CategoryId)
                    return BadRequest("המזהים אינם תואמים");

                var existing = await _context.Categories.FindAsync(id);

                if (existing == null)
                    return NotFound("הקטגוריה לא נמצאה");

                existing.CategoryName = category.CategoryName;
                existing.CategoryDescription =
                    category.CategoryDescription;
                existing.SubCategory = category.SubCategory;

                await _context.SaveChangesAsync();

                return NoContent();
            }

            // DELETE: api/Categories/3
            // מוחק קטגוריה לפי המזהה שלה.
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteCategory(int id)
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                    return NotFound("הקטגוריה לא נמצאה");

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

} //    private readonly SpanishDB DB;
    //    private IMapper mappingProfile;
    //    public CategoriesController(SpanishDB DB, IMapper mappingProfile)
    //    {
    //        this.DB = DB;
    //        this.mappingProfile = mappingProfile;
    //    }
    //    //קבלת רשימת קטגוריות
    //    [HttpGet]
    //    public List<ShowCategoriesDTO> GetCategoriesDTOs()
    //    {
    //        return mappingProfile.Map<List<ShowCategoriesDTO>>(DB.Categories);
    //    }
    //    //רישום קטגוריה חדשה
    //    [HttpPost]
    //    public ActionResult<Categories> AddCategory(AddCategoriesDTO cat)
    //    {
    //        mappingProfile.Map<AddCategoriesDTO>(DB.Categories);
    //        return CreatedAtAction(
    //            nameof(GetCategoriByID),
    //            new { id = Categories. },
    //            cat
    //        );
    //    }
    //    //קבלת קטגוריה לפי הקוד שלה
    //    [HttpGet("{id}")]
    //    public ActionResult<ShowCategoriesDTO> GetCategoriByID(int id)
    //    {

    //    }
    //}
