using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.SchoolSupervisorRepo;
using Apprenticeship.Data.Repository.StudentRepo;
using Apprenticeship.Data.Repository.TeamLeaderRepo;
using Apprenticeship.Data.Repository.ObjectiveRepo;
using Apprenticeship.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Apprenticeship.Controllers
{
    public class ObjectiveController : Controller
    {
        IObjectiveRepository objectiveRepo;
        public ObjectiveController(IObjectiveRepository objectiveRepo)
        {
            this.objectiveRepo = objectiveRepo;
        }
        //Read From Database
        [Authorize(Roles = "ADMIN, TEAMLEADER, STUDENT")]

        public IActionResult Index()
        {
            ViewBag.objectives = objectiveRepo.GetAllObjectives();
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Add() {
            return View();
        } 

        //Create Objective Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Insert(ObjectiveDTO objective)
		{
            var objective_ = new Objective();
            objective_.objectiveName = objective.objectiveName;
            objectiveRepo.AddObjective(objective_);
            return RedirectToAction("Index");
		}
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(int objectiveId)
		{
            var objective_ = objectiveRepo.GetObjective(objectiveId);
			return View(objective_);
		}
        //Edit Objective Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edited(ObjectiveDTO objective)
		{
			var objective_ = new Objective();
			objective_.objectiveId = objective.objectiveId;
			objective_.objectiveName = objective.objectiveName;


			objectiveRepo.EditObjective(objective_);
			return RedirectToAction("Index");
		}
       
        //Delete Objective Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int objectiveId)
		{
			objectiveRepo.DeleteObjective(objectiveId);
			return RedirectToAction("Index");
		}
	}
}
