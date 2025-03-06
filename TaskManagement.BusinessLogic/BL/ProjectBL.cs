using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.BusinessLogic.BL.DTOs;
using TaskManagement.DataAccessLayer.Entities.Projects;
using TaskManagement.DataAccessLayer.Repositories.Interfaces;

namespace TaskManagement.BusinessLogic.BL
{
    public class ProjectBL
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectBL(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Delete(Guid id)
        {

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(id);
            project.Active = false;
            await _unitOfWork.Repository<Project>().UpdateAsync(project);
            await _unitOfWork.Commit();
        }

        public async Task<Guid> Create(ProjectDTOs.Request.Create dto)
        {
            Project project = new Project(dto.Name, dto.Description);
            await _unitOfWork.Repository<Project>().AddAsync(project);
            await _unitOfWork.Commit();

            return project.Id;
        }
        public async Task<Guid> Update(ProjectDTOs.Request.Update dto)
        {
            //Can be changed to mapper.

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(dto.Id);
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.Active = dto.Active;

            await _unitOfWork.Repository<Project>().UpdateAsync(project);
            await _unitOfWork.Commit();
            return project.Id;
        }

        public async Task<List<Project>> Get(int skip, int take)
        {
            var filterBuilder = _unitOfWork.Repository<Project>().GetFilterBuilder;
            filterBuilder.Add(s => s.Active);

            return await _unitOfWork.Repository<Project>().GetAsyncWithoutSelect(filterBuilder.Build(), skip: skip, take: take);
        }
    }
}
