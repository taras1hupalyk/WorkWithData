using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithData.Models
{
    public record User
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Email { get; set; }

        public DateTime RegisteredAt { get; set; }
        public DateTime BirthDay { get; set; }

        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<TaskDTO> Tasks { get; set; }
    }
}
