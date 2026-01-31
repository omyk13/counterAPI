using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using counterAPI.Context;
using counterAPI.Models;
using counterAPI.DTOs;

namespace counterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountersController : ControllerBase
    {
        private readonly CounterContext _context;

        public CountersController(CounterContext context)
        {
            _context = context;
        }

        // GET: api/Counters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CounterDTO>>> GetCounters()
        {
            return await _context.Counters.Select(c => CounterToDTO(c)).ToListAsync();
        }

        // GET: api/Counters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CounterDTO>> GetCounter(int id)
        {
            var counter = await _context.Counters.FindAsync(id);

            if (counter == null)
            {
                return NotFound();
            }

            return CounterToDTO(counter);
        }

        // PUT: api/Counters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCounter(int id, CounterDTO counterDTO)
        {
            if (id != counterDTO.Id)
            {
                return BadRequest();
            }

            var counter = await _context.Counters.FindAsync(id);
            if (counter == null)
            {
                return NotFound();
            }

            counter.Id = counterDTO.Id;
            counter.Name = counterDTO.Name;
            counter.Value = counterDTO.Value;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CounterExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Counters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CounterDTO>> PostCounter(CounterDTO counterDTO)
        {
            var counter = new Counter
            {
                Id = counterDTO.Id,
                Name = counterDTO.Name,
                Value = counterDTO.Value
            };

            _context.Counters.Add(counter);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCounter),
                new { id = counter.Id },
                CounterToDTO(counter)
                );
        }

        // DELETE: api/Counters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCounter(int id)
        {
            var counter = await _context.Counters.FindAsync(id);
            if (counter == null)
            {
                return NotFound();
            }

            _context.Counters.Remove(counter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CounterExists(int id)
        {
            return _context.Counters.Any(e => e.Id == id);
        }

        private static CounterDTO CounterToDTO(Counter counter) =>
            new CounterDTO
            {
                Id = counter.Id,
                Name = counter.Name,
                Value = counter.Value
            };
    }
}
