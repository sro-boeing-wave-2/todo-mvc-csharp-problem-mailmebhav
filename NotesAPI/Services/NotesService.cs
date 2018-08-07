using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NotesAPI.Services.NotesService;

namespace NotesAPI.Services
{
    public class NotesService : INotesService
    {
        private readonly NotesAPIContext _context;

        public NotesService(NotesAPIContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Note>> GetNotes(string title, string label, bool? pinned)
        {
            var result = await _context.Note.Include(n => n.Checklists).Include(n => n.Labels)
            .Where(x => ((title == null || x.Title == title) && (label == null || x.Labels.Any(y => y.LabelName == label)) && (pinned == null || x.Pinned == pinned))).ToListAsync();
            return result;
        }

        public async Task<Note> GetNotesservice(int id)
        {
            var notes = await _context.Note.Include(n => n.Labels).Include(n => n.Checklists).SingleOrDefaultAsync(x => x.ID == id);
            return notes;
        }

        public async Task<Note> PutNotes(Note notes)
        {
            _context.Note.Update(notes);
            await _context.SaveChangesAsync();
            return await Task.FromResult(notes);
        }

        public async Task<Note> PostNotes(Note notes)
        {
            _context.Note.Add(notes);
            await _context.SaveChangesAsync();
            return await Task.FromResult(notes);
        }

        public async Task<Note> DeleteNotes(int id)
        {
            var notes = await _context.Note.Include(x => x.Checklists).Include(x => x.Labels).SingleOrDefaultAsync(x => (x.ID == id));
            if (notes == null)
            {
                return await Task.FromResult<Note>(null);
            }
            _context.Note.Remove(notes);
            await _context.SaveChangesAsync();
            return notes;
        }

        public async Task<IEnumerable<Note>> DeleteNotes(string title)
        {
            var notes = await _context.Note.Include(x => x.Checklists).Include(x => x.Labels).Where(x => (x.Title == title)).ToListAsync();
            if (notes == null)
            {
                return await Task.FromResult<IEnumerable<Note>>(null);
            }
            _context.Note.RemoveRange(notes);
            await _context.SaveChangesAsync();

            return notes;
        }

        public bool NotesExists(int id)
        {
            var result = _context.Note.Any(e => e.ID == id);
            return result;
        }
    }

    public interface INotesService
    {
        Task<IEnumerable<Note>> GetNotes(string title, string label, bool? pinned);
        Task<Note> GetNotesservice(int id);
        Task<Note> PutNotes(Note notes);
        Task<Note> PostNotes(Note notes);
        Task<Note> DeleteNotes(int id);
        Task<IEnumerable<Note>> DeleteNotes(string title);
        bool NotesExists(int id);
    }
}
