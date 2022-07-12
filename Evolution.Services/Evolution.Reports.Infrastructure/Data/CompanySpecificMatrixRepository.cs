using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Reports.Domain.Interfaces.Data;
using Evolution.Common.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Infrastructure.Data
{
    public class CompanySpecificMatrixRepository : ICompanySpecificMatrixRepository, IDisposable
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public CompanySpecificMatrixRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IList<DomainModel.ResourceTaxonomyServices> GetByResource(string[] companyID)
        {
            List<string> llstCompanyID = companyID.ToList();
            var data = _dbContext.TechnicalSpecialist.Join(_dbContext.Data, techSpec => new { col1 = techSpec.EmploymentTypeId.Value, col2 = Convert.ToInt32(MasterType.EmploymentType) }, MD => new { col1 = MD.Id, col2 = MD.MasterDataTypeId }, (techSpec, MD) => new { techSpec, MD })
                       .Join(_dbContext.Company, techSpecCompany => techSpecCompany.techSpec.CompanyId, company => company.Id, (techSpecCompany, company) => new { techSpecCompany, company })
                       .Where(x => llstCompanyID.Contains(x.company.Code) && x.techSpecCompany.techSpec.ProfileStatus.Name == ResourceSearchConstants.TS_Profile_Status_Active && x.techSpecCompany.techSpec.EmploymentType.Name != ResourceSearchConstants.Employment_Type_OfficeStaff) //Changes for IGO D910
                       .Select(y => new DomainModel.ResourceTaxonomyServices
                       {
                           Company = y.company.Name,
                           ChildArray = y.techSpecCompany.techSpec.TechnicalSpecialistTaxonomy.Select(z => new DomainModel.TaxonomyServices
                           {
                               TaxonomyServiceName = z.TaxonomyServices.TaxonomyServiceName,
                               TaxonomyServicesId = z.TaxonomyServicesId,
                               IsTaxonomyApplicable = true
                           }).OrderBy(r => r.TaxonomyServiceName).ToList(),
                           ResourceId = y.techSpecCompany.techSpec.Id,
                           ResourceName = y.techSpecCompany.techSpec.LastName + ", " + y.techSpecCompany.techSpec.FirstName + "(" + y.techSpecCompany.techSpec.Pin + ") - ",
                           IsEmployee = y.techSpecCompany.MD.Name == "Former" || y.techSpecCompany.MD.Name == "FT Employee" || y.techSpecCompany.MD.Name == "Office Staff" || y.techSpecCompany.MD.Name == "PT Employee" || y.techSpecCompany.MD.Name == "Temp Employee" ? true : false,
                           IsContractor = y.techSpecCompany.MD.Name == "Independent Contractor" || y.techSpecCompany.MD.Name == "Third Party Contractor" ? true : false
                       }).OrderBy(a => a.ResourceName).ToList();
            return data;
        }

        public IList<DomainModel.TaxonomyResourceServices> GetByTaxonomyService(string[] companyID)
        {
            List<string> companyIDs = companyID.ToList();
            List<DomainModel.TaxonomyResourceServices> companySpecifcMatrix = new List<DomainModel.TaxonomyResourceServices>();
            var taxonomyServiceList = _dbContext.TaxonomyService.Select(a=> new {a.TaxonomyServiceName }).Distinct().ToList();

            var data = _dbContext.TechnicalSpecialist.Join(_dbContext.Data, techSpec => new { col1 = techSpec.EmploymentTypeId.Value, col2 = Convert.ToInt32(MasterType.EmploymentType) }, MD => new { col1 = MD.Id, col2 = MD.MasterDataTypeId }, (techSpec, MD) => new { techSpec, MD })
                        .Join(_dbContext.TechnicalSpecialistTaxonomy, tsTaxonomy => tsTaxonomy.techSpec.Id, taxonomy => taxonomy.TechnicalSpecialistId, (tsTaxonomy, taxonomy) => new { tsTaxonomy, taxonomy })
                        .Join(_dbContext.Company, taxonomyCompany => taxonomyCompany.tsTaxonomy.techSpec.CompanyId, company => company.Id, (taxonomyCompany, company) => new { taxonomyCompany, company })
                        .Where(x => companyIDs.Contains(x.company.Code) && x.taxonomyCompany.tsTaxonomy.techSpec.ProfileStatus.Name == ResourceSearchConstants.TS_Profile_Status_Active && x.taxonomyCompany.tsTaxonomy.techSpec.EmploymentType.Name != ResourceSearchConstants.Employment_Type_OfficeStaff)
                        .Select(y => new DomainModel.ResourceTaxonomyServices
                        {
                            Company = y.company.Name,
                            TaxonomyServiceName = y.taxonomyCompany.taxonomy.TaxonomyServices.TaxonomyServiceName,
                            ResourceId = y.taxonomyCompany.tsTaxonomy.techSpec.Id,
                            ResourceName = y.taxonomyCompany.tsTaxonomy.techSpec.LastName + ", " + y.taxonomyCompany.tsTaxonomy.techSpec.FirstName + "(" + y.taxonomyCompany.tsTaxonomy.techSpec.Pin + ")",
                            IsEmployee = y.taxonomyCompany.tsTaxonomy.MD.Name == "Former" || y.taxonomyCompany.tsTaxonomy.MD.Name == "FT Employee" || y.taxonomyCompany.tsTaxonomy.MD.Name == "Office Staff" || y.taxonomyCompany.tsTaxonomy.MD.Name == "PT Employee" || y.taxonomyCompany.tsTaxonomy.MD.Name == "Temp Employee" ? true : false,
                            IsContractor = y.taxonomyCompany.tsTaxonomy.MD.Name == "Independent Contractor" || y.taxonomyCompany.tsTaxonomy.MD.Name == "Third Party Contractor" ? true : false,
                            LastName = y.taxonomyCompany.tsTaxonomy.techSpec.LastName
                        }).ToList();

            foreach (var item in taxonomyServiceList)
            {
                DomainModel.TaxonomyResourceServices lobjTaxonomyResourceServices = new DomainModel.TaxonomyResourceServices();
                lobjTaxonomyResourceServices.TaxonomyServiceName = item.TaxonomyServiceName;
                var resourceData = data.Where(a => a.TaxonomyServiceName == item.TaxonomyServiceName)?.OrderBy(x => x.ResourceName)?.ToList();
                foreach (var items in resourceData)
                {
                    lobjTaxonomyResourceServices.IsTaxonomyApplicable = true;
                    if (!string.IsNullOrEmpty(items.ResourceName))
                    {
                        lobjTaxonomyResourceServices.ChildArray.Add(new DomainModel.ResourceDetails()
                        {
                            Company = items.Company,
                            IsContractor = items.IsContractor,
                            IsEmployee = items.IsEmployee,
                            ResourceId = items.ResourceId,
                            ResourceName = items.ResourceName,
                            LastName = items.LastName
                        });
                    }
                }
                var childData = lobjTaxonomyResourceServices.ChildArray.OrderBy(a => a.Company).ThenBy(a => a.LastName).ToList();
                lobjTaxonomyResourceServices.ChildArray = childData;
                companySpecifcMatrix.Add(lobjTaxonomyResourceServices);
            }
            return companySpecifcMatrix?.OrderBy(x => x.TaxonomyServiceName).ToList();
        }

        public byte[] ExportReport(string[] companyCode)
        {
            byte[] bytes = null;
            List<string> companyCodes = companyCode.ToList();
            List<DbModel.TaxonomyService> taxonomyServices = _dbContext.TaxonomyService.OrderBy(a => a.TaxonomyServiceName).ToList();
            List<string> columns = new List<string>() { "Resource", "Employee", "Contractor", "Company" };
            columns.AddRange(taxonomyServices.Select(a => string.Format("\"{0}\"", a.TaxonomyServiceName)).Distinct().ToList());
            List<string> emptyRow = new List<string>(columns.Count);
            List<string> totalRow = new List<string>(columns.Count);
            columns.ForEach(a => { emptyRow.Add(""); totalRow.Add(""); });
            totalRow[0] = "Total";
            List<List<string>> finalData = new List<List<string>>();
            var data = _dbContext.TechnicalSpecialist.Join(_dbContext.Data, techSpec => new { col1 = techSpec.EmploymentTypeId.Value, col2 = Convert.ToInt32(MasterType.EmploymentType) }, MD => new { col1 = MD.Id, col2 = MD.MasterDataTypeId }, (techSpec, MD) => new { techSpec, MD })
                       .Join(_dbContext.Company, techSpecCompany => techSpecCompany.techSpec.CompanyId, company => company.Id, (techSpecCompany, company) => new { techSpecCompany, company })
                       .Where(x => companyCodes.Contains(x.company.Code) && x.techSpecCompany.techSpec.ProfileStatus.Name == ResourceSearchConstants.TS_Profile_Status_Active && x.techSpecCompany.techSpec.EmploymentType.Name != ResourceSearchConstants.Employment_Type_OfficeStaff)
                       .Select(y => new
                       {
                           Company = y.company.Name,
                           y.techSpecCompany.techSpec.TechnicalSpecialistTaxonomy,
                           ResourceId = y.techSpecCompany.techSpec.Id,
                           ResourceName = y.techSpecCompany.techSpec.LastName.Replace("\"", "").Replace(",", "") + ", " + y.techSpecCompany.techSpec.FirstName.Replace("\"", "").Replace(",", "") + "(" + y.techSpecCompany.techSpec.Pin + ")",
                           Employee = y.techSpecCompany.MD.Name == "Former" || y.techSpecCompany.MD.Name == "FT Employee" || y.techSpecCompany.MD.Name == "Office Staff" || y.techSpecCompany.MD.Name == "PT Employee" || y.techSpecCompany.MD.Name == "Temp Employee" ? "X" : "",
                           Contractor = y.techSpecCompany.MD.Name == "Independent Contractor" || y.techSpecCompany.MD.Name == "Third Party Contractor" ? "X" : ""
                       }).OrderBy(a => a.ResourceName).ToList();

            if (data != null && data.Count > 0)
            {
                int lintEmployeeCount = data.Count(a => a.Employee == "X");
                int lintContractorCount = data.Count(a => a.Contractor == "X");
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Company Specific Matrix Report");
                string lstrHeader = string.Join(",", columns);
                csvContent.AppendLine(lstrHeader);
                foreach (var item in data)
                {
                    List<string> tempRow = new List<string>(columns.Count);
                    tempRow.AddRange(emptyRow);
                    tempRow[0] = string.Format("\"{0}\"", item.ResourceName);
                    tempRow[1] = item.Employee;
                    tempRow[2] = item.Contractor;
                    tempRow[3] = string.Format("\"{0}\"", item.Company);
                    foreach (var items in item.TechnicalSpecialistTaxonomy)
                    {
                        string lstrTaxonomyName = string.Format("\"{0}\"", items.TaxonomyServices.TaxonomyServiceName);
                        int index = columns.IndexOf(lstrTaxonomyName);
                        if (index > -1)
                        {
                            tempRow[index] = "X";
                            if (string.IsNullOrEmpty(totalRow[index]))
                                totalRow[index] = "1";
                            else
                            {
                                int lintCount = Convert.ToInt32(totalRow[index]) + 1;
                                totalRow[index] = lintCount.ToString();
                            }

                        }
                    }
                    string lstrRow = string.Join(",", tempRow);
                    csvContent.AppendLine(lstrRow);
                }
                totalRow[1] = lintEmployeeCount.ToString();
                totalRow[2] = lintContractorCount.ToString();
                csvContent.AppendLine(string.Join(",", totalRow));
                using (var ms = new MemoryStream())
                {
                    TextWriter tw = new StreamWriter(ms);
                    tw.Write(csvContent.ToString());
                    tw.Flush();
                    ms.Position = 0;
                    bytes = ms.ToArray();
                }
            }
            return bytes;
        }

        public void Dispose()
        {
            _dbContext = null;
            _mapper = null;
        }
    }
}