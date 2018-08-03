using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using NotesApi.Models;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace NotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesApiContext _context;

        public NotesController(NotesApiContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Notes> GetNotes([FromQuery] string title)
        {
            if (Request.QueryString == null)
            {
                return _context.Notes.Include(n => n.Checklists).Include(n => n.Labels);
            }
            else
            {
                return _context.Notes.Include(n => n.Checklists).Include(n => n.Labels)
            }
        }

        //// GET: api/Notes?
        //[HttpGet]
        //public IEnumerable<Notes> GetNotes([FromQuery] string query)
        //{
        //    if (query == null)
        //    {
        //        return _context.Notes.Include(n => n.Checklists).Include(n => n.Labels);
        //    }
        //    else
        //    {
        //        foreach(String key in Req)
        //    }
        //}

        // GET: api/Notes/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _context.Notes.Include(n => n.Labels).Include(n => n.Checklists).SingleOrDefaultAsync(x => x.ID == id);
            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }

        //// GET: api/Notes/First Note
        //[HttpGet("{title:regex(^[[A-Za-z0-9 _]]*[[A-Za-z0-9]][[A-Za-z0-9 _]]*$)}")]
        //public async Task<IActionResult> GetNotes([FromRoute] string title)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var notes = await _context.Notes.Include(n => n.Labels).Include(n => n.Checklists).Where(x => x.Title.ToLower() == title.ToLower()).ToListAsync();
        //    if (notes == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(notes);
        //}

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotes([FromRoute] int id, [FromBody] Notes notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notes.ID)
            {
                return BadRequest();
            }

            _context.Entry(notes).State = EntityState.Modified;

            try
            {
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
        public async Task<IActionResult> PostNotes([FromBody] Notes notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Notes.Add(notes);
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

            var notes = await _context.Notes.FindAsync(id);
            if (notes == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(notes);
            await _context.SaveChangesAsync();

            return Ok(notes);
        }

        private bool NotesExists(int id)
        {
            return _context.Notes.Any(e => e.ID == id);
        }
    }
}