using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.SchoolSupervisorRepo
{
	public interface ISchoolSupervisorRepository
	{
		public List<SchoolSupervisor> GetAllSchoolSupervisors();
		public void DeleteSchoolSupervisor(string Id);
		public SchoolSupervisor GetSchoolSupervisor(string Id);
		public void EditSchoolSupervisor(SchoolSupervisor schoolSupervisor);
		public Task AddSchoolSupervisor(SchoolSupervisor schoolSupervisor, string password);
	}
}
