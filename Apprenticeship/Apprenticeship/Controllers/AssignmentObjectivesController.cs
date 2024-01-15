using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.AssignmentRepo;
using Apprenticeship.Data.Repository.ObjectiveRepo;
using Apprenticeship.Data.Repository.AssignmentObjectivesRepo;
using Apprenticeship.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Apprenticeship.Controllers
{
    public class AssignmentObjectivesController : Controller
    {
        IAssignmentObjectivesRepository assignmentObjectivesRepo;
        IAssignmentRepository assignmentRepo;
        IObjectiveRepository objectivesRepo;
        public AssignmentObjectivesController(IAssignmentObjectivesRepository assignmentObjectivesRepo, IAssignmentRepository assignmentRepo, IObjectiveRepository objectivesRepo)
        {
            this.assignmentObjectivesRepo = assignmentObjectivesRepo;
            this.assignmentRepo = assignmentRepo;
            this.objectivesRepo = objectivesRepo;
        }
        //Read From Database
        [Authorize(Roles = "TEAMLEADER, STUDENT")]
        public IActionResult Index()
        {
            ViewBag.assignmentObjectives = assignmentObjectivesRepo.GetAllAssignmentObjectives();
            return View();
        }
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Add()
        {
            ViewBag.trainings = assignmentRepo.GetAllAssignments();
            ViewBag.objectives = objectivesRepo.GetAllObjectives();

            return View();
        }
        //Create assignmentObjectives Row in Database
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Insert(AssignmentObjectiveDTO assignmentObjective)
        {
            var assignmentObjective_ = new AssignmentObjectives();
            assignmentObjective_.objectiveId = assignmentObjective.objectiveId;
            assignmentObjective_.assignmentId = assignmentObjective.assignmentId;
            assignmentObjectivesRepo.AddAssignmentObjective(assignmentObjective_);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Edit(AssignmentObjectiveDTO assignmentObjective)
        {
            var assignmentObjective_ = assignmentObjectivesRepo.GetAssignmentObjective(assignmentObjective.objectiveId, assignmentObjective.assignmentId);
            return View(assignmentObjective_);
        }
        //Edit assignmentObjectives Row in Database
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Edited(ObjectiveDTO objective)
        {
            var assignmentObjective_ = new AssignmentObjectives();
            assignmentObjectivesRepo.EditAssignmentObjective(assignmentObjective_);
            return RedirectToAction("Index");
        }
        //Delete assignmentObjectives Row in Database

        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Delete(AssignmentObjectiveDTO assignmentObjective)
        {
            assignmentObjectivesRepo.DeleteAssignmentObjective(assignmentObjective.objectiveId, assignmentObjective.assignmentId);
            return RedirectToAction("Index");
        }
    }
}
