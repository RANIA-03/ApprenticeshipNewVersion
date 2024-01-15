using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.AssignmentObjectivesRepo;
using Apprenticeship.Data.Repository.AssignmentRepo;
using Apprenticeship.Data.Repository.DocumentRepo;
using Apprenticeship.Data.Repository.ObjectiveRepo;
using Apprenticeship.Data.Repository.StudentRepo;
using Apprenticeship.Data.Repository.TeamLeaderRepo;
using Apprenticeship.Data.Repository.TrainingObjectivesRepo;
using Apprenticeship.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Apprenticeship.Controllers
{
    public class AssignmentController : Controller
    {
        IAssignmentRepository  assignmentRepo;
        IDocumentRepository documentRepo;
        IObjectiveRepository objectiveRepo;
        IAssignmentObjectivesRepository assignmentObjectivesRepo;
        ITrainingObjectivesRepository trainingObjectivesRepo;
        IStudentRepository studentRepo;
        ITeamLeaderRepository teamLeaderRepo;
        public AssignmentController(IAssignmentRepository assignmentRepo, IDocumentRepository documentRepo, IObjectiveRepository objectiveRepo, IAssignmentObjectivesRepository assignmentObjectivesRepo, ITrainingObjectivesRepository trainingObjectivesRepo, ITeamLeaderRepository teamLeaderRepo)
        {
            this.assignmentRepo = assignmentRepo;
            this.documentRepo = documentRepo;   
            this.objectiveRepo = objectiveRepo;
            this.assignmentObjectivesRepo = assignmentObjectivesRepo;
            this.trainingObjectivesRepo = trainingObjectivesRepo;
            this.studentRepo = studentRepo;
            this.teamLeaderRepo = teamLeaderRepo;
        }
        //Read From Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index(int trainingId)
		{
			ViewBag.assignments = assignmentRepo.GetAllAssignments();
            return View(trainingId);
		}
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Add(int trainingId)
        {
			AssignmentDTO assignment=new AssignmentDTO();
            assignment.trainingId =trainingId;
            ViewBag.objectives = trainingObjectivesRepo.GetAllTrainingObjectives().Where(t=>t.trainingId == assignment.trainingId).ToList();
			return View(assignment);
        }
        //Create Assignment Row in Database
        [Authorize(Roles = "TEAMLEADER")]
        [HttpPost]
        public IActionResult Insert(AssignmentDTO assignment, List<IFormFile> formFile)
        {
            ModelState.Remove("assignmentId");
            if (ModelState.IsValid)
            {
                var assignment_ = new Assignment();
                assignment_.assignmentTitle = assignment.assignmentTitle;
                assignment_.assignmentDescription = assignment.assignmentDescription;
                assignment_.assignmentNotes = assignment.assignmentNotes;
                assignment_.startDate = assignment.startDate;
                assignment_.endDate = assignment.endDate;
                assignment_.trainingId = assignment.trainingId;
                assignment_.assignmentId = assignment.assignmentId;
                assignmentRepo.AddAssignment(assignment_, assignment.objectiveIds);
                var assignmentId = assignment_.assignmentId;
                //Adding document to DB
                foreach (var file in formFile)
                {
                    Document document = new Document();
                    if (file.Length > 0)
                    {
                        Stream st = file.OpenReadStream();
                        using (BinaryReader br = new BinaryReader(st))//start
                        {
                            var byteFile = br.ReadBytes((int)st.Length);
                            document.file = byteFile;
                        }//end
                        document.name = file.FileName;
                        document.contentType = file.ContentType;
                        document.assignmentId = assignmentId;
                        documentRepo.AddDocument(document);
                    }
                }
                return RedirectToAction("Assignments", "TeamLeader", new { trainingId = assignment.trainingId });
            }
            ViewBag.objectives = trainingObjectivesRepo.GetAllTrainingObjectives().Where(t => t.trainingId == assignment.trainingId).ToList();
            return View("Add",assignment);
        }
		[Authorize(Roles = "TEAMLEADER, STUDENT, SCHOOLSUPERVISOR")]
		public FileStreamResult GetFile(long documentId)
		{
            var file = documentRepo.GetDocument(documentId);
			Stream stream = new MemoryStream(file.file);
			return new FileStreamResult(stream, file.contentType);
		}
		[Authorize(Roles = "TEAMLEADER")]
        public IActionResult Edit(int assignmentId, int trainingId)
        {
            var assignment = assignmentRepo.GetAssignment(assignmentId, trainingId);
            AssignmentDTO assignment_ = new AssignmentDTO();
            assignment_.assignmentTitle = assignment.assignmentTitle;
            assignment_.assignmentDescription = assignment.assignmentDescription;
            assignment_.assignmentNotes = assignment.assignmentNotes;
            assignment_.startDate = assignment.startDate;
            assignment_.endDate = assignment.endDate;
            assignment_.trainingId = assignment.trainingId;
            assignment_.assignmentId = assignment.assignmentId;
            ViewBag.assignmentObjectivesSelected = assignmentObjectivesRepo.GetAllAssignmentObjectives().Where(t => t.assignmentId == assignmentId).ToList();
            ViewBag.assignmentObjectives = trainingObjectivesRepo.GetAllTrainingObjectives().Where(t => t.trainingId == trainingId).ToList();
            ViewBag.documents = documentRepo.GetAllDocuments().Where(a => a.assignmentId == assignmentId).ToList();
			return View(assignment_);
        }
        //Edit Assignment Row in Database
        [Authorize(Roles = "TEAMLEADER")]
        [HttpPost]
        public IActionResult Edited(AssignmentDTO assignment,List<IFormFile> formFile)
        {
            if (ModelState.IsValid)
            {
                var assignment_ = new Assignment();
                assignment_.assignmentId = assignment.assignmentId;
                assignment_.assignmentTitle = assignment.assignmentTitle;
                assignment_.assignmentDescription = assignment.assignmentDescription;
                assignment_.assignmentNotes = assignment.assignmentNotes;
                assignment_.startDate = assignment.startDate;
                assignment_.endDate = assignment.endDate;
                assignment_.trainingId = assignment.trainingId;
                assignmentRepo.EditAssignment(assignment_);
                assignmentObjectivesRepo.RemoveAssignmentObjectivesByAssignmentId(assignment.assignmentId);
                foreach (var o in assignment.objectiveIds)
                {
                    assignmentObjectivesRepo.AddAssignmentObjective(new AssignmentObjectives { assignmentId = assignment.assignmentId, objectiveId = o });
                }
                //Adding document to DB
                foreach (var file in formFile)
                {
                    Document document = new Document();
                    if (file.Length > 0)
                    {
                        Stream st = file.OpenReadStream();
                        using (BinaryReader br = new BinaryReader(st))//start
                        {
                            var byteFile = br.ReadBytes((int)st.Length);
                            document.file = byteFile;
                        }//end
                        document.name = file.FileName;
                        document.contentType = file.ContentType;
                        document.assignmentId = assignment_.assignmentId;
                        documentRepo.AddDocument(document);
                    }
                }
                return RedirectToAction("Assignments", "TeamLeader", new { trainingId = assignment.trainingId });
            }
            ViewBag.assignmentObjectivesSelected = assignmentObjectivesRepo.GetAllAssignmentObjectives().Where(t => t.assignmentId == assignment.assignmentId).ToList();
            ViewBag.assignmentObjectives = trainingObjectivesRepo.GetAllTrainingObjectives().Where(t => t.trainingId == assignment.trainingId).ToList();
            ViewBag.documents = documentRepo.GetAllDocuments().Where(a => a.assignmentId == assignment.assignmentId).ToList();
            return View("Edit", assignment);
        }
        //Delete Assignment Row in Database
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult Delete(AssignmentDTO assignment)
        {
            assignmentRepo.DeleteAssignment(assignment.assignmentId,assignment.trainingId);
			return RedirectToAction("Assignments", "TeamLeader", new { trainingId = assignment.trainingId });
		}
        [Authorize(Roles = "TEAMLEADER")]
        public IActionResult DeleteDocument(int assignmentId,int trainingId, int documentId)
        {
            documentRepo.DeleteDocument(documentId);
            return RedirectToAction("Edit", "Assignment", new {assignmentId,trainingId});
        }
    }
}
    