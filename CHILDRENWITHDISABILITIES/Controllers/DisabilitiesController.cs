using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChildrenWithDisabilitiesAPI.Data;
using ChildrenWithDisabilitiesAPI.Models;
using ChildrenWithDisabilitiesAPI.DTOs;

namespace ChildrenWithDisabilitiesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisabilitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DisabilitiesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Disabilities.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(DisabilityDTO dto)
        {
            var disability = new Disability
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Age = dto.Age,
                Adds = dto.Adds,
                District = dto.District,
                DisabilityType = dto.DisabilityType,
                Contact = dto.Contact,
                PhotoPath = dto.PhotoPath
            };

            _context.Disabilities.Add(disability);
            await _context.SaveChangesAsync();

            return Ok(disability);
        }
    }
}
