using CampusLibrary.Core.Data;
using CampusLibrary.Core.Models;
using Dapper;

namespace CampusLibrary.Core.Repositories;

public class StudentRepository
{
    private readonly DbConnectionFactory _factory;

    public StudentRepository(DbConnectionFactory factory)
    {
        _factory = factory;
    }

    public List<Student> GetAll()
    {
        using var conn = _factory.CreateOpenConnection();
        const string sql = @"
SELECT student_id AS StudentId, name AS Name, class_name AS ClassName,
       contact AS Contact, total_borrow_count AS TotalBorrowCount
FROM student
ORDER BY student_id";
        return conn.Query<Student>(sql).ToList();
    }

    public Student? GetById(string studentId)
    {
        using var conn = _factory.CreateOpenConnection();
        const string sql = @"
SELECT student_id AS StudentId, name AS Name, class_name AS ClassName,
       contact AS Contact, total_borrow_count AS TotalBorrowCount
FROM student
WHERE student_id = @studentId";
        return conn.QueryFirstOrDefault<Student>(sql, new { studentId });
    }

    public void Add(Student student)
    {
        using var conn = _factory.CreateOpenConnection();
        const string sql = @"
INSERT INTO student(student_id, name, class_name, contact, total_borrow_count)
VALUES(@StudentId, @Name, @ClassName, @Contact, @TotalBorrowCount)";
        conn.Execute(sql, student);
    }

    public void Update(Student student)
    {
        using var conn = _factory.CreateOpenConnection();
        const string sql = @"
UPDATE student SET name=@Name, class_name=@ClassName, contact=@Contact
WHERE student_id=@StudentId";
        conn.Execute(sql, student);
    }

    public void Delete(string studentId)
    {
        using var conn = _factory.CreateOpenConnection();
        conn.Execute("DELETE FROM student WHERE student_id=@studentId", new { studentId });
    }
}

