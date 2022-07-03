using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithData.Models
{
    public record Project
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public int TeamId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User Author { get; set; }
        public virtual IEnumerable<TaskDTO> TasksDTO { get; set; }
        public Team Team { get; set; }
    }
}
