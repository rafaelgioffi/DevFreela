using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.UnitTests.Fakes;
using FluentAssertions;
using MediatR;
using Moq;
using NSubstitute;

namespace DevFreela.UnitTests.Application
{
    public class InsertProjectHandlerTests
    {
        [Fact]
        public async Task InputDataAreOk_Insert_Success_NSubstitute()
        {
            const int ID = 1;
            //Arrange
            var repository = Substitute.For<IProjectRepository>();
            var mediator = Substitute.For<IMediator>();
            repository.Add(Arg.Any<Project>()).Returns(Task.FromResult(ID));

            //var command = new InsertProjectCommand
            //{
            //    Title = "Project A",
            //    Description = "Descrição do projeto...",
            //    TotalCost = 20000,
            //    IdClient = 1,
            //    IdFreelancer = 2
            //};

            var command = FakeDataHelper.CreateFakeInsertProjectCommand();

            var handler = new InsertProjectHandler(mediator, repository);

            //Act
            var result = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.True(result.IsSuccess);

            result.IsSuccess.Should().BeTrue();

            Assert.Equal(ID, result.Data);

            result.Data.Should().Be(ID);

            await repository.Received(1).Add(Arg.Any<Project>());
        }

        [Fact]
        public async Task InputDataAreOk_Insert_Success_Moq()
        {
            //Arrange
            const int ID = 1;

            var mediator = Mock.Of<IMediator>();
            // Modo 1
            //var mock = new Mock<IProjectRepository>();
            //mock.Setup(r => r.Add(It.IsAny<Project>())).ReturnsAsync(ID);

            //Modo 2
            var repository = Mock
                .Of<IProjectRepository>(r => r.Add(It.IsAny<Project>()) == Task.FromResult(ID));

            var command = new InsertProjectCommand
            {
                Title = "Project A",
                Description = "Descrição do projeto...",
                TotalCost = 20000,
                IdClient = 1,
                IdFreelancer = 2
            };

            var handler = new InsertProjectHandler(mediator, repository); // Modo 1
            //var handler = new InsertProjectHandler(mediator, mock.Object); //Modo 2

            //Act
            var result = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ID, result.Data);

            //mock.Verify(m => m.Add(It.IsAny<Project>()), Times.Once);   // Modo 1

            Mock.Get(repository).Verify(m => m.Add(It.IsAny<Project>()), Times.Once); //Modo 2
        }
    }
}
