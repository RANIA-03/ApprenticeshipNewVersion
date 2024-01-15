using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.TeamLeaderRepo
{
	public interface ITeamLeaderRepository
	{
		public List<TeamLeader> GetAllTeamLeaders();
		public void DeleteTeamLeader(string Id);
		public TeamLeader GetTeamLeader(string Id);
		public void EditTeamLeader(TeamLeader teamLeader);
		public Task AddTeamLeader(TeamLeader teamLeader,string password);
	}
}
