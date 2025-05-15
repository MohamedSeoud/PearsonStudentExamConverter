using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using MediatR;
using PearsonStudentExamConverter.Core.Entities;

namespace PearsonStudentExamConverter.Application.Features.StudentExam.Commands;

public class ConvertStudentExamCommandHandler : IRequestHandler<ConvertStudentExamCommand, ConvertStudentExamCommandResponse>
{
    public async Task<ConvertStudentExamCommandResponse> Handle(ConvertStudentExamCommand request, CancellationToken cancellationToken)
    {
        var response = new ConvertStudentExamCommandResponse();
        var errors = new List<string>();

        try
        {
            using var streamReader = new StreamReader(request.CsvFile.OpenReadStream());
            using var csv = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower().Replace(" ", ""),
                MissingFieldFound = null,
                HeaderValidated = null,
                BadDataFound = context => errors.Add($"Bad data found: {context.RawRecord}")
            });

            csv.Context.RegisterClassMap<StudentExamRecordMap>();

            var records = csv.GetRecords<StudentExamRecord>().ToList();

            var students = records
                .GroupBy(r => new { r.StudentId, r.Name, r.Subject })
                .Select(g => new Student
                {
                    StudentId = g.Key.StudentId,
                    Name = g.Key.Name,
                    Subject = g.Key.Subject,
                    Scores = g.Select(r => new StudentScore
                    {
                        LearningObjective = r.LearningObjective,
                        Score = r.Score
                    }).ToList()
                })
                .ToList();

            // Sort scores based on subject scoring method
            foreach (var student in students)
            {
                if (SubjectScoreOrder.ScoreOrders.TryGetValue(student.Subject, out var order))
                {
                    student.Scores = student.Scores
                        .OrderByDescending(s => order.IndexOf(s.Score))
                        .ToList();
                }
            }

            response.Students = students;
            response.Success = true;
        }
        catch (Exception ex)
        {
            errors.Add($"Error processing CSV: {ex.Message}");
            response.Success = false;
            response.Message = "Error processing CSV file";
            response.Errors = errors;
        }

        return response;
    }
}

public class StudentExamRecord
{
    [Name("Student ID")]
    public int StudentId { get; set; }

    [Name("Name")]
    public string Name { get; set; } = string.Empty;

    [Name("Learning Objective")]
    public string LearningObjective { get; set; } = string.Empty;

    [Name("Score")]
    public string Score { get; set; } = string.Empty;

    [Name("Subject")]
    public string Subject { get; set; } = string.Empty;
}