using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Internal;

namespace NotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesAPIContext _context;

        public NotesController(NotesAPIContext context)
        {
            _context = context;
        }

        //GET: api/Notes or GET: api/Notes?{query}
        [HttpGet]
        public async Task<IActionResult> GetNotes([FromQuery] string title, [FromQuery] string label, [FromQuery] bool? pinned)
        {
            var result = await _context.Note.Include(n => n.Checklists).Include(n => n.Labels)
                .Where(x => ((title == null || x.Title == title) && (label == null || x.Labels.Any(y => y.LabelName == label)) && (pinned == null || x.Pinned == pinned))).ToListAsync();
            return Ok(result);
        }

        // GET: api/Notes/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _context.Note.Include(n => n.Labels).Include(n => n.Checklists).SingleOrDefaultAsync(x => x.ID == id);
            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotes([FromRoute] int id, [FromBody] Note notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notes.ID)
            {
                return BadRequest();
            }
            try
            {
                _context.Note.Update(notes);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNotes([FromBody] Note notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Note.Add(notes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotes", new { id = notes.ID }, notes);
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var notes = await _context.Note.Include(x => x.Checklists).Include(x => x.Labels).SingleOrDefaultAsync(x => (x.ID == id));
            if (notes == null)
            {
                return NotFound();
            }

            _context.Note.Remove(notes);
            await _context.SaveChangesAsync();

            return Ok(notes);
        }

        // DELETE: api/Notes/?{query}
        [HttpDelete]
        public async Task<IActionResult> DeleteNotes([FromQuery] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var notes = await _context.Note.Include(x => x.Checklists).Include(x => x.Labels).Where(x => (x.Title == title)).ToListAsync();
            if (notes == null)
            {
                return NotFound();
            }
            _context.Note.RemoveRange(notes);
            await _context.SaveChangesAsync();

            return Ok(notes);
        }

        private bool NotesExists(int id)
        {
            return _context.Note.Any(e => e.ID == id);
        }
    }
}