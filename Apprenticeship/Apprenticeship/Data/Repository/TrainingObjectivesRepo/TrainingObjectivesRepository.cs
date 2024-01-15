using Apprenticeship.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apprenticeship.Data.Repository.TrainingObjectivesRepo
{
    public class TrainingObjectivesRepository : ITrainingObjectivesRepository
    {
        ApplicationDbContext context;
        public TrainingObjectivesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddTrainingObjective(TrainingObjectives trainingObjective)
        {
            context.trainingObjectives.Add(trainingObjective);
            context.SaveChanges();
        }

        public void DeleteTrainingObjective(int objectiveId, int trainingId)
        {
            var trainingObjective = GetTrainingObjective(objectiveId, trainingId);
            context.trainingObjectives.Remove(trainingObjective);
            context.SaveChanges();
        }

        public void EditTrainingObjective(TrainingObjectives trainingObjectives)
        {
            var trainingObjective_ = GetTrainingObjective(trainingObjectives.objectiveId,trainingObjectives.trainingId);
            trainingObjective_.objectiveId = trainingObjectives.objectiveId;
            trainingObjective_.trainingId = trainingObjectives.trainingId;
            context.SaveChanges();
        }

        public List<TrainingObjectives> GetAllTrainingObjectives()
        {
            return context.trainingObjectives.Include(tc => tc.objective).Include(tc=>tc.training).ThenInclude(s => s.student).ToList();
        }

        public TrainingObjectives GetTrainingObjective(int objectiveId, int trainingId)
        {
            var trainingObjective_ = context.trainingObjectives.Include(t => t.objective).Include(t => t.training).ThenInclude(s => s.student).Where(tc => tc.objectiveId == objectiveId && tc.trainingId == trainingId).SingleOrDefault();
            return trainingObjective_;
        }
        public void RemoveTrainingObjectivesByTrainingId(int trainingId)
        {
            var objectivesToRemove = context.trainingObjectives.Where(to => to.trainingId == trainingId);
            foreach (var objective in objectivesToRemove)
            {
                context.trainingObjectives.Remove(objective);
            }
            context.SaveChanges();
        }
    }
}
