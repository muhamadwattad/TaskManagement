using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Framework.Enums;

namespace TaskManagement.DataAccessLayer.Entities.Projects
{
    public class ProjectTask : Entity
    {
        [ForeignKey("Project")]
        public Guid ProjectId { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public ProjectTaskEnums.TaskStatus Status { get; set; }

        public ProjectTask(Guid projectId, string title, string? description, ProjectTaskEnums.TaskStatus status)
        {
            ProjectId = projectId;
            Title = title;
            Description = description;
            Status = status;
        }

        public Project? Project { get; set; }
    }
}
