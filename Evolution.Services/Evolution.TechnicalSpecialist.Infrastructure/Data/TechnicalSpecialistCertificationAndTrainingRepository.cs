using AutoMapper;
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
    public class TechnicalSpecialistCertificationAndTrainingRepository : GenericRepository<DbModel.TechnicalSpecialistCertificationAndTraining>, ITechnicalSpecialistCertificationAndTrainingRepository

    {
        private readonly DbModel.EvolutionSqlDbContext _dbContext = null;
        private readonly IMapper _mapper = null;

        #region Constructor 

        public TechnicalSpecialistCertificationAndTrainingRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        #endregion

        #region Get 
        
        public IList<DbModel.TechnicalSpecialistCertificationAndTraining> Get(IList<int> Ids)
        {
            return Get(certAndTrainingIds : Ids); 
        }

        public IList<DbModel.TechnicalSpecialistCertificationAndTraining> Get(IList<string> certificationOrTraingNames, CompCertTrainingType certificateOrTrainingType)
        {
            return Get(certificateOrTrainingType, null, null, null, certificationOrTraingNames);
        }

        public IList<DbModel.TechnicalSpecialistCertificationAndTraining> Get(IList<KeyValuePair<string, string>> ePinAndCertificationOrTraingNames, CompCertTrainingType certificateOrTrainingType)
        { 
            var pins = ePinAndCertificationOrTraingNames?.Select(x => x.Key).ToList();
            var certOrTraingNames = ePinAndCertificationOrTraingNames?.Select(x => x.Value).ToList();

            return Get(certificateOrTrainingType, null, null, pins, certOrTraingNames); 
        }

        public IList<DbModel.TechnicalSpecialistCertificationAndTraining> GetByPinId(IList<string> pinIds)
        {
            return Get(pins : pinIds);
        }

        #endregion

        #region Search

        public IList<DbModel.TechnicalSpecialistCertificationAndTraining> Search(TechnicalSpecialistCertification searchModel)
        {
            var tsCertificateAsQueryable = this.PopulateTsCertificationAndTrainingAsQuerable(certificationSearchModel : searchModel);
            return this.Get(CompCertTrainingType.Ce,tsCertificateAsQueryable, null, null, null);
        }

        public IList<DbModel.TechnicalSpecialistCertificationAndTraining> Search(TechnicalSpecialistTraining searchModel)
        {
            var tsTrainingAsQueryable = this.PopulateTsCertificationAndTrainingAsQuerable(trainingSearchModel : searchModel);
            return this.Get(CompCertTrainingType.Tr,tsTrainingAsQueryable, null, null, null);
        }

        #endregion

        #region Modify
        public void Update(List<KeyValuePair<DbModel.TechnicalSpecialistCertificationAndTraining, List<KeyValuePair<string, object>>>> tsCertificateAndTraining,
        params Expression<Func<DbModel.TechnicalSpecialistCertificationAndTraining, object>>[] updatedProperties)
        {
            tsCertificateAndTraining.ForEach(x =>
            {
                ObjectExtension.SetPropertyValue(x.Key, x.Value);
                base.Update(x.Key, updatedProperties);
            });

        }
        #endregion

        #region Private Methods

        private IList<DbModel.TechnicalSpecialistCertificationAndTraining> Get(CompCertTrainingType certificateOrTrainingType,IQueryable<DbModel.TechnicalSpecialistCertificationAndTraining> tsCertAndTrainingAsQuerable = null,
                                                            IList<int> certAndTrainingIds = null,
                                                            IList<string> pins = null,
                                                            IList<string> certAndTrainingName = null)
        {
            if (tsCertAndTrainingAsQuerable != null)
                tsCertAndTrainingAsQuerable = tsCertAndTrainingAsQuerable.Where(x=> x.RecordType == certificateOrTrainingType.ToString());
             
            return Get(tsCertAndTrainingAsQuerable, certAndTrainingIds, pins, certAndTrainingName);
        }

        private IList<DbModel.TechnicalSpecialistCertificationAndTraining> Get(IQueryable<DbModel.TechnicalSpecialistCertificationAndTraining> tsCertAndTrainingAsQuerable = null,
                                                    IList<int> certAndTrainingIds = null,
                                                    IList<string> pins = null,
                                                    IList<string> certAndTrainingName = null)
        {
            if (tsCertAndTrainingAsQuerable == null)
                tsCertAndTrainingAsQuerable = _dbContext.TechnicalSpecialistCertificationAndTraining;

            if (certAndTrainingIds?.Count > 0)
                tsCertAndTrainingAsQuerable = tsCertAndTrainingAsQuerable.Where(x => certAndTrainingIds.Contains(x.Id));

            if (certAndTrainingName?.Count > 0)
                tsCertAndTrainingAsQuerable = tsCertAndTrainingAsQuerable.Where(x => certAndTrainingName.Contains(x.CertificationAndTraining.Name));

            if (pins?.Count > 0)
                tsCertAndTrainingAsQuerable = tsCertAndTrainingAsQuerable.Where(x => pins.Contains(x.TechnicalSpecialist.Pin.ToString()));

            return tsCertAndTrainingAsQuerable.Include(x => x.TechnicalSpecialist)
                                                .Include(x => x.CertificationAndTraining)
                                                .Include(x => x.VerifiedBy)
                                                .ToList();
        }

        private IQueryable<DbModel.TechnicalSpecialistCertificationAndTraining> PopulateTsCertificationAndTrainingAsQuerable(TechnicalSpecialistTraining trainingSearchModel = null, TechnicalSpecialistCertification certificationSearchModel = null)
        {
            DbModel.TechnicalSpecialistCertificationAndTraining dbSearchModel = null;

            IQueryable<DbModel.TechnicalSpecialistCertificationAndTraining> tsCertificationAndTrainingAsQueryable = _dbContext.TechnicalSpecialistCertificationAndTraining;
             
            if (trainingSearchModel != null)
            {
                dbSearchModel = _mapper.Map<DbModel.TechnicalSpecialistCertificationAndTraining>(trainingSearchModel);

                if (trainingSearchModel != null && trainingSearchModel.Epin > 0)
                { 
                    tsCertificationAndTrainingAsQueryable = tsCertificationAndTrainingAsQueryable.Where(x => x.TechnicalSpecialist.Pin == trainingSearchModel.Epin);
                } 
                if (trainingSearchModel.TrainingName.HasEvoWildCardChar())
                {
                    tsCertificationAndTrainingAsQueryable = tsCertificationAndTrainingAsQueryable.WhereLike(x => x.CertificationAndTraining.Name, trainingSearchModel.TrainingName, '*');
                }
                if (trainingSearchModel.VerifiedBy.HasEvoWildCardChar())
                {
                    tsCertificationAndTrainingAsQueryable = tsCertificationAndTrainingAsQueryable.WhereLike(x => x.VerifiedBy.SamaccountName, trainingSearchModel.VerifiedBy, '*');
                } 
            }
            else
            {
                dbSearchModel = _mapper.Map<DbModel.TechnicalSpecialistCertificationAndTraining>(certificationSearchModel);

                if (certificationSearchModel != null && certificationSearchModel.Epin > 0)
                {
                    tsCertificationAndTrainingAsQueryable = tsCertificationAndTrainingAsQueryable.Where(x => x.TechnicalSpecialist.Pin == certificationSearchModel.Epin);
                } 
                if (certificationSearchModel.CertificationName.HasEvoWildCardChar())
                {
                    tsCertificationAndTrainingAsQueryable = tsCertificationAndTrainingAsQueryable.WhereLike(x => x.CertificationAndTraining.Name, certificationSearchModel.CertificationName, '*');
                } 
                if (certificationSearchModel.VerifiedBy.HasEvoWildCardChar())
                {
                    tsCertificationAndTrainingAsQueryable = tsCertificationAndTrainingAsQueryable.WhereLike(x => x.VerifiedBy.SamaccountName, certificationSearchModel.VerifiedBy, '*');
                }
            }
             
            var defWhereExpr = dbSearchModel.ToExpression();
            if (defWhereExpr != null)
                tsCertificationAndTrainingAsQueryable = tsCertificationAndTrainingAsQueryable.Where(defWhereExpr);

            return tsCertificationAndTrainingAsQueryable;
        }

        #endregion
         
    }
}

