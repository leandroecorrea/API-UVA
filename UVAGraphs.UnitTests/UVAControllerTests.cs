using Xunit;
using UVAGraphs.Api._Controllers;
namespace UVAGraphs.UnitTests;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UVAGraphs.Api.Services;
using UVAGraphs.Api.Dtos;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class UVAControllerTests
{
    private readonly Mock<ILogger<UVAController>> loggerStub = new Mock<ILogger<UVAController>>();
    private readonly Mock<IUVAServices> servicesStub = new Mock<IUVAServices>();
    [Fact]
    public void Get_AtCall_ShouldReturnListOfUVADto()
    {   
        servicesStub.Setup(s=> s.GetAll())
        .Returns(new List<UVADto> ());
        var controller = new UVAController(loggerStub.Object, servicesStub.Object);

        var list = controller.Get();

        Assert.IsType<List<UVADto>>(list);
    }

    [Theory]
    [InlineData("")]
    [InlineData("asasas")]
    [InlineData("111/a")]

    public void GetValue_WhenModelStateIsInvalid_ReturnsBadRequestResult(string badFormat)
    {
        float mockNumber = 1;
        servicesStub.Setup(s => s.GetValueFromDate(It.IsAny<DateTime>()))
        .Returns(mockNumber);
        var controller = new UVAController(loggerStub.Object, servicesStub.Object);
        var result = controller.GetValue(badFormat);
        var badRequestResult = (result.Result as BadRequestObjectResult).Value;        
        Assert.IsType<SerializableError>(badRequestResult);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("asas", "11/02/2019")]    
    public void GetRise_WhenModelStateIsInvalid_ReturnsBadRequestResult(string badBeginDate, string badEndDate)
    {
        float mockNumber = 1;
        servicesStub.Setup(s => s.GetRise(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
        .Returns(mockNumber);
        var controller = new UVAController(loggerStub.Object, servicesStub.Object);
        var result = controller.GetRise(badBeginDate, badEndDate);
        var badRequestResult = (result.Result as BadRequestObjectResult).Value;
        Assert.IsType<SerializableError>(badRequestResult);
    }

    [Fact]
    public void GetRise_WhenEndDateIsBeforeBeginDate_ReturnsBadRequestResultWithMessage()
    {
        float mockNumber = 1;
        servicesStub.Setup(s => s.GetRise(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
        .Returns(mockNumber);
        var controller = new UVAController(loggerStub.Object, servicesStub.Object);
        string beginDate = "01/01/2020";
        string endDate = "01/01/2019";
        var result = controller.GetRise(beginDate, endDate);
        var badRequestResult = (result.Result as BadRequestObjectResult).Value;        
        Assert.Equal(badRequestResult, "La fecha de inicio es posterior a la de finalización");
    }

}