using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.DataAccessLayer.Entities.Projects
{
    public class Project : Entity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public Project(string name, string? description)
        {
            Name = name;
            Description = description;
        }
    }
}
