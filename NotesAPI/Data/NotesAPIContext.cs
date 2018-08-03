using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NotesAPI.Models
{
    public class NotesAPIContext : DbContext
    {
        public NotesAPIContext (DbContextOptions<NotesAPIContext> options)
            : base(options)
        {
        }

        public DbSet<NotesAPI.Models.Note> Note { get; set; }
    }
}
