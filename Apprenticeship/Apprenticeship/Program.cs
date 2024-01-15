using Apprenticeship.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.SchoolRepo;
using Apprenticeship.Data.Repository.CompanyRepo;
using Apprenticeship.Data.Repository.SchoolSupervisorRepo;
using Apprenticeship.Data.Repository.StudentRepo;
using Apprenticeship.Data.Repository.TeamLeaderRepo;
using Apprenticeship.Data.Repository.TrainingRepo;
using Apprenticeship.Data.Repository.ObjectiveRepo;
using Apprenticeship.Data.Repository.TrainingObjectivesRepo;
using Apprenticeship.Data.Repository.AssignmentRepo;
using Apprenticeship.Data.Repository.ReportRepo;
using Apprenticeship.Data.Repository.AssignmentObjectivesRepo;
using Apprenticeship.Data.Repository.ReportStatusRepo;
using Apprenticeship.Data.Repository.ReportsLogRepo;
using Apprenticeship.Data.Repository.DocumentRepo;

namespace Apprenticeship
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<Person>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
			builder.Services.AddControllersWithViews();
			builder.Services.AddTransient<ISchoolRepository, SchoolRepository>();
			builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
			builder.Services.AddTransient<IStudentRepository, StudentRepository>();
			builder.Services.AddTransient<UserManager<Person>>();
			builder.Services.AddTransient<ISchoolSupervisorRepository, SchoolSupervisorRepository>();
			builder.Services.AddTransient<ITeamLeaderRepository, TeamLeaderRepository>();
			builder.Services.AddTransient<ITrainingRepository, TrainingRepository>();
			builder.Services.AddTransient<IObjectiveRepository, ObjectiveRepository>();
			builder.Services.AddTransient<ITrainingObjectivesRepository, TrainingObjectivesRepository>();
			builder.Services.AddTransient<IAssignmentRepository, AssignmentRepository>();
			builder.Services.AddTransient<IReportRepository, ReportRepository>();
			builder.Services.AddTransient<IReportStatusRepository, ReportStatusRepository>();
			builder.Services.AddTransient<IReportsLogRepository, ReportsLogRepository>();
			builder.Services.AddTransient<IAssignmentObjectivesRepository, AssignmentObjectivesRepository>();
			builder.Services.AddTransient<IDocumentRepository, DocumentRepository>();
			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			app.Run();
		}
	}
}