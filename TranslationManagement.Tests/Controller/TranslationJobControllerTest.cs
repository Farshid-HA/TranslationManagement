using AutoFixture;
using Domain.Dtos;
using Domain.IServices;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using TranslationManagement.Api.Controllers;
using TranslationManagement.Domain.Dtos;
using Xunit;

namespace TranslationManagement.Tests.Controller
{

    public class TranslationJobControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITranslationJobServices> _serviceMock;
        private readonly ILogger<TranslationJobController> _logger;
        private readonly TranslationJobController _controller;


        public TranslationJobControllerTest()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<ITranslationJobServices>>();
            _logger = Mock.Of<ILogger<TranslationJobController>>();
            _controller = new TranslationJobController(_logger, _serviceMock.Object);

        }

        [Fact]
        public void GetTranslationJob_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var translatorMock = _fixture.Create<PaginationDto<TranslationJobDto>>();
            _serviceMock.Setup(x => x.GetJobs("", 1)).Returns(translatorMock);


            //Act
            var result = _controller.GetJobs();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ResultDto<PaginationDto<TranslationJobDto>>>();
        }
        //when we search the name that s not exist
        [Fact]
        public void GetTranslationJob_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            var translatorMock = _fixture.Create<PaginationDto<TranslationJobDto>>();
            _serviceMock.Setup(x => x.GetJobs("dd", 1)).Returns(translatorMock);


            //Act
            var result = _controller.GetJobs();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ResultDto<PaginationDto<TranslationJobDto>>>();
        }

        [Fact]
        public async Task CreateTranslationJob_ShouldReturnOkResponse_WhenValidRequest()
        {
            //Arrange
            var request = _fixture.Create<TranslationJobDto>();
            var response = _fixture.Create<Task<ResultDto<bool>>>();


            _serviceMock.Setup(x => x.CreateJob(request)).Returns(response);


            //Act
            var result = await _controller.CreateJob(request);

            //Assert
            result.Should().NotBeNull();
            _serviceMock.Verify(x => x.CreateJob(request), Times.Once());
            result.Should();
        }

        [Fact]
        public async Task UpdateTranslationJob_ShouldReturnFalseResponse_WhenNotValidRequest()
        {
            //Arrange
            var updateJobStatus = _fixture.Create<UpdateJobStatus>();
            var response = _fixture.Create<Task<ResultDto<bool>>>();
            _serviceMock.Setup(x => x.UpdateJobStatus(updateJobStatus.jobId, updateJobStatus.translatorId, (int)updateJobStatus.newStatus)).Returns(response);

            //Act
            var result = await _controller.UpdateJobStatus(updateJobStatus);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ResultDto<bool>>();
            _serviceMock.Verify(x => x.UpdateJobStatus(updateJobStatus.jobId, updateJobStatus.translatorId, (int)updateJobStatus.newStatus), Times.Once());
            result.Should();
        }

    }
}
