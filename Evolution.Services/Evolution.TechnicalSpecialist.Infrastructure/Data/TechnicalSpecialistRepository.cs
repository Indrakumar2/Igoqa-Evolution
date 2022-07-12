using AutoMapper;
using AutoMapper.QueryableExtensions;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.GenericDbRepository.Services;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Infrastructure.Data
{
    public class TechnicalSpecialistInfoRepository : GenericRepository<DbModel.TechnicalSpecialist>,
                                                     ITechnicalSpecialistRepository
    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        public TechnicalSpecialistInfoRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public IList<BaseTechnicalSpecialistInfo> Search(string companyCode,string logonName = null, IList<int> ePins = null)
        {
            IQueryable<DbModel.TechnicalSpecialist> tsAsQueryable = _dbContext.TechnicalSpecialist;
            if (!string.IsNullOrEmpty(companyCode))
                tsAsQueryable = tsAsQueryable.Where(x => x.Company.Code == companyCode);
            if (!string.IsNullOrEmpty(logonName))
                tsAsQueryable = tsAsQueryable.Where(x => x.LogInName == logonName);
            if (ePins?.Count > 0)
                tsAsQueryable = tsAsQueryable.Where(x => ePins.Contains(x.Pin));

            return tsAsQueryable.ProjectTo<BaseTechnicalSpecialistInfo>().ToList();

        }

        public IList<TechnicalSpecialistInfo> Search(BaseTechnicalSpecialistInfo model,
                                                     params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            var tsInfo = _mapper.Map<TechnicalSpecialistInfo>(model);
            return _mapper.Map<IList<TechnicalSpecialistInfo>>(this.Search(tsInfo, includes));
        }

        public IList<DbModel.TechnicalSpecialist> Search(TechnicalSpecialistInfo model,
                                                         params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            var tsAsQueryable = this.PopulateTsAsQuerable(model);
            return this.Get(tsAsQueryable, null, null, includes);
        }

        public IList<DbModel.TechnicalSpecialist> Search(TechnicalSpecialistInfo model,string[] includes)
        {
            var tsAsQueryable = this.PopulateTsAsQuerable(model);
            return this.Get(tsAsQueryable, includes);
        }

        public IList<DbModel.TechnicalSpecialist> Get(IList<int> ids,
                                                      params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            return Get(null, ids, null, includes);
        }


        public IList<DbModel.TechnicalSpecialist> Get(IList<string> pins,
                                                      params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            return Get(null, null, pins, includes);
        }

        public IList<TechnicalSpecialistExpiredRecord> GetExpiredRecords()
        {
            DateTime? expiryDate = DateTime.Now.AddDays(90).Date;
            IList<TechnicalSpecialistExpiredRecord> expiredRecord = new List<TechnicalSpecialistExpiredRecord>();
           var expiredDrivingAndPassport = _dbContext.TechnicalSpecialist.Include(x => x.TechnicalSpecialistContact)
                      .Where(x => x.ProfileStatusId != 2997 && (x.PassportExpiryDate.Value.Date ==  expiryDate.Value.Date || x.DrivingLicenseExpiryDate.Value.Date == expiryDate.Value.Date))
                       .Select(x => x).ToList();

            var expiredTrainingAndCertificate = _dbContext.TechnicalSpecialistCertificationAndTraining
                        .Include(x => x.TechnicalSpecialist)
                        .ThenInclude(x => x.TechnicalSpecialistContact)
                        .Where(x => x.TechnicalSpecialist.ProfileStatusId != 2997 && x.ExpiryDate.Value.Date == expiryDate.Value.Date)
                        .Select(x => x ).ToList();

            if (expiredDrivingAndPassport != null && expiredDrivingAndPassport.Any())
            {
                expiredDrivingAndPassport.ForEach(x=> {
                    if (x.PassportExpiryDate == expiryDate)
                    {
                        expiredRecord.Add(new TechnicalSpecialistExpiredRecord
                        {
                            CompanyId = x.CompanyId,
                            DocumentType = "Passport",
                            Email = x.TechnicalSpecialistContact?.FirstOrDefault(x1 => x1.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress,
                            ExpiryDate = x.PassportExpiryDate,
                            TechnicalSpecialistId = x.Id
                        });
                    }
                    if (x.DrivingLicenseExpiryDate == expiryDate)
                    {
                        expiredRecord.Add(new TechnicalSpecialistExpiredRecord
                        {
                            CompanyId = x.CompanyId,
                            DocumentType = "Driving Licence",
                            Email = x.TechnicalSpecialistContact?.FirstOrDefault(x1 => x1.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress,
                            ExpiryDate = x.DrivingLicenseExpiryDate,
                            TechnicalSpecialistId = x.Id
                        });
                    }
                } );
            }

            if (expiredTrainingAndCertificate != null && expiredTrainingAndCertificate.Any())
            {
                expiredTrainingAndCertificate.ForEach(x=> {
                    expiredRecord.Add(new TechnicalSpecialistExpiredRecord
                    {
                        CompanyId = x.TechnicalSpecialist.CompanyId,
                        DocumentType =x.RecordType== "Ce"? "Certificate" : "Training",
                        Email = x.TechnicalSpecialist.TechnicalSpecialistContact?.FirstOrDefault(x1 => x1.ContactType == ContactType.PrimaryEmail.ToString())?.EmailAddress,
                        ExpiryDate = x.ExpiryDate,
                        TechnicalSpecialistId = x.TechnicalSpecialistId
                    });
                });
            } 

            return expiredRecord;
        }

        private IList<DbModel.TechnicalSpecialist> Get(IQueryable<DbModel.TechnicalSpecialist> tsAsQuerable = null,
                                                       IList<int> ids = null,
                                                       IList<string> pins = null,
                                                       params Expression<Func<DbModel.TechnicalSpecialist, object>>[] includes)
        {
            if (tsAsQuerable == null)
                tsAsQuerable = _dbContext.TechnicalSpecialist;

            if (ids?.Count > 0)
                tsAsQuerable = tsAsQuerable.Where(x => ids.Contains(x.Id));
            if (pins?.Count > 0)
                tsAsQuerable = tsAsQuerable.Where(x => pins.Contains(x.Pin.ToString()));

            if (includes.Any())
                tsAsQuerable = includes.Aggregate(tsAsQuerable, (current, include) => current.Include(include));
            else
                tsAsQuerable = tsAsQuerable.Include(x => x.Company)
                                           .Include(x => x.CompanyPayroll)
                                           .Include(x => x.EmploymentType)
                                           .Include(x => x.PassportCountryOrigin)
                                           .Include(x => x.ProfileAction)
                                           .Include(x => x.ProfileStatus)
                                           .Include(x => x.SubDivision);

            return tsAsQuerable.ToList();
        }

        private IList<DbModel.TechnicalSpecialist> Get(IQueryable<DbModel.TechnicalSpecialist> tsAsQuerable = null,string[] includes=null)
        {
            if (tsAsQuerable == null)
                tsAsQuerable = _dbContext.TechnicalSpecialist;

            if ( includes?.Length>0)
                tsAsQuerable = includes.Aggregate(tsAsQuerable, (current, include) => current.Include(include));
            
            return tsAsQuerable.ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialist> PopulateTsAsQuerable(TechnicalSpecialistInfo model)
        {
            var dbSearchModel = this._mapper.Map<DbModel.TechnicalSpecialist>(model);
            IQueryable<DbModel.TechnicalSpecialist> tsAsQueryable = _dbContext.TechnicalSpecialist;

            #region Wildcard Search for Company Code
            if (model.CompanyCode.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.Company.Code, model.CompanyCode, '*');
            else if (!string.IsNullOrEmpty(model.CompanyCode))
                tsAsQueryable = tsAsQueryable.Where(x => x.Company.Code == model.CompanyCode);
            #endregion

            #region Wildcard Search for Company Name
            if (model.CompanyName.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.Company.Name, model.CompanyName, '*');
            else if (!string.IsNullOrEmpty(model.CompanyName))
                tsAsQueryable = tsAsQueryable.Where(x => x.Company.Name == model.CompanyName);
            #endregion

            #region  Wildcard Search for Company Sub Division
            if (model.SubDivisionName.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.SubDivision.Name, model.SubDivisionName, '*');
            else if (!string.IsNullOrEmpty(model.SubDivisionName))
                tsAsQueryable = tsAsQueryable.Where(x => x.SubDivision.Name == model.SubDivisionName);
            #endregion

            #region Wildcard Search for Profile Status
            if (model.ProfileStatus.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.ProfileStatus.Name, model.ProfileStatus, '*');
            else if (!string.IsNullOrEmpty(model.ProfileStatus))
                tsAsQueryable = tsAsQueryable.Where(x => x.ProfileStatus.Name == model.ProfileStatus);
            #endregion

            #region Wildcard Search for Employe Type
            if (model.EmploymentType.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.EmploymentType.Name, model.EmploymentType, '*');
            else if (!string.IsNullOrEmpty(model.EmploymentType))
                tsAsQueryable = tsAsQueryable.Where(x => x.EmploymentType.Name == model.EmploymentType);
            #endregion

            #region  Wildcard Search for Company Payroll Type
            if (model.CompanyPayrollName.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.CompanyPayroll.Name, model.CompanyPayrollName, '*');//Defect 45 Changes
            else if (!string.IsNullOrEmpty(model.CompanyPayrollName))
                tsAsQueryable = tsAsQueryable.Where(x => x.CompanyPayroll.Name == model.CompanyPayrollName);//Defect 45 Changes
            #endregion

            #region  Wildcard Search for Passport Origin Country
            if (model.PassportCountryName.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.PassportCountryOrigin.Name, model.PassportCountryName, '*');
            else if (!string.IsNullOrEmpty(model.PassportCountryName))
                tsAsQueryable = tsAsQueryable.Where(x => x.PassportCountryOrigin.Name == model.PassportCountryName);
            #endregion

            #region  Case in-sensitive search for firstname
            if (model.FirstName.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.FirstName.ToLower(), model.FirstName.ToLower(), '*');
            else if (!string.IsNullOrEmpty(model.FirstName))
                tsAsQueryable = tsAsQueryable.Where(x => x.FirstName.ToLower() == model.FirstName.ToLower());
            #endregion

            #region  Case in-sensitive search for lastname
            if (model.LastName.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.LastName.ToLower(), model.LastName.ToLower(), '*');
            else if (!string.IsNullOrEmpty(model.LastName))
                tsAsQueryable = tsAsQueryable.Where(x => x.LastName.ToLower() == model.LastName.ToLower());
            #endregion

            if (model.ePinString.HasEvoWildCardChar())
                tsAsQueryable = tsAsQueryable.WhereLike(x => x.Pin.ToString(), model.ePinString, '*');
            else if(!string.IsNullOrEmpty(model.ePinString))
                tsAsQueryable = tsAsQueryable.Where(x => x.Pin.ToString().Equals(model.ePinString));

            if (model.Epins?.Count > 0)
                tsAsQueryable = tsAsQueryable.Where(x => model.Epins.Contains(x.Pin.ToString()));

            var defWhereExpr = dbSearchModel.ToExpression(new List<string> { "IsEreportingQualified", "IsTsCredSent" });
            if (defWhereExpr != null)
                tsAsQueryable = tsAsQueryable.Where(defWhereExpr);

            return tsAsQueryable;
        }

        public void Update(List<KeyValuePair<DbModel.TechnicalSpecialist, List<KeyValuePair<string, object>>>> technicalSpecialist,
        params Expression<Func<DbModel.TechnicalSpecialist, object>>[] updatedProperties)
        {
            technicalSpecialist.ForEach(x =>
            {
                ObjectExtension.SetPropertyValue(x.Key, x.Value);
                base.Update(x.Key, updatedProperties);
            });
           
        }
    }
    
}
