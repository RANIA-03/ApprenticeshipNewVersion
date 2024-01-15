namespace Apprenticeship.Data.Entities
{
    public class TrainingObjectives
    {
        public Training training { get; set; }
        public int trainingId { get; set; }
        public Objective objective { get; set; }
        public int objectiveId { get; set; }
    }
}
