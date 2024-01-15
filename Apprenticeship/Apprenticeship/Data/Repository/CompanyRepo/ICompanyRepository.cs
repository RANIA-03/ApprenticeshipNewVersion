using Apprenticeship.Data.Entities;

namespace Apprenticeship.Data.Repository.CompanyRepo
{
    public interface ICompanyRepository
    {
        public List<Company> GetAllCompanies();
        public void DeleteCompany(int companyId);
        public Company GetCompany(int companyId);
        public void EditCompany(Company company);
        public void AddCompany(Company company);
    }
}
