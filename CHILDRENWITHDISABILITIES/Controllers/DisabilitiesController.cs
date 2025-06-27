using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChildrenWithDisabilitiesAPI.Data;
using ChildrenWithDisabilitiesAPI.Models;
using ChildrenWithDisabilitiesAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ChildrenWithDisabilitiesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔐 Require authentication for ALL endpoints in this controller
    public class DisabilitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DisabilitiesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Disabilities
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Disabilities.ToListAsync());
        }

        // ✅ POST: api/Disabilities
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

        // ✅ PUT: api/Disabilities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DisabilityDTO dto)
        {
            var disability = await _context.Disabilities.FindAsync(id);
            if (disability == null)
                return NotFound();

            disability.FirstName = dto.FirstName;
            disability.LastName = dto.LastName;
            disability.Gender = dto.Gender;
            disability.Age = dto.Age;
            disability.Adds = dto.Adds;
            disability.District = dto.District;
            disability.DisabilityType = dto.DisabilityType;
            disability.Contact = dto.Contact;
            disability.PhotoPath = dto.PhotoPath;

            await _context.SaveChangesAsync();

            return Ok(disability);
        }

        // ✅ DELETE: api/Disabilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var disability = await _context.Disabilities.FindAsync(id);
            if (disability == null)
                return NotFound();

            _context.Disabilities.Remove(disability);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ GET: api/Disabilities/distribution
        [HttpGet("distribution")]
        public async Task<IActionResult> GetDisabilityDistribution()
        {
            var result = await _context.Disabilities
                .GroupBy(d => d.DisabilityType)
                .Select(g => new
                {
                    DisabilityType = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
            return Ok(result);
        }

        // ✅ GET: api/Disabilities/chart
        [HttpGet("chart")]
        public async Task<IActionResult> GetChartData()
        {
            var chartData = await _context.Disabilities
                .GroupBy(d => d.District)
                .Select(g => new
                {
                    District = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
            return Ok(chartData);
        }

        // ✅ GET: api/Disabilities/search
        [HttpGet("search")]
        public async Task<IActionResult> Search(string firstname = "", string lastname = "")
        {
            var results = await _context.Disabilities
                .Where(d => d.FirstName.Contains(firstname) && d.LastName.Contains(lastname))
                .ToListAsync();

            return Ok(results);
        }
    }
}
