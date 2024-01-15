using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.CompanyRepo
{
    public class CompanyRepository : ICompanyRepository
    {
        ApplicationDbContext context;
        public CompanyRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<Company> GetAllCompanies()
        {
            return context.companies.ToList();
        }

        public void DeleteCompany(int companyId)
        {
			var company = GetCompany(companyId);
			context.companies.Remove(company);
            context.SaveChanges();
        }

        public void EditCompany(Company company)
        {
            var CompanyInfo = GetCompany(company.companyId);
            CompanyInfo.companyName = company.companyName;
            CompanyInfo.companyAddress = company.companyAddress;
            context.SaveChanges();
        }
       
        public Company GetCompany(int companyId)
        {
            var companyInfo = context.companies.Where(c => c.companyId == companyId).SingleOrDefault();
            return companyInfo;
        }

        public void AddCompany(Company company)
        {
            context.companies.Add(company);
            context.SaveChanges();
        }

    }
}
