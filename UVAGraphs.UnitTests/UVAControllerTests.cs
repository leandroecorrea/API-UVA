using Xunit;
using UVAGraphs.Api._Controllers;
namespace UVAGraphs.UnitTests;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UVAGraphs.Api.Dtos;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using UVAGraphs.Api.Repositories;

public class UVAControllerTests
{
    private readonly Mock<ILogger<UVAController>> loggerStub = new Mock<ILogger<UVAController>>();
    private readonly Mock<IUVARepository> repositoryStub = new Mock<IUVARepository>();

    [Fact]
    public void Get_AtCall_ShouldReturnListOfUVADto()
    {   
        repositoryStub.Setup(s=> s.GetAll())
        .Returns(new List<UVADto> ());
        var controller = new UVAController(loggerStub.Object, repositoryStub.Object);

        var list = controller.Get();

        Assert.IsType<List<UVADto>>(list);
    }

    [Fact]
    public void Get_AtCall_ShouldReturnNullIfServiceSendsNull()
    {
        repositoryStub.Setup(s => s.GetAll())
        .Returns<List<UVADto>>(null);
        var controller = new UVAController(loggerStub.Object, repositoryStub.Object);
        var list = controller.Get();
        Assert.Equal(null, list);
    }

    [Theory]
    [InlineData("")]
    [InlineData("asasas")]
    [InlineData("111/a")]

    public void GetValue_WhenModelStateIsInvalid_ReturnsBadRequestResult(string badFormat)
    {
        float mockNumber = 1;
        repositoryStub.Setup(s => s.GetValueFromDate(It.IsAny<DateTime>()))
        .Returns(mockNumber);
        var controller = new UVAController(loggerStub.Object, repositoryStub.Object);
        var result = controller.GetValue(badFormat);
        var badRequestResult = (result.Result as BadRequestObjectResult).Value;        
        Assert.IsType<SerializableError>(badRequestResult);
    }

    [Theory]
    [InlineData("01/02/2020")]
    [InlineData("31/01/2020")]
    public void GetValue_WhenPassingValidParameter_ReturnsOkObjectResultAndValue(string validDate)
    {
        float mockNumber = 1000;                
        repositoryStub.Setup(s => s.GetValueFromDate(new DateTime(2020, 01, 31)))
        .Returns(mockNumber);
        repositoryStub.Setup(s => s.GetValueFromDate(new DateTime (2020, 2, 1)))
        .Returns(mockNumber);
        var controller = new UVAController(loggerStub.Object, repositoryStub.Object);
        var result = ((OkObjectResult)controller.GetValue(validDate).Result);        
        Assert.Equal(mockNumber, result.Value);
    }

    [Fact]
    public void GetValue_WhenThereIsNoValue_ReturnsNotFoundAndNull()
    {
        string outOfRangeDate = "01/01/2001";
        repositoryStub.Setup(s => s.GetValueFromDate(It.IsAny<DateTime>()))
        .Returns<float?>(null);
        var controller = new UVAController(loggerStub.Object, repositoryStub.Object);
        var result = (NotFoundObjectResult)controller.GetValue(outOfRangeDate).Result;
        var nullFloat = result.Value;
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(null, nullFloat);
    }
    
    [Theory]
    [InlineData("", "")]
    [InlineData("asas", "11/02/2019")]    
    public void GetRise_WhenModelStateIsInvalid_ReturnsBadRequestResult(string badBeginDate, string badEndDate)
    {
        (float, float) mockTuple = (10, 11);        
        float mockRise = 10;
        repositoryStub.Setup(r => r.GetValuesForRise(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
        .Returns(mockTuple);
        var controller = new UVAController(loggerStub.Object, repositoryStub.Object);
        var result = controller.GetRise(badBeginDate, badEndDate);
        var badRequestResult = (result.Result as BadRequestObjectResult).Value;
        Assert.IsType<SerializableError>(badRequestResult);
    }

    [Fact]
    public void GetRise_WhenEndDateIsBeforeBeginDate_ReturnsBadRequestResultWithMessage()
    {
        (float, float) mockTuple = (10, 11);        
        float mockRise = 10;
        repositoryStub.Setup(r => r.GetValuesForRise(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
        .Returns(mockTuple);
        var controller = new UVAController(loggerStub.Object, repositoryStub.Object);
        string beginDate = "01/01/2020";
        string endDate = "01/01/2019";
        var result = controller.GetRise(beginDate, endDate);
        var badRequestResult = (result.Result as BadRequestObjectResult).Value;        
        Assert.Equal(badRequestResult, "La fecha de inicio es posterior a la de finalización");
    }

    [Fact]
    public void GetRise_WhenPassingValidParameters_ReturnsOkObjectAndRise()
    {
        (float, float) mockTuple = (100, 120);        
        float mockRise = 20;
        repositoryStub.Setup(r => r.GetValuesForRise(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
        .Returns(mockTuple);
        var controller = new UVAController(loggerStub.Object, repositoryStub.Object);
        string beginDate = "01/01/2021";
        string endDate = "2/12/2021";
        var result = ((OkObjectResult)controller.GetRise(beginDate, endDate).Result);
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockRise, result.Value);
    }
    [Fact]
    public void GetRise_WhenPassingWrongDates_ReturnsNotFoundAndNull()
    {        
        repositoryStub.Setup(r => r.GetValuesForRise(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
        .Returns(null);
        var controller = new UVAController(loggerStub.Object, repositoryStub.Object);
        string beginDate = "01/01/2021";
        string endDate = "2/12/2021";
        var result = ((NotFoundObjectResult)controller.GetRise(beginDate, endDate).Result);
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(null, result.Value);
    }
}