using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.TrainingObjectivesRepo;
using Microsoft.EntityFrameworkCore;
namespace Apprenticeship.Data.Repository.TrainingRepo
{
    public class TrainingRepository : ITrainingRepository
    {
        ApplicationDbContext context;
        ITrainingObjectivesRepository trainingObjectivesRepo;
        public TrainingRepository(ApplicationDbContext context, ITrainingObjectivesRepository trainingObjectivesRepo)
        {
            this.context = context;
            this.trainingObjectivesRepo = trainingObjectivesRepo;
        }
        public void AddTraining(Training training, List<int> objectiveIds)
        {
            context.trainings.Add(training);
            context.SaveChanges();
            var lastTraining = context.trainings.OrderByDescending(t => t.trainingId).FirstOrDefault();

            foreach (var objectiveId in objectiveIds)
            {
                var to = new TrainingObjectives();
                to.objectiveId=objectiveId;
                to.trainingId = lastTraining.trainingId;
                trainingObjectivesRepo.AddTrainingObjective(to);
            }

        }

        public void DeleteTraining(int trainingId)
        {
			var training = GetTraining(trainingId);
			context.trainings.Remove(training);
			context.SaveChanges();
		}

        public void EditTraining(Training training)
        {
			var trainingInfo = GetTraining(training.trainingId);
			trainingInfo.studentId = training.studentId;
			trainingInfo.schoolSupervisorId = training.schoolSupervisorId;
			trainingInfo.teamLeaderId = training.teamLeaderId;
			trainingInfo.startDate = training.startDate;
			trainingInfo.endDate = training.endDate;
			context.SaveChanges();
		}

        public List<Training> GetAllTrainings()
        {
			return context.trainings
			 .Include(t => t.student)
				.Include(t => t.schoolSupervisor)
				.Include(t => t.teamLeader)
				.Include(t => t.trainingObjectives)
				.ThenInclude(to => to.objective).ToList();

		}


		public Training GetTraining(int trainingId)
		{
			
			return context.trainings
				.Where(t => t.trainingId == trainingId)
				.Include(t => t.student)
				.Include(t => t.schoolSupervisor)
				.Include(t => t.teamLeader)
				.Include(t => t.trainingObjectives)
					.ThenInclude(to => to.objective) 
				.SingleOrDefault();
		}

	}
}
