using Apprenticeship.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Apprenticeship.Data.Repository.SchoolSupervisorRepo
{
	public class SchoolSupervisorRepository : ISchoolSupervisorRepository
	{
        ApplicationDbContext context;
        private readonly UserManager<Person> _userManager;
        public SchoolSupervisorRepository(ApplicationDbContext context, UserManager<Person> _userManager)
        {
            this.context = context;
            this._userManager = _userManager;
        }

        public List<SchoolSupervisor> GetAllSchoolSupervisors()
		{
			return context.schoolSupervisors.Include(s => s.school).ToList();
		}

		public void DeleteSchoolSupervisor(string Id)
		{
			var schoolSupervisor = GetSchoolSupervisor(Id);
			context.schoolSupervisors.Remove(schoolSupervisor);
			context.SaveChanges();
		}

		public void EditSchoolSupervisor(SchoolSupervisor schoolSupervisor)
		{
			var schoolSupervisorInfo = GetSchoolSupervisor(schoolSupervisor.Id);
			schoolSupervisorInfo.fristName = schoolSupervisor.fristName;
			schoolSupervisorInfo.secondName = schoolSupervisor.secondName;
			schoolSupervisorInfo.thirdName = schoolSupervisor.thirdName;
			schoolSupervisorInfo.lastName = schoolSupervisor.lastName;
			schoolSupervisorInfo.PhoneNumber = schoolSupervisor.PhoneNumber;
			schoolSupervisorInfo.Email = schoolSupervisor.Email;
			schoolSupervisorInfo.NormalizedEmail = schoolSupervisor.Email.ToUpper();
			schoolSupervisorInfo.UserName = schoolSupervisor.Email;
			schoolSupervisorInfo.NormalizedUserName = schoolSupervisor.Email.ToUpper();
			schoolSupervisorInfo.schoolId = schoolSupervisor.schoolId;
			context.SaveChanges();
		}

		public SchoolSupervisor GetSchoolSupervisor(string Id)
		{
			var schoolSupervisorInfo = context.schoolSupervisors.Where(s => s.Id == Id).SingleOrDefault();
			return schoolSupervisorInfo;
		}

		public async Task AddSchoolSupervisor(SchoolSupervisor schoolSupervisor,string password)
		{
			schoolSupervisor.EmailConfirmed = true;
            await _userManager.CreateAsync(schoolSupervisor, password);
            await _userManager.AddToRoleAsync(schoolSupervisor, "SCHOOLSUPERVISOR");
        }
        private Person CreateUser()
        {
            try
            {
                return Activator.CreateInstance<SchoolSupervisor>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(SchoolSupervisor)}'. " +
                    $"Ensure that '{nameof(SchoolSupervisor)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

    }
}
