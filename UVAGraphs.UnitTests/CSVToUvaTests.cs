using Xunit;
using UVAGraphs.Api.Model;
using System.Collections.Generic;
using System;

namespace UVAGraphs.UnitTests;

public class CSVToUvaTests
{
    [Fact]
    //UnitOfWork_StateUnderTest_ExpectedBehaviour
    public void GetInstance_AtCall_ShouldNotReturnNull()
    {
        //Arrange
        var instance = CsvToUVA.GetInstance();
        //Act

        //Assert
        Assert.NotNull(instance);
    }

    [Fact]
    public void GetUVAs_AtCall_ReturnsValidDataType()
    {
        var list = CsvToUVA.GetInstance().GetUVAs();
        Assert.IsType<List<UVA>>(list);
    }
    [Fact]
    public void GetUVAs_AtCall_FirstIndexValueIsCorrect()
    {
        var first = CsvToUVA.GetInstance().GetUVAs()[0].Value;
        Assert.Equal(first, 14.05f);
    }
    [Fact]
    public void GetUVAs_AtCall_FirstIndexDateIsCorrect()
    {
        var first = CsvToUVA.GetInstance().GetUVAs()[0].Date;
        Assert.Equal(first, new DateTime(2016, 3, 31));
    }
    [Fact]
    public void GetUVAs_AtCall_LastIndexValueIsCorrect()
    {
        var list = CsvToUVA.GetInstance().GetUVAs();
        var last = list[list.Count - 1].Value;
        Assert.Equal(last, 102.39f);
    }
    [Fact]
    public void GetUVAs_AtCall_LastIndexDateIsCorrect()
    {
        var list = CsvToUVA.GetInstance().GetUVAs();
        var last = list[list.Count - 1].Date;
        Assert.Equal(last, new DateTime(2022, 2, 15));
    }
}