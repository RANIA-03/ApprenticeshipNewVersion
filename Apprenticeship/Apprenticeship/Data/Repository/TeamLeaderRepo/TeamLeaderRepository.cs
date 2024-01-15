using Apprenticeship.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Apprenticeship.Data.Repository.TeamLeaderRepo
{
	public class TeamLeaderRepository : ITeamLeaderRepository
	{
		ApplicationDbContext context;
        private readonly UserManager<Person> _userManager;
        public TeamLeaderRepository(ApplicationDbContext context,UserManager<Person> _userManager)
		{
			this.context = context;
			this._userManager = _userManager;
		}

		public List<TeamLeader> GetAllTeamLeaders()
		{
			return context.teamLeaders.Include(c => c.company).ToList();
		}

		public void DeleteTeamLeader(string Id)
		{
			var teamLeader = GetTeamLeader(Id);
			context.teamLeaders.Remove(teamLeader);
			context.SaveChanges();
		}

		public void EditTeamLeader(TeamLeader teamLeader)
		{
			var teamLeaderInfo = GetTeamLeader(teamLeader.Id);
			teamLeaderInfo.fristName = teamLeader.fristName;
			teamLeaderInfo.secondName = teamLeader.secondName;
			teamLeaderInfo.thirdName = teamLeader.thirdName;
			teamLeaderInfo.lastName = teamLeader.lastName;
			teamLeaderInfo.PhoneNumber = teamLeader.PhoneNumber;
			teamLeaderInfo.companyId = teamLeader.companyId;
            teamLeaderInfo.Email = teamLeader.Email;
            teamLeaderInfo.NormalizedEmail = teamLeader.Email.ToUpper();
            teamLeaderInfo.UserName = teamLeader.Email;
            teamLeaderInfo.NormalizedUserName = teamLeader.Email.ToUpper();
            context.SaveChanges();
		}

		public TeamLeader GetTeamLeader(string Id)
		{
			var teamLeaderInfo = context.teamLeaders.Where(tl => tl.Id == Id).SingleOrDefault();
			return teamLeaderInfo;
		}

		public async Task AddTeamLeader(TeamLeader teamLeader, string password)
		{
            teamLeader.EmailConfirmed = true;
            await _userManager.CreateAsync(teamLeader, password);
            await _userManager.AddToRoleAsync(teamLeader, "TEAMLEADER");
        }
        private Person CreateUser()
        {
            try
            {
                return Activator.CreateInstance<TeamLeader>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(TeamLeader)}'. " +
                    $"Ensure that '{nameof(TeamLeader)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
