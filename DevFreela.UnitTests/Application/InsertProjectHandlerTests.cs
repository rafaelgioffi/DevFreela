using DevFreela.Application.Commands.InsertProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using MediatR;
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

            var command = new InsertProjectCommand
            {
                Title = "Project A",
                Description = "Descrição do projeto...",
                TotalCost = 20000,
                IdClient = 1,
                IdFreelancer = 2
            };

            var handler = new InsertProjectHandler(mediator, repository);

            //Act
            var result = await handler.Handle(command, new CancellationToken());

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ID, result.Data);

            await repository.Received(1).Add(Arg.Any<Project>());
        }
    }
}
