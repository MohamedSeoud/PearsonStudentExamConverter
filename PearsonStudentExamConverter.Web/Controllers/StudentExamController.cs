using MediatR;
using Microsoft.AspNetCore.Mvc;
using PearsonStudentExamConverter.Application.Features.StudentExam.Commands;

namespace PearsonStudentExamConverter.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentExamController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentExamController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("convert")]
    public async Task<IActionResult> ConvertCsvToJson([FromForm] ConvertStudentExamCommand command)
    {
        if (command.CsvFile == null || command.CsvFile.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        if (!command.CsvFile.FileName.EndsWith(".csv"))
        {
            return BadRequest("Only CSV files are allowed");
        }

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(new { result.Message, result.Errors });
        }

        return Ok(result.Students);
    }
}