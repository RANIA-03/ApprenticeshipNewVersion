namespace Apprenticeship.Data.Entities
{
    public class Student : Person
    {
        public string studentMajor { get; set; }
        public School school { get; set; }
        public int schoolId { get; set; }
        public List<Training> trainings { get; set; }
    }
}
