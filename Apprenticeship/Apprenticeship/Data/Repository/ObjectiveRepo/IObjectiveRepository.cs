using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.ObjectiveRepo
{
    public interface IObjectiveRepository
    {
        public List<Objective> GetAllObjectives();
        public void DeleteObjective(int objectiveId);
        public Objective GetObjective(int objectiveId);
        public void EditObjective(Objective objective);
        public void AddObjective(Objective objective);
    }
}
