using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.BusinessLogic.BL;
using TaskManagement.BusinessLogic.BL.DTOs;
using TaskManagement.DataAccessLayer.Entities.Projects;
using static TaskManagement.Framework.Statics.UserStatics;

namespace TaskManagement.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController(ProjectBL projectBL) : ControllerBase
    {
        /// <summary>
        /// Gets Projects
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Project>))]
        [HttpGet("Get")]
        public async Task<IActionResult> Get(int skip = 0, int take = -1)
        {
            List<Project> projects = await projectBL.Get(skip, take);
            return Ok(projects);
        }

        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [HttpPut("Create")]
        public async Task<IActionResult> Create(ProjectDTOs.Request.Create dto)
        {

            Guid id = await projectBL.Create(dto);
            return Ok(id);
        }
        /// <summary>
        /// Updates a project
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [HttpPost("Update")]
        public async Task<IActionResult> Update(ProjectDTOs.Request.Update dto)
        {
            Guid id = await projectBL.Update(dto);
            return Ok(id);
        }

        /// <summary>
        /// Deletes a project (Only users with Admin role can delete)
        /// </summary>
        /// <param name="Id">project's Id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [HttpDelete("Delete/{Id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Delete(Guid Id)
        {
            await projectBL.Delete(Id);
            return Ok("Project has been Deleted");
        }
    }
}
