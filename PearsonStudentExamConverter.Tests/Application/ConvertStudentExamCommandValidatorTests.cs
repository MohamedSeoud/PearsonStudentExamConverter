using FluentValidation.TestHelper;
using PearsonStudentExamConverter.Application.Features.StudentExam.Commands;
using Xunit;

namespace PearsonStudentExamConverter.Tests.Application;

public class ConvertStudentExamCommandValidatorTests
{
    private readonly ConvertStudentExamCommandValidator _validator = new();

    [Fact]
    public void Validate_EmptyContent_ShouldHaveValidationError()
    {
        var command = new ConvertStudentExamCommand { CsvContent = string.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CsvContent);
    }

    [Fact]
    public void Validate_ShortContent_ShouldHaveValidationError()
    {
        var command = new ConvertStudentExamCommand { CsvContent = "short" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CsvContent);
    }

    [Fact]
    public void Validate_ValidContent_ShouldNotHaveValidationError()
    {
        var command = new ConvertStudentExamCommand { CsvContent = "Student ID,Name,Learning Objective,Score,Subject\n1112,John Smith,EN_1,3,English" };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}