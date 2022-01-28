// using Xunit;
// using Moq;
// using UVAGraphs.Api.Repositories;
// using System.Collections.Generic;
// using UVAGraphs.Api.Dtos;
// using System;
// using UVAGraphs.Api.Model;
// using Microsoft.EntityFrameworkCore;

// namespace UVAGraphs.UnitTests;

// public class UVARepositorysTests
// {
//     private readonly Mock<IUVAContext> contextStub = new Mock<IUVAContext>();

//     [Fact]
//     public void Get_AtCall_ShouldReturnListOfUVADto()
//     {   
//         var mockSet = new Mock<DbSet<UVA>>();        
//         var service = new UVARepository(contextStub.Object);
//         var result = service.GetAll();
//         Assert.IsType<List<UVADto>>(result);
//         Assert.Equal(1, result.Count);
//     }

//     [Fact]
//     public void GetValueFromDate_WhenPassingValidDate_ReturnsValue()
//     {
        
//         UVA mockUva = new UVA{Date = It.IsAny<DateTime>(), Value = mockValue} ;
//         contextStub.Setup(c => c.Set<DbSet<UVA>>())
//         .Returns<DbSet<UVA>>(null);
//         var service = new UVARepository(contextStub.Object);
//         var result = service.GetValueFromDate(new DateTime(2020, 1, 1));
//         Assert.Equal(mockValue, result);
//     }
//     [Fact]
//     public void GetValueFromDate_WhenPassingWrongDate_ReturnsNull()
//     {
//         contextStub.Setup(c => c.Uvas.Find(It.IsAny<DateTime>()))
//         .Returns<UVA>(null);
//         var service = new UVARepository(contextStub.Object);
//         var result = service.GetValueFromDate(DateTime.Now);
//         Assert.Equal(null, result);
//     }

//     [Fact]
//     public void GetRise_WhenPassingValidDates_ReturnsRise()
//     {        
//         float firstValue = 10f;
//         float secondValue = 11f;
//         var firstDate = new DateTime(2020, 1, 1);
//         var secondDate = new DateTime(2020, 1, 31);
//         UVA firstUva = new UVA{Date = firstDate, Value = firstValue};
//         UVA secondUva = new UVA{Date = secondDate, Value = secondValue};
//         contextStub.Setup(c => c.Uvas.Find(firstDate))
//         .Returns(firstUva);
//         contextStub.Setup(c => c.Uvas.Find(secondDate))
//         .Returns(secondUva);
//         var service = new UVARepository(contextStub.Object);
//         var result = service.GetValuesForRise(firstDate, secondDate);
//         Assert.Equal(firstValue, result.Item1);
//         Assert.Equal(secondValue, result.Item2);
//     }
    
//     [Fact]
//     public void GetRise_WhenPassingWrongDates_ReturnsNull()
//     {
//         contextStub.Setup(c => c.Uvas.Find(It.IsAny<DateTime>()))
//         .Returns<UVA>(null);
//         var service = new UVARepository(contextStub.Object);
//         var result = service.GetValuesForRise(new DateTime(2020, 1, 1), new DateTime(2020, 1, 31));
//         (float?, float?) expectedNullTuple = (null, null);
//         Assert.Equal(expectedNullTuple, result);
//     }

// }