using Moq;
using TaskManagement.DataAccessLayer.Entities.Projects;
using TaskManagement.DataAccessLayer.Repositories.Interfaces;

namespace TaskManagement.Tests
{
    public class FakeUnitOfWork
    {
        public Mock<IRepository<Project>> ProjectRepository { get; }
        public Mock<IRepository<ProjectTask>> ProjectTaskRepository { get; }
        public Mock<IUnitOfWork> UnitOfWork { get; }

        public FakeUnitOfWork()
        {
            ProjectRepository = new Mock<IRepository<Project>>();
            ProjectTaskRepository = new Mock<IRepository<ProjectTask>>();
            UnitOfWork = new Mock<IUnitOfWork>();

            UnitOfWork.Setup(u => u.Repository<Project>()).Returns(ProjectRepository.Object);
            UnitOfWork.Setup(u => u.Repository<ProjectTask>()).Returns(ProjectTaskRepository.Object);

            // Mock Commit to return an int
            UnitOfWork.Setup(u => u.Commit(It.IsAny<CancellationToken?>())).ReturnsAsync(1);
        }
    }

}
