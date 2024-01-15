using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.SchoolRepo;
using Apprenticeship.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Apprenticeship.Controllers
{
    public class SchoolController : Controller
    {
        
        ISchoolRepository schoolRepo;
        public SchoolController(ISchoolRepository schoolRepo) {
            this.schoolRepo = schoolRepo;
        }
        //Read From Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewBag.schools = schoolRepo.GetAllSchools();
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Add() {
            return View();
        }
        //Create School Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Insert(SchoolDTO school)
		{
            if (ModelState.IsValid)
            {
                var school_ = new School();
			    school_.schoolName = school.schoolName;
			    school_.schoolAddress = school.schoolAddress;
			    school_.shortName = school.shortName;
			    schoolRepo.AddSchool(school_);
			    return RedirectToAction("Index");
            }
            return View("Add",school);
			
		}
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(int schoolId)
		{
			var school_ = schoolRepo.GetSchool(schoolId);
            var school = new SchoolDTO();
            school.schoolId = school_.schoolId;
            school.schoolName = school_.schoolName;
            school.schoolAddress = school_.schoolAddress;
            school.shortName = school_.shortName;
            return View(school);
		}
        //Edit School Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edited(SchoolDTO school)
		{
            if (ModelState.IsValid)
            {
                var school_ = new School();
                school_.schoolId = school.schoolId;
                school_.schoolName = school.schoolName;
                school_.schoolAddress = school.schoolAddress;
                school_.shortName = school.shortName;
                schoolRepo.EditSchool(school_);
                return RedirectToAction("Index");
            }
            return View("Edit", school);
        }
        //Delete School Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int schoolId)
		{
			schoolRepo.DeleteSchool(schoolId);
			return RedirectToAction("Index");

		}
	}
}
