using AutoMapper;
using BLL.Services;
using DAL;
using DTO;
using DTO.Categories;
using DTO.Lesson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace projectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _icategoryservice;

        public CategoriesController(ICategoryService icategoryservice)
        {
            _icategoryservice = icategoryservice;
        }

        [HttpGet]
        public ActionResult<List<ShowCategoriesDTO>> GetAll()
        {
            return Ok(_icategoryservice.GetAllCategories());
        }

        [HttpPost]
        public IActionResult Add([FromBody] AddCategoriesDTO dto)
        {
            return Ok(_icategoryservice.AddCategory(dto));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_icategoryservice.DeleteCategory(id))
                return NotFound();

            return NoContent();
        }
    }
}