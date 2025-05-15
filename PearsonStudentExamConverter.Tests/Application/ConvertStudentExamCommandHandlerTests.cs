using FluentAssertions;
using PearsonStudentExamConverter.Application.Features.StudentExam.Commands;
using PearsonStudentExamConverter.Core.Entities;
using Xunit;

namespace PearsonStudentExamConverter.Tests.Application;

public class ConvertStudentExamCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCsv_ReturnsCorrectJsonStructure()
    {
        // Arrange
        var csvContent = @"Student ID,Name,Learning Objective,Score,Subject
1112,John Smith,EN_1,3,English
1112,John Smith,EN_2,4,English
1112,John Smith,EN_3,2,English";

        var handler = new ConvertStudentExamCommandHandler();
        var command = new ConvertStudentExamCommand { CsvContent = csvContent };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Students.Should().HaveCount(1);

        var student = result.Students![0];
        student.StudentId.Should().Be(1112);
        student.Name.Should().Be("John Smith");
        student.Subject.Should().Be("English");
        student.Scores.Should().HaveCount(3);

        // Check sorting (English scores should be high to low)
        student.Scores[0].Score.Should().Be("4");
        student.Scores[1].Score.Should().Be("3");
        student.Scores[2].Score.Should().Be("2");
    }

    [Fact]
    public async Task Handle_MathScores_ReturnsCorrectlySorted()
    {
        // Arrange
        var csvContent = @"Student ID,Name,Learning Objective,Score,Subject
1113,Sarah Tyrell,MA_1,D,Maths
1113,Sarah Tyrell,MA_2,A,Maths
1113,Sarah Tyrell,MA_3,C,Maths";

        var handler = new ConvertStudentExamCommandHandler();
        var command = new ConvertStudentExamCommand { CsvContent = csvContent };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Students![0].Scores[0].Score.Should().Be("A");
        result.Students[0].Scores[1].Score.Should().Be("C");
        result.Students[0].Scores[2].Score.Should().Be("D");
    }

    [Fact]
    public async Task Handle_ScienceScores_ReturnsCorrectlySorted()
    {
        // Arrange
        var csvContent = @"Student ID,Name,Learning Objective,Score,Subject
1114,Tara Hayworth,SCI_1,Excellent,Science
1114,Tara Hayworth,SCI_2,Good,Science
1114,Tara Hayworth,SCI_3,Poor,Science";

        var handler = new ConvertStudentExamCommandHandler();
        var command = new ConvertStudentExamCommand { CsvContent = csvContent };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Students![0].Scores[0].Score.Should().Be("Excellent");
        result.Students[0].Scores[1].Score.Should().Be("Good");
        result.Students[0].Scores[2].Score.Should().Be("Poor");
    }

    [Fact]
    public async Task Handle_InvalidCsv_ReturnsError()
    {
        // Arrange
        var handler = new ConvertStudentExamCommandHandler();
        var command = new ConvertStudentExamCommand { CsvContent = "invalid,csv,content" };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Success.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}