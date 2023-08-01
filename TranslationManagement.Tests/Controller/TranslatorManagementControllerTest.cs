using AutoFixture;
using Domain.Dtos;
using Domain.Enums;
using Domain.IServices;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using TranslationManagement.Api.Controlers;
using TranslationManagement.Domain.Dtos;
using Xunit;

namespace TranslationManagement.Tests.Controller
{

    public class TranslatorManagementControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITranslatorManagementService> _serviceMock;
        private readonly TranslatorManagementController _controller;
        private readonly ILogger<TranslatorManagementController> _logger;

        public TranslatorManagementControllerTest()
        {
            _fixture = new Fixture();
            _serviceMock = _fixture.Freeze<Mock<ITranslatorManagementService>>();
            _logger = Mock.Of<ILogger<TranslatorManagementController>>();
            _controller = new TranslatorManagementController(_logger, _serviceMock.Object);
        }

        [Fact]
        public void GetTranslator_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var translatorMock = _fixture.Create<PaginationDto<TranslatorModelDto>>();
            _serviceMock.Setup(x => x.GetTranslators("", 1)).Returns(translatorMock);


            //Act
            var result = _controller.GetTranslators();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ResultDto<PaginationDto<TranslatorModelDto>>>();
        }
        //when we search the name that s not exist
        [Fact]
        public void GetTranslator_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            var translatorMock = _fixture.Create<PaginationDto<TranslatorModelDto>>();
            _serviceMock.Setup(x => x.GetTranslators("dd", 1)).Returns(translatorMock);


            //Act
            var result = _controller.GetTranslators();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ResultDto<PaginationDto<TranslatorModelDto>>>();
        }

        [Fact]
        public async Task CreateTranslator_ShouldReturnOkResponse_WhenValidRequest()
        {
            //Arrange
            var request = _fixture.Create<TranslatorModelDto>();
            var response = _fixture.Create<Task<ResultDto<bool>>>();
            _serviceMock.Setup(x => x.AddTranslator(request)).Returns(response);

            //Act
            var result = await _controller.AddTranslator(request);

            //Assert
            result.Should().NotBeNull();
            _serviceMock.Verify(x => x.AddTranslator(request), Times.Once());
            result.Should();
        }

        [Fact]
        public async Task UpdateTranslator_ShouldReturnFalseResponse_WhenNotValidRequest()
        {
            //Arrange
            var id = _fixture.Create<int>();
            var status = _fixture.Create<TranslatorStatusEnum>();
            var response = _fixture.Create<Task<ResultDto<bool>>>();
            _serviceMock.Setup(x => x.UpdateTranslatorStatus(id, (int)status)).Returns(response);

            //Act
            var result = await _controller.UpdateTranslatorStatus(id, (int)status);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ResultDto<bool>>();
            _serviceMock.Verify(x => x.UpdateTranslatorStatus(id, (int)status), Times.Once());
            result.Should();
        }

    }
}
