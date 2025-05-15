using FluentValidation;
using System.Linq;

namespace PearsonStudentExamConverter.Application.Features.StudentExam.Commands;

public class ConvertStudentExamCommandValidator : AbstractValidator<ConvertStudentExamCommand>
{
    public ConvertStudentExamCommandValidator()
    {
        RuleFor(x => x.CsvFile)
            .NotNull().WithMessage("CSV file is required")
            .Must(file => file.Length > 0).WithMessage("File is empty")
            .Must(file => file.FileName.EndsWith(".csv")).WithMessage("Only CSV files are allowed")
            .Must(file => file.Length <= 5 * 1024 * 1024).WithMessage("File size must be less than 5MB");

        RuleFor(x => x.CsvFile)
            .Custom((file, context) => {
                if (file != null)
                {
                    var allowedContentTypes = new[] { "text/csv", "application/vnd.ms-excel" };
                    if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
                    {
                        context.AddFailure("Invalid file type. Only CSV files are allowed");
                    }
                }
            });
    }
}