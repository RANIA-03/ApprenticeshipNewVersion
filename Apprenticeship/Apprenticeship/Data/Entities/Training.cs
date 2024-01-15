using System;

namespace Apprenticeship.Data.Entities
{
    public class Training
    {
        public int trainingId { get; set; }
        public Student student { get; set; }
        public string studentId { get; set; }

        public TeamLeader teamLeader { get; set; }
        public string teamLeaderId { get; set; }
        public SchoolSupervisor schoolSupervisor { get; set; }
        public string schoolSupervisorId { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public List<TrainingObjectives> trainingObjectives { get; set; }
    }
}
