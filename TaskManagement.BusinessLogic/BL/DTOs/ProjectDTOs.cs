namespace TaskManagement.BusinessLogic.BL.DTOs
{
    public class ProjectDTOs
    {
        public class Request
        {
            public abstract class Base
            {
                public string Name { get; set; } = null!;
                public string? Description { get; set; }
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
