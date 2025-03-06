namespace TaskManagement.Tests
{
    using FluentAssertions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using TaskManagement.BusinessLogic.BL;
    using TaskManagement.BusinessLogic.BL.DTOs;
    using TaskManagement.DataAccessLayer.Entities.Projects;
    using TaskManagement.DataAccessLayer.Repositories.Interfaces;
    using Xunit;

    public class ProjectTaskBLTests
    {
        private readonly ProjectTaskBL _projectTaskBL;
        private readonly FakeUnitOfWork _fakeUnitOfWork;

        public ProjectTaskBLTests()
        {
            _fakeUnitOfWork = new FakeUnitOfWork();
            _projectTaskBL = new ProjectTaskBL(_fakeUnitOfWork.UnitOfWork.Object);
        }


        /// <summary>
        /// Creating a project task should not return Empty
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Create_ShouldReturnTaskId_WhenTaskIsCreated()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var createDto = new ProjectTaskDTOs.Request.Create
            {
                ProjectId = projectId,
                Title = "Task 1",
                Description = "Test Desc",
                Status = Framework.Enums.ProjectTaskEnums.TaskStatus.Todo,
            };

            _fakeUnitOfWork.ProjectRepository
                .Setup(repo => repo.Exists(It.IsAny<Expression<Func<Project, bool>>>()))
                .ReturnsAsync(true);

            _fakeUnitOfWork.ProjectTaskRepository
                .Setup(repo => repo.AddAsync(It.IsAny<ProjectTask>()))
                .Returns(Task.CompletedTask);

            _fakeUnitOfWork.UnitOfWork
                .Setup(u => u.Commit(It.IsAny<CancellationToken?>()))
                .ReturnsAsync(1);

            // Act
            var result = await _projectTaskBL.Create(createDto);

            // Assert
            result.Should().NotBeEmpty();
            _fakeUnitOfWork.ProjectTaskRepository.Verify(repo => repo.AddAsync(It.IsAny<ProjectTask>()), Times.Once);
            _fakeUnitOfWork.UnitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken?>()), Times.Once);
        }

        /// <summary>
        /// Deleting project task should turn the task into Active False
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Delete_ShouldSetTaskInactive()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var projectTask = new ProjectTask(Guid.NewGuid(), "Task 1", "Desc", Framework.Enums.ProjectTaskEnums.TaskStatus.Todo) { Id = taskId, Active = true };

            _fakeUnitOfWork.ProjectTaskRepository
                .Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync(projectTask);

            _fakeUnitOfWork.UnitOfWork
                .Setup(u => u.Commit(It.IsAny<CancellationToken?>()))
                .ReturnsAsync(1);

            // Act
            await _projectTaskBL.Delete(taskId);

            // Assert
            projectTask.Active.Should().BeFalse();
            _fakeUnitOfWork.ProjectTaskRepository.Verify(repo => repo.UpdateAsync(projectTask), Times.Once);
            _fakeUnitOfWork.UnitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken?>()), Times.Once);
        }
        /// <summary>
        /// Should turn the task into its updated data
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Update_ShouldModifyTaskDetails()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var updateDto = new ProjectTaskDTOs.Request.Update
            {
                Id = taskId,
                ProjectId = projectId,
                Title = "Updated Task",
                Description = "Updated Desc",
                Status = Framework.Enums.ProjectTaskEnums.TaskStatus.InProgress,
                Active = true
            };

            var projectTask = new ProjectTask(projectId, "Task 1", "Desc", Framework.Enums.ProjectTaskEnums.TaskStatus.Todo) { Id = taskId, Active = true };

            _fakeUnitOfWork.ProjectRepository
                .Setup(repo => repo.Exists(It.IsAny<Expression<Func<Project, bool>>>()))
                .ReturnsAsync(true);

            _fakeUnitOfWork.ProjectTaskRepository
                .Setup(repo => repo.GetByIdAsync(taskId))
                .ReturnsAsync(projectTask);

            _fakeUnitOfWork.UnitOfWork
                .Setup(u => u.Commit(It.IsAny<CancellationToken?>()))
                .ReturnsAsync(1);

            // Act
            var result = await _projectTaskBL.Update(updateDto);

            // Assert
            result.Should().Be(taskId);
            projectTask.Title.Should().Be("Updated Task");
            projectTask.Description.Should().Be("Updated Desc");
            projectTask.Status.Should().Be(Framework.Enums.ProjectTaskEnums.TaskStatus.InProgress);
            _fakeUnitOfWork.ProjectTaskRepository.Verify(repo => repo.UpdateAsync(projectTask), Times.Once);
            _fakeUnitOfWork.UnitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken?>()), Times.Once);
        }


        /// <summary>
        /// Should return the tasks
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Get_ShouldReturnActiveTasks()
        {
            // Arrange
            var tasks = new List<ProjectTask>
    {
        new ProjectTask(Guid.NewGuid(), "Task 1", "Desc", Framework.Enums.ProjectTaskEnums.TaskStatus.Todo) { Active = true },
        new ProjectTask(Guid.NewGuid(), "Task 2", "Desc", Framework.Enums.ProjectTaskEnums.TaskStatus.Done) { Active = true }
    };

            // Mock FilterBuilder
            var mockFilterBuilder = new Mock<IFilterBuilder<ProjectTask>>();
            _fakeUnitOfWork.ProjectTaskRepository
                .Setup(repo => repo.GetFilterBuilder)
                .Returns(mockFilterBuilder.Object);

            // Ensure filterBuilder.Add() can be called without throwing errors
            mockFilterBuilder.Setup(fb => fb.Add(It.IsAny<Expression<Func<ProjectTask, bool>>>()))
                             .Verifiable();

            _fakeUnitOfWork.ProjectTaskRepository
                .Setup(repo => repo.GetAsyncWithoutSelect(
                    It.IsAny<IEnumerable<Expression<Func<ProjectTask, bool>>>>(),
                    It.IsAny<Func<IQueryable<ProjectTask>, IOrderedQueryable<ProjectTask>>>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string[]>()
                ))
                .ReturnsAsync(tasks);

            // Act
            var result = await _projectTaskBL.Get(0, 10, null);

            // Assert
            result.Should().HaveCount(2);

            // Verify that `Add` was called on the filterBuilder
            mockFilterBuilder.Verify(fb => fb.Add(It.IsAny<Expression<Func<ProjectTask, bool>>>()), Times.Once);
        }

    }

}
