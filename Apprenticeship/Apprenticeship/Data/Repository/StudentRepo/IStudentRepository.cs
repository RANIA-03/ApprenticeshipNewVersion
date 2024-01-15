using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.StudentRepo
{
    public interface IStudentRepository
    {
        public List<Student> GetAllStudents();
        public void DeleteStudent(string Id);
        public Student GetStudent(string Id);
        public void EditStudent(Student student);
        public Task AddStudent(Student student, string password);
    }
}
