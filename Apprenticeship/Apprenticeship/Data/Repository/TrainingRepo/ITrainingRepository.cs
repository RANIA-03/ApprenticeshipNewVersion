using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.TrainingRepo
{
    public interface ITrainingRepository
    {
        public List<Training> GetAllTrainings();
        public void DeleteTraining(int trainingId);
        public Training GetTraining(int trainingId);
        public void EditTraining(Training training);
        public void AddTraining(Training training,List<int> objectiveIds);
    }
}
