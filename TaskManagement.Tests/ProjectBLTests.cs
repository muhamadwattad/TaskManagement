using FluentAssertions;
using Moq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagement.BusinessLogic.BL;
using TaskManagement.BusinessLogic.BL.DTOs;
using TaskManagement.DataAccessLayer.Entities.Projects;
using TaskManagement.DataAccessLayer.Repositories.Interfaces;

namespace TaskManagement.Tests
{
    public class ProjectBLTests
    {
        private readonly ProjectBL _projectBL;
        private readonly FakeUnitOfWork _fakeUnitOfWork;

        public ProjectBLTests()
        {
            _fakeUnitOfWork = new FakeUnitOfWork();
            _projectBL = new ProjectBL(_fakeUnitOfWork.UnitOfWork.Object);
        }

        /// <summary>
        /// Should return not Empty Id
        /// </summary>
        /// <returns></returns>

        [Fact]
        public async Task Create_ShouldReturnProjectId_WhenProjectIsCreated()
        {
            // Arrange
            var createDto = new ProjectDTOs.Request.Create { Name = "Project 1", Description = "Project desc 1" };
            _fakeUnitOfWork.ProjectRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Project>()))
                .Returns(Task.CompletedTask);

            _fakeUnitOfWork.UnitOfWork
          .Setup(u => u.Commit(It.IsAny<CancellationToken?>()))
          .ReturnsAsync(1); // Returning a dummy int value


            // Act
            var result = await _projectBL.Create(createDto);

           
            // Assert
            result.Should().NotBeEmpty();
            _fakeUnitOfWork.ProjectRepository.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Once);
        }


        /// <summary>
        /// Should turn the project into Active False
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Delete_ShouldSetProjectInactive()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project("Project 1", "Project desc 1") { Id = projectId, Active = true };

            _fakeUnitOfWork.ProjectRepository
                .Setup(repo => repo.GetByIdAsync(projectId))
                .ReturnsAsync(project);

            _fakeUnitOfWork.UnitOfWork
    .Setup(u => u.Commit(It.IsAny<CancellationToken?>()))
    .ReturnsAsync(1); 


            // Act
            await _projectBL.Delete(projectId);

            // Assert
            project.Active.Should().BeFalse();
            _fakeUnitOfWork.ProjectRepository.Verify(repo => repo.UpdateAsync(project), Times.Once);
        }


        /// <summary>
        /// Should turn the project into its updated data
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Update_ShouldModifyProjectDetails()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var updateDto = new ProjectDTOs.Request.Update
            {
                Id = projectId,
                Name = "Updated Project Name",
                Description = "Updated Project Description",
                Active = true
            };

            var project = new Project("Original Name", "Original Description") { Id = projectId, Active = false };

            _fakeUnitOfWork.ProjectRepository
                .Setup(repo => repo.GetByIdAsync(projectId))
                .ReturnsAsync(project);

            _fakeUnitOfWork.UnitOfWork
                .Setup(u => u.Commit(It.IsAny<CancellationToken?>()))
                .ReturnsAsync(1);

            // Act
            var result = await _projectBL.Update(updateDto);

            // Assert
            result.Should().Be(projectId);
            project.Name.Should().Be("Updated Project Name");
            project.Description.Should().Be("Updated Project Description");
            project.Active.Should().BeTrue();

            _fakeUnitOfWork.ProjectRepository.Verify(repo => repo.UpdateAsync(project), Times.Once);
            _fakeUnitOfWork.UnitOfWork.Verify(u => u.Commit(It.IsAny<CancellationToken?>()), Times.Once);
        }

        /// <summary>
        /// Should retrun the projects
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Get_ShouldReturnActiveProjects()
        {
            // Arrange
            var projects = new List<Project>
        {
            new Project("Project 1", "Desc 1") { Active = true },
            new Project("Project 2", "Desc 2") { Active = true }
        };

            // Mock FilterBuilder
            var mockFilterBuilder = new Mock<IFilterBuilder<Project>>();
            _fakeUnitOfWork.ProjectRepository
                .Setup(repo => repo.GetFilterBuilder)
                .Returns(mockFilterBuilder.Object);

            mockFilterBuilder.Setup(fb => fb.Add(It.IsAny<Expression<Func<Project, bool>>>()))
                             .Verifiable();



            _fakeUnitOfWork.ProjectRepository
                .Setup(repo => repo.GetAsyncWithoutSelect(
                    It.IsAny<IEnumerable<Expression<Func<Project, bool>>>>(),
                    It.IsAny<Func<IQueryable<Project>, IOrderedQueryable<Project>>>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string[]>()
                ))
                .ReturnsAsync(projects);



            // Act
            var result = await _projectBL.Get(0, 10);

            // Assert
            result.Should().HaveCount(2);

            mockFilterBuilder.Verify(fb => fb.Add(It.IsAny<Expression<Func<Project, bool>>>()), Times.Once);

        }
    }
}
