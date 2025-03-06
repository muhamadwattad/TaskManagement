using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.Framework.Enums;

namespace TaskManagement.BusinessLogic.BL.DTOs
{
    public class ProjectTaskDTOs
    {
        public class Request
        {
            public abstract class Base
            {
                public Guid ProjectId { get; set; }
                public string Title { get; set; } = null!;
                public string? Description { get; set; }
                public ProjectTaskEnums.TaskStatus Status { get; set; }
            }
            public class Create : Base
            {
            }

            public class Update : Base
            {
                public Guid Id { get; set; }
                public bool Active { get; set; }
            }
        }
       
    }
}
