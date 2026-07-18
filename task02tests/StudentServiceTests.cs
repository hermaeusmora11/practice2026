using System.Collections.Generic;
using System.Linq;
using Xunit;
using task02;   // Пространство имён вашего проекта

namespace task02tests;

public class StudentServiceTests
{
    private readonly List<Student> _students;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        // Тестовые данные – другие студенты, факультеты и оценки
        _students = new List<Student>
        {
            new Student { Name = "Алексей",   Faculty = "Математика",   Grades = new List<int> { 4, 5, 4, 5 } },
            new Student { Name = "Екатерина", Faculty = "Информатика",  Grades = new List<int> { 3, 3, 4, 3 } },
            new Student { Name = "Сергей",    Faculty = "Математика",   Grades = new List<int> { 5, 5, 5, 5 } },
            new Student { Name = "Ольга",     Faculty = "Физика",       Grades = new List<int> { 4, 4, 4, 4 } },
            new Student { Name = "Дмитрий",   Faculty = "Информатика",  Grades = new List<int> { 5, 4, 5, 4 } }
        };
        _service = new StudentService(_students);
    }

    [Fact]
    public void GetStudentsByFaculty_ShouldReturnOnlyStudentsFromGivenFaculty()
    {
        // Act
        var result = _service.GetStudentsByFaculty("Математика").ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, s => Assert.Equal("Математика", s.Faculty));
        Assert.Contains(result, s => s.Name == "Алексей");
        Assert.Contains(result, s => s.Name == "Сергей");
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ShouldReturnCorrectFaculty()
    {
        // Act
        var result = _service.GetFacultyWithHighestAverageGrade();

        // Assert
        // Средние по факультетам:
        // Математика: (4.5 + 5.0) / 2 = 4.75
        // Информатика: (3.25 + 4.5) / 2 = 3.875
        // Физика: 4.0
        Assert.Equal("Математика", result);
    }

    [Fact]
    public void GetStudentsWithMinAverageGrade_ShouldReturnStudentsAboveThreshold()
    {
        // Act
        var result = _service.GetStudentsWithMinAverageGrade(4.0).ToList();

        // Assert
        Assert.Equal(4, result.Count); // Алексей, Сергей, Ольга, Дмитрий
        Assert.Contains(result, s => s.Name == "Алексей");
        Assert.Contains(result, s => s.Name == "Сергей");
        Assert.Contains(result, s => s.Name == "Ольга");
        Assert.Contains(result, s => s.Name == "Дмитрий");
        Assert.DoesNotContain(result, s => s.Name == "Екатерина");
    }

    [Fact]
    public void GetStudentsOrderedByName_ShouldSortByNameAlphabetically()
    {
        // Act
        var result = _service.GetStudentsOrderedByName().ToList();

        // Assert (по русскому алфавиту: А, Д, Е, О, С)
        Assert.Equal(5, result.Count);
        Assert.Equal("Алексей",   result[0].Name);
        Assert.Equal("Дмитрий",   result[1].Name);
        Assert.Equal("Екатерина", result[2].Name);
        Assert.Equal("Ольга",     result[3].Name);
        Assert.Equal("Сергей",    result[4].Name);
    }

    [Fact]
    public void GroupStudentsByFaculty_ShouldGroupCorrectly()
    {
        // Act
        var result = _service.GroupStudentsByFaculty();

        // Assert
        Assert.Equal(3, result.Count);                       // три факультета
        Assert.Equal(2, result["Математика"].Count());      // два студента
        Assert.Equal(2, result["Информатика"].Count());     // два студента
        Assert.Single(result["Физика"]);                    // один студент

        // Дополнительная проверка состава группы "Математика"
        var mathGroup = result["Математика"].ToList();
        Assert.Contains(mathGroup, s => s.Name == "Алексей");
        Assert.Contains(mathGroup, s => s.Name == "Сергей");
    }
}
