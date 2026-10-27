using AutoMapper;
using DAL;
using DTO;
using DTO.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly SpanishDB _context;
        private readonly IMapper _mapper;

        public CategoriesController(SpanishDB context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<List<ShowCategoriesDTO>>> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            return Ok(_mapper.Map<List<ShowCategoriesDTO>>(categories));
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowCategoriesDTO>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound("הקטגוריה לא נמצאה");

            return Ok(_mapper.Map<ShowCategoriesDTO>(category));
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<ShowCategoriesDTO>> AddCategory(
            [FromBody] AddCategoriesDTO dto)
        {
            var category = _mapper.Map<Categories>(dto);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ShowCategoriesDTO>(category);

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = category.CategoryId },
                result);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(
            int id,
            [FromBody] AddCategoriesDTO dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound("הקטגוריה לא נמצאה");

            _mapper.Map(dto, category);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Categories/5
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