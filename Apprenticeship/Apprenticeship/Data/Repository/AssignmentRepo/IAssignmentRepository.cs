using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.AssignmentRepo
{
    public interface IAssignmentRepository
    {
        public List<Assignment> GetAllAssignments();
        public void DeleteAssignment(int assignmentId, int trainingId);
        public Assignment GetAssignment(int assignmentId, int trainingId);
        public void EditAssignment(Assignment assignment);
        public void AddAssignment(Assignment assignment, List<int> objectiveIds);
    }
}
