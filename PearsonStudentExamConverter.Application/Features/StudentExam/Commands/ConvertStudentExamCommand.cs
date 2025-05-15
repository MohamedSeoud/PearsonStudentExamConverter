using MediatR;
using Microsoft.AspNetCore.Http;
using PearsonStudentExamConverter.Core.Entities;

namespace PearsonStudentExamConverter.Application.Features.StudentExam.Commands;

public class ConvertStudentExamCommand : IRequest<ConvertStudentExamCommandResponse>
{
    public IFormFile CsvFile { get; set; }
}

public class ConvertStudentExamCommandResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public List<Student>? Students { get; set; }
    public List<string>? Errors { get; set; }
}