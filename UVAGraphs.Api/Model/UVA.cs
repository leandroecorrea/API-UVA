namespace UVAGraphs.Api.Model;
using System.ComponentModel.DataAnnotations;

public record UVA
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; init; }
    public float Value { get; init; }
} 