using LearningPlan.DataAccess;
using LearningPlan.DomainModel;
using LearningPlan.DomainModel.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace LearningPlan.Services.Implementation.Tests
{
    [TestClass]
    public class PlanServiceTests
    {
        private IPlanService _planService;
        private Mock<IPlanService> _planServiceMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IReadRepository<Plan>> _planReadRepositoryMock;

        [TestInitialize]
        public void Initialize()
        {
            _planServiceMock = new Mock<IPlanService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _planReadRepositoryMock = new Mock<IReadRepository<Plan>>();

            _planService = new PlanService(null,
               null,
               _planReadRepositoryMock.Object,
               null,
               null,
               null,
               null,
               null,
               _unitOfWorkMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _planServiceMock.Verify();
        }

        [TestMethod]
        public async Task Update_Successful()
        {
            _planReadRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(new Plan(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())));
            _unitOfWorkMock.Setup(x => x.CommitAsync());

            await _planService.UpdateAsync(new Model.PlanServiceModel());
        }


        [TestMethod]
        [ExpectedException(typeof(DomainServicesException))]
        public async Task Update_PlanNotFound_Exception()
        {
            _planReadRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>()));
            await _planService.UpdateAsync(new Model.PlanServiceModel());
        }
    }
}
