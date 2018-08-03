using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NotesApi.Models
{
    public class NotesApiContext : DbContext
    {
        public NotesApiContext (DbContextOptions<NotesApiContext> options)
            : base(options)
        {
        }

        public DbSet<Notes> Notes { get; set; }
        //public DbSet<Checklists> Checklists { get; set; }
        //public DbSet<Labels> Labels { get; set; }
    }
}
