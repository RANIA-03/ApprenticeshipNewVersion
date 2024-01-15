using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.SchoolRepo
{
    public interface ISchoolRepository
    {
        public List<School> GetAllSchools();
        public void DeleteSchool(int schoolId);
        public School GetSchool(int schoolId);
        public void EditSchool(School school);
        public void AddSchool(School school);
    }
}
