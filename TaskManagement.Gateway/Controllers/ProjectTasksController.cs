using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.BusinessLogic.BL.DTOs;
using TaskManagement.BusinessLogic.BL;
using TaskManagement.DataAccessLayer.Entities.Projects;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using TaskManagement.Framework.Enums;
using static TaskManagement.Framework.Statics.UserStatics;

namespace TaskManagement.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectTasksController(ProjectTaskBL projectTaskBL) : ControllerBase
    {
        /// <summary>
        /// Gets Pro ject tasks
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProjectTask>))]
        [HttpGet("Get")]
        public async Task<IActionResult> Get(int skip = 0, int take = -1, Guid? projectId = null)
        {
            List<ProjectTask> projects = await projectTaskBL.Get(skip, take, projectId);
            return Ok(projects);
        }

        /// <summary>
        /// Creates a new project task
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [HttpPut("Create")]
        public async Task<IActionResult> Create(ProjectTaskDTOs.Request.Create dto)
        {
            Guid id = await projectTaskBL.Create(dto);
            return Ok(id);
        }
        /// <summary>
        /// Updates a project task
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProjectTaskDTOs.Request.Update dto)
        {
            Guid id = await projectTaskBL.Update(dto);
            return Ok(id);
        }

        /// <summary>
        /// Deletes a project task (Only users with Admin role can delete)
        /// </summary>
        /// <param name="Id">project's Id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [HttpDelete("Delete/{Id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(Guid Id)
        {
            await projectTaskBL.Delete(Id);
            return Ok("Project task has been Deleted");
        }
    }
}
