namespace PearsonStudentExamConverter.Core.Entities;

public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public List<StudentScore> Scores { get; set; } = new();
}

public class StudentScore
{
    public string LearningObjective { get; set; } = string.Empty;
    public string Score { get; set; } = string.Empty;
}