using CsvHelper.Configuration;

namespace PearsonStudentExamConverter.Application.Features.StudentExam.Commands;

public sealed class StudentExamRecordMap : ClassMap<StudentExamRecord>
{
    public StudentExamRecordMap()
    {
        Map(m => m.StudentId).Name("Student ID");
        Map(m => m.Name).Name("Name");
        Map(m => m.LearningObjective).Name("Learning Objective");
        Map(m => m.Score).Name("Score");
        Map(m => m.Subject).Name("Subject");
    }
}