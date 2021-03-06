﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;
using NotesAPI.Services;

namespace NotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesAPIContext _context;
        private INotesService _NotesServices;

        public NotesController(INotesService notesService)
        {
            _NotesServices = notesService;
        }

        //GET: api/Notes or GET: api/Notes?{query}
        [HttpGet]
        public async Task<IActionResult> GetNotes([FromQuery] string title = null, [FromQuery] string label = null, [FromQuery] bool? pinned = null)
        {
            var result = await _NotesServices.GetNotes(title, label, pinned);
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

            var notes = await _NotesServices.GetNotesservice(id);

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
                await _NotesServices.PutNotes(notes);
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

            await _NotesServices.PostNotes(notes);

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
            var notes = await _NotesServices.DeleteNotes(id);
            if (notes == null)
            {
                return NotFound();
            }

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
            var notes = await _NotesServices.DeleteNotes(title);
            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }

        private bool NotesExists(int id)
        {
            var result = _NotesServices.NotesExists(id);
            return result;
        }
    }
}