using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithData.Enums;

namespace WorkWithData.Models
{
    public record TaskDTO
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int PerformerId { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public TaskState State { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }

        public User Performer { get; set; }
    }
}
