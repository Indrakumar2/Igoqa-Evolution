using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.Reports.Domain.Interfaces.Data;
using Evolution.Reports.Domain.Models.Reports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.Reports.Domain.Models.Reports;

namespace Evolution.Reports.Infrastructure.Data
{
    public class TaxonomyRepository : GenericRepository<DbModel.TechnicalSpecialist>, ITaxonomyRepository
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;

        public TaxonomyRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IList<DomainModel.Taxonomy> Search(DomainModel.Taxonomy searchModel)
        {
            List<DomainModel.Taxonomy> resultData = new List<Taxonomy>();
            resultData = (
                                from ETS in _dbContext.TechnicalSpecialist.Include(a=>a.TechnicalSpecialistStamp)
                                join MD4 in _dbContext.Data on new { Col1 = ETS.SubDivisionId, Col2 = 44 } equals new { Col1 = MD4.Id, Col2 = MD4.MasterDataTypeId }
                                join MD1 in _dbContext.Data on new { Col1 = ETS.ProfileStatusId, Col2 = 45 } equals new { Col1 = MD1.Id, Col2 = MD1.MasterDataTypeId }
                                join MD2 in _dbContext.Data on new { Col1 = ETS.EmploymentTypeId, Col2 = 46 } equals new { Col1 = (int?)MD2.Id, Col2 = MD2.MasterDataTypeId } into Master
                                from _MD2 in Master.DefaultIfEmpty()
                                join ETST in _dbContext.TechnicalSpecialistTaxonomy on ETS.Id equals ETST.TechnicalSpecialistId
                                join MD3 in _dbContext.Data on new { Col1 = ETST.TaxonomyCategoryId, Col2 = 25 } equals new { Col1 = MD3.Id, Col2 = MD3.MasterDataTypeId } into Master1
                                from _MD3 in Master1.DefaultIfEmpty()
                                join ETSC1 in _dbContext.TechnicalSpecialistContact on new { Col1 = ETS.Id, Col2 = "PrimaryMobile" } equals new { Col1 = ETSC1.TechnicalSpecialistId, Col2 = ETSC1.ContactType } into _ETSC1
                                from Mobile in _ETSC1.DefaultIfEmpty()
                                join ETSC2 in _dbContext.TechnicalSpecialistContact on new { Col1 = ETS.Id, Col2 = "PrimaryEmail" } equals new { Col1 = ETSC2.TechnicalSpecialistId, Col2 = ETSC2.ContactType } into _ETSC2
                                from Email in _ETSC2.DefaultIfEmpty()
                                join ETSC3 in _dbContext.TechnicalSpecialistContact on new { Col1 = ETS.Id, Col2 = "PrimaryAddress" } equals new { Col1 = ETSC3.TechnicalSpecialistId, Col2 = ETSC3.ContactType } into _ETSC3
                                from Address in _ETSC3.DefaultIfEmpty()
                                where searchModel.CompanyIds.Contains(ETS.Company.Id)
                                select new Taxonomy
                                {
                                    FirstName = ETS.FirstName,
                                    LastName = ETS.LastName,
                                    MiddleName = ETS.MiddleName,
                                    CompanyCode = ETS.Company.Code,
                                    ProfileStatus = MD1.Name,
                                    EmploymentStatus = _MD2.Name,
                                    Category = _MD3.Name,
                                    RAMP = ETS.IsReviewAndModerationProcess.HasValue && ETS.IsReviewAndModerationProcess.Value ? "Yes" : "No",
                                    SubCategory = ETST.TaxonomySubCategory.TaxonomySubCategoryName,
                                    CompanyId = ETS.Company.Id,
                                    CompanyName = ETS.Company.Name,
                                    Service = ETST.TaxonomyServices.TaxonomyServiceName,
                                    ApprovalStatus = ETST.ApprovalStatus,
                                    EPin = ETS.Id,
                                    EReporting = ETS.IsEreportingQualified.HasValue && ETS.IsEreportingQualified.Value ? "Yes" : "No",
                                    SubDivision = MD4.Name,
                                    Mobile = Mobile.MobileNumber,
                                    Email = Email.EmailAddress,
                                    PostalCode = Address.PostalCode,
                                    City = Address.City.Name,
                                    Country = Address.Country.Name,
                                    County = Address.County.Name,
                                    HardStampNumber  = ETS.TechnicalSpecialistStamp.OrderByDescending(a => a.Id).FirstOrDefault(a => a.ReturnDate == null && !a.IsSoftStamp && a.TechnicalSpecialistId == ETS.Id).StampNumber,
                                    SoftStampNumber = ETS.TechnicalSpecialistStamp.OrderByDescending(a => a.Id).FirstOrDefault(a => a.ReturnDate == null && a.IsSoftStamp && a.TechnicalSpecialistId == ETS.Id).StampNumber,
                                    HardStampCountryCode = ETS.TechnicalSpecialistStamp.Any(a => a.ReturnDate == null && !a.IsSoftStamp && a.TechnicalSpecialistId == ETS.Id) ? ETS.TechnicalSpecialistStamp.OrderByDescending(a => a.Id).FirstOrDefault(a => a.ReturnDate == null && !a.IsSoftStamp && a.TechnicalSpecialistId == ETS.Id).Country.Code : "",
                                    SoftStampCountryCode = ETS.TechnicalSpecialistStamp.Any(a => a.ReturnDate == null && a.IsSoftStamp && a.TechnicalSpecialistId == ETS.Id) ? ETS.TechnicalSpecialistStamp.OrderByDescending(a => a.Id).FirstOrDefault(a => a.ReturnDate == null && a.IsSoftStamp && a.TechnicalSpecialistId == ETS.Id).Country.Code : ""
                                }
                            ).ToList();

            
            if (searchModel.CompanyIds.Count > 0)
                resultData = resultData.Where(x => searchModel.CompanyIds.Contains(x.CompanyId)).ToList();

            if (searchModel.ResourceEpins != null && searchModel.ResourceEpins.Count > 0)
                resultData = resultData.Where(x => searchModel.ResourceEpins.Contains(x.EPin)).ToList();

            if (searchModel.EpinList != null && searchModel.EpinList.Count > 0)
                resultData = resultData.Where(x => searchModel.EpinList.Contains(x.EPin)).ToList();

            if (!string.IsNullOrEmpty(searchModel.ProfileStatus))
                resultData = resultData.Where(x => x.ProfileStatus.Contains(searchModel.ProfileStatus)).ToList();

            if (!string.IsNullOrEmpty(searchModel.EmploymentStatus))
                resultData = resultData.Where(x => x.EmploymentStatus.Contains(searchModel.EmploymentStatus)).ToList();

            if (!string.IsNullOrEmpty(searchModel.ApprovalStatus))
                resultData = resultData.Where(x => x.ApprovalStatus.Contains(searchModel.ApprovalStatus)).ToList();

            if (!string.IsNullOrEmpty(searchModel.Category))
                resultData = resultData.Where(x => x.Category.Contains(searchModel.Category)).ToList();

            if (!string.IsNullOrEmpty(searchModel.SubCategory))
                resultData = resultData.Where(x => x.SubCategory.Contains(searchModel.SubCategory)).ToList();

            if (!string.IsNullOrEmpty(searchModel.Service))
                resultData = resultData.Where(x => x.Service.Contains(searchModel.Service)).ToList();
            return resultData;
        }
    }
}