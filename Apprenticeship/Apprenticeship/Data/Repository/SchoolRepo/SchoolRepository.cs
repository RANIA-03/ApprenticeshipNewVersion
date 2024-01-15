using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.SchoolRepo
{
    public class SchoolRepository : ISchoolRepository
    {
        ApplicationDbContext context;
		public SchoolRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<School> GetAllSchools()
        {
            return context.schools.ToList();
        }

        public void DeleteSchool(int schoolId)
        {
			var school = GetSchool(schoolId);
			context.schools.Remove(school);
            context.SaveChanges();
        }

        public void EditSchool(School school)
        {
            var schoolInfo = GetSchool(school.schoolId);
            schoolInfo.schoolName = school.schoolName;
            schoolInfo.schoolAddress = school.schoolAddress;
			schoolInfo.shortName = school.shortName;
			context.SaveChanges();
        }
       
        public School GetSchool(int schoolId)
        {
            var schoolInfo = context.schools.Where(s => s.schoolId == schoolId).SingleOrDefault();
            return schoolInfo;
        }

        public void AddSchool(School school)
        {
            context.schools.Add(school);
            context.SaveChanges();
        }

    }
}
