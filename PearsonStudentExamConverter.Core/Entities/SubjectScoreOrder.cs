namespace PearsonStudentExamConverter.Core.Entities;

public static class SubjectScoreOrder
{
    public static readonly Dictionary<string, List<string>> ScoreOrders = new()
    {
        { "English", new List<string> { "8", "7", "6", "5", "4", "3", "2", "1" } },
        { "Maths", new List<string> { "A", "B", "C", "D", "E", "F" } },
        { "Science", new List<string> { "Excellent", "Good", "Average", "Poor", "Very Poor" } }
    };
}