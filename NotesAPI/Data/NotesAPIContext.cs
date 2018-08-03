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
        public DbSet<Note> Note { get; set; }
        public DbSet<Label> Label { get; set; }
        public DbSet<Checklist> Checklist { get; set; }
    }
}
