using Apprenticeship.Data.Entities;
using Apprenticeship.Data.Repository.CompanyRepo;
using Apprenticeship.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Apprenticeship.Controllers
{
    public class CompanyController : Controller
    {
        ICompanyRepository companyRepo;
        public CompanyController(ICompanyRepository companyRepo) {
            this.companyRepo = companyRepo;
        }
        //Read From Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Index()
        {
            ViewBag.companies = companyRepo.GetAllCompanies();
            return View();
        }
        [Authorize(Roles = "ADMIN")]
        public IActionResult Add() {
            return View();
        }
        //Create Company Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Insert(CompanyDTO company)
		{
            if (ModelState.IsValid)
            {
                var company_ = new Company();
                company_.companyName = company.companyName;
                company_.companyAddress = company.companyAddress;
                companyRepo.AddCompany(company_);
                return RedirectToAction("Index");
            }
            return View("Add",company);
		}
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(int companyId)
		{
			var company_ = companyRepo.GetCompany(companyId);
            var company = new CompanyDTO();
            company.companyId = company_.companyId;
            company.companyName = company_.companyName;
            company.companyAddress = company_.companyAddress;
            return View(company);
		}
        //Edit Company Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Edited(CompanyDTO company)
		{
            if (ModelState.IsValid)
            {
                var company_ = new Company();
                company_.companyId = company.companyId;
                company_.companyName = company.companyName;
                company_.companyAddress = company.companyAddress;
                companyRepo.EditCompany(company_);
                return RedirectToAction("Index");
            }
            return View("Edit",company);
		}
        //Delete Company Row in Database
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int companyId)
		{
			companyRepo.DeleteCompany(companyId);
			return RedirectToAction("Index");

		}
	}
}
