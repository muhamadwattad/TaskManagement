using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.BusinessLogic.BL.DTOs;
using TaskManagement.DataAccessLayer.Entities.Projects;
using TaskManagement.DataAccessLayer.Repositories.Interfaces;
using TaskManagement.Framework.Exceptions;

namespace TaskManagement.BusinessLogic.BL
{
    public class ProjectTaskBL
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectTaskBL(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Delete(Guid id)
        {
            var projectTask = await _unitOfWork.Repository<ProjectTask>().GetByIdAsync(id);
            projectTask.Active = false;

            await _unitOfWork.Repository<ProjectTask>().UpdateAsync(projectTask);
            await _unitOfWork.Commit();
        }

        public async Task<Guid> Update(ProjectTaskDTOs.Request.Update dto)
        {
            //Can be changed to mapper.

            if (!await _unitOfWork.Repository<Project>().Exists(s => s.Active && s.Id == dto.ProjectId))
                throw new NotFoundException("Project was not found");

            var projectTask = await _unitOfWork.Repository<ProjectTask>().GetByIdAsync(dto.Id);

            projectTask.ProjectId = dto.ProjectId;
            projectTask.Description = dto.Description;
            projectTask.Active = dto.Active;
            projectTask.Status = dto.Status;
            projectTask.Title = dto.Title;

            await _unitOfWork.Repository<ProjectTask>().UpdateAsync(projectTask);
            await _unitOfWork.Commit();
            return projectTask.Id;
        }
        public async Task<Guid> Create(ProjectTaskDTOs.Request.Create dto)
        {
            if (!await _unitOfWork.Repository<Project>().Exists(s => s.Active && s.Id == dto.ProjectId))
                throw new NotFoundException("Project was not found");

            ProjectTask projectTask = new ProjectTask(dto.ProjectId, dto.Title, dto.Description, dto.Status);
            await _unitOfWork.Repository<ProjectTask>().AddAsync(projectTask);
            await _unitOfWork.Commit();

            return projectTask.Id;
        }

        public async Task<List<ProjectTask>> Get(int skip, int take, Guid? projectId)
        {
            var filterBuilder = _unitOfWork.Repository<ProjectTask>().GetFilterBuilder;
            filterBuilder.Add(s => s.Active);

            if (projectId.HasValue)
                filterBuilder.Add(s => s.ProjectId == projectId);


            return await _unitOfWork.Repository<ProjectTask>().GetAsyncWithoutSelect(filterBuilder.Build(), skip: skip, take: take);
        }

    }
}
