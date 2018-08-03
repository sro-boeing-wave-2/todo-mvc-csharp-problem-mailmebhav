using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesApi.Models
{
    public class Notes
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public List<Checklists> Checklists { get; set; }
        public List<Labels> Labels { get; set; }
        public bool Pinned { get; set; }
    }
}
