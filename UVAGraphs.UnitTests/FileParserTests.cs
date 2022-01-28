// using Xunit;
// using UVAGraphs.Api.Model;
// using System.Collections.Generic;
// using System;
// using UVAGraphs.Api.Utils;

// namespace UVAGraphs.UnitTests;

// public class CSVToUvaTests
// {    

//     [Fact]
//     public void GetUVAs_AtCall_ReturnsValidDataType()
//     {
//         var list = FileParser.ParseFile();
//         Assert.IsType<List<UVA>>(list);
//     }
//     [Fact]
//     public void GetUVAs_AtCall_FirstIndexValueIsCorrect()
//     {
//         var first = FileParser.ParseFile()[0].Value;
//         Assert.Equal(first, 14.05f);
//     }
//     [Fact]
//     public void GetUVAs_AtCall_FirstIndexDateIsCorrect()
//     {
//         var first = FileParser.ParseFile()[0].Date;
//         Assert.Equal(first, new DateTime(2016, 3, 31));
//     }
//     [Fact]
//     public void GetUVAs_AtCall_LastIndexValueIsCorrect()
//     {
//         var list = FileParser.ParseFile();
//         var last = list[list.Count - 1].Value;
//         Assert.Equal(last, 102.39f);
//     }
//     [Fact]
//     public void GetUVAs_AtCall_LastIndexDateIsCorrect()
//     {
//         var list = FileParser.ParseFile();
//         var last = list[list.Count - 1].Date;
//         Assert.Equal(last, new DateTime(2022, 2, 15));
//     }
// }