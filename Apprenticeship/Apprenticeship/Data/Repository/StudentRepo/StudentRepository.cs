using Apprenticeship.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Apprenticeship.Data.Repository.StudentRepo
{
    public class StudentRepository : IStudentRepository
    {
        ApplicationDbContext context;
        private readonly UserManager<Person> _userManager;
        public StudentRepository(ApplicationDbContext context, UserManager<Person> _userManager)
        {
            this.context = context;
            this._userManager = _userManager;
        }

        public List<Student> GetAllStudents()
        {
            return context.students.Include(s => s.school).ToList();
		}

        public void DeleteStudent(string Id)
        {
			var student = GetStudent(Id);
			context.students.Remove(student);
            context.SaveChanges();
        }

        public void EditStudent(Student student)
        {
            var studentInfo = GetStudent(student.Id);
			studentInfo.fristName = student.fristName;
			studentInfo.secondName = student.secondName;
			studentInfo.thirdName = student.thirdName;
			studentInfo.lastName = student.lastName;
			studentInfo.PhoneNumber = student.PhoneNumber;
			//studentInfo.Email = student.Email;
			//studentInfo.NormalizedEmail = student.Email.ToUpper();
			//studentInfo.UserName = student.Email;
			//studentInfo.NormalizedUserName = student.Email.ToUpper();
			studentInfo.studentMajor = student.studentMajor;
			studentInfo.schoolId = student.schoolId;
            context.SaveChanges();
        }
       
        public Student GetStudent(string Id)
        {
            return context.students.Where(s => s.Id == Id).SingleOrDefault();
        }

        public async Task AddStudent(Student student,string password)
        {
            student.EmailConfirmed = true;
            await _userManager.CreateAsync(student, password);
            await _userManager.AddToRoleAsync(student, "STUDENT");
        }
        private Person CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Student>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Student)}'. " +
                    $"Ensure that '{nameof(Student)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
