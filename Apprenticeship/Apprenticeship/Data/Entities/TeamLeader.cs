namespace Apprenticeship.Data.Entities
{
    public class TeamLeader : Person
    {
        public Company company { get; set; }
        public int companyId { get; set; }
        public List<Training> trainings { get; set; }

    }
}
