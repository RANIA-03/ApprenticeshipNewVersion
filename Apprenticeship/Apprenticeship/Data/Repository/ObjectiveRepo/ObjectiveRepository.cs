using Apprenticeship.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apprenticeship.Data.Repository.ObjectiveRepo
{
    public class ObjectiveRepository : IObjectiveRepository
    {
        ApplicationDbContext context;
        public ObjectiveRepository(ApplicationDbContext context) {
            this.context = context;
        }
        public void AddObjective(Objective objective)
        {
            context.objectives.Add(objective);
            context.SaveChanges();
        }

        public void DeleteObjective(int objectiveId)
        {
			var objective = GetObjective(objectiveId);
			context.objectives.Remove(objective);
			context.SaveChanges();
		}

        public void EditObjective(Objective objective)
        {
			var objectiveInfo = GetObjective(objective.objectiveId);
			objectiveInfo.objectiveName = objective.objectiveName;
			context.SaveChanges();
		}

        public List<Objective> GetAllObjectives()
        {
            return context.objectives.ToList();
        }

        public Objective GetObjective(int objectiveId)
        {
			var objectiveInfo = context.objectives.Where(o => o.objectiveId == objectiveId).SingleOrDefault();
			return objectiveInfo;
		}
    }
}
