namespace Apprenticeship.Data.Entities
{
    public class SchoolSupervisor : Person
    {
        public School school { get; set; }
        public int schoolId { get; set; }
        public List<Training> trainings { get; set; }

    }
}
