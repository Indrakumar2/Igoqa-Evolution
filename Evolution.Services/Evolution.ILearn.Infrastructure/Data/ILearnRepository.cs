using AutoMapper;
using Evolution.Assignment.Infrastructure.Data;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.DbRepository.Models.SqlDatabaseContext;
using Evolution.GenericDbRepository.Services;
using Evolution.ILearn.Domain.Interfaces;
using Evolution.ILearn.Domain.Models;
using Evolution.ILearn.Domain.Models.ILearn;
using Evolution.Logging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ILearn.Domain.Models;
using TechDomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;



namespace Evolution.ILearn.Infrastructure.Data
{
    public class ILearnRepository : GenericRepository<DbModel.IlearnData>, ILearnRespositoryInterface
    {
        private DbModel.EvolutionSqlDbContext _dbContext = null;
        private IMapper _mapper = null;
        private IAppLogger<ILearnRepository> _logger = null;


        public ILearnRepository(DbModel.EvolutionSqlDbContext dbContext, IMapper mapper, IAppLogger<ILearnRepository> logger) : base(dbContext)
        {
            this._dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public List<TechDomainModel.TechnicalSpecialistCompetency> AddCompantencyData()
        {
            List<ILearnMapping> mappingData = new List<ILearnMapping>();
            Exception exception = null;
            List<TechDomainModel.TechnicalSpecialistCompetency> compAndTrainData = new List<TechDomainModel.TechnicalSpecialistCompetency>();
            try
            {
                DateTime getBeforeDate = DateTime.Now.AddDays(-1);
                getBeforeDate = getBeforeDate.Date;
                // var getILearnData = _dbContext.IlearnData;//For patch work need to remove
                var getILearnData = _dbContext.IlearnData.Where(x => x.CompletedDate >= getBeforeDate).ToList(); //For patch work need to remove
                var ILearnMapping = _dbContext.ILearnMapping.ToList();
                var joinedILearnData = getILearnData.Join(ILearnMapping,
                  LD => new { Id = LD.TrainingObjectId },
                  LM => new { Id = LM.IlearnObjectId },
                   (LD, LM) => new DomainModel.ILearn.ILearnAndMapping()
                   {
                       Training_Object_ID = LD.TrainingObjectId,
                       Training_Title = LD.TrainingTitle,
                       Transcript_Completed_Date = LD.CompletedDate,
                       GRM_ePin_ID = Convert.ToInt32(LD.TechnicalSpecialistId),
                       Transcript_Score = LD.Score?.ToString(),
                       TrainingType = LM.TrainingType,
                       Title = LM.Title,
                       IlearnId = LM.IlearnId,
                       Training_Hours = LD.TrainingHours?.ToString() ////For Sarah Review Changes
                   })?.ToList();

                joinedILearnData?.ForEach(eachData =>
                {
                    if (Convert.ToInt32(eachData.TrainingType) == Convert.ToInt32(MasterType.Competency))
                    {
                        TechDomainModel.TechnicalSpecialistCompetency obj = new TechDomainModel.TechnicalSpecialistCompetency
                        {
                            Epin = Convert.ToInt32(eachData?.GRM_ePin_ID),
                            RecordType = CompCertTrainingType.Co.ToString(),
                            Expiry = null,
                            Score = !string.IsNullOrEmpty(eachData.Transcript_Score) ? eachData.Transcript_Score.Equals("0.00") ? "Completed" : eachData.Transcript_Score.Replace(".00", "") : null,
                            Duration = eachData.Training_Hours,//This is for CO
                            Competency = !string.IsNullOrEmpty(eachData.Transcript_Score) && eachData.Transcript_Score.Equals("0.00") ? "Completed" : eachData.Transcript_Score == null ? eachData.Transcript_Score : "Pass", //For Sarah Review Changes
                            EffectiveDate = eachData.Transcript_Completed_Date,//This is for CO
                            Titile = eachData.Title,//Master Data purpose
                            ILearnID = Convert.ToInt32(eachData.IlearnId),
                            IsILearn = true
                        };
                        compAndTrainData.Add(obj);
                    }
                });
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            finally
            {

            }
            return compAndTrainData;
        }
        public IList<IlearnData> Save(LearnData learnData)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _mapper = null;
            _logger = null;
        }



        public List<TechDomainModel.TechnicalSpecialistInternalTraining> AddInternalTrainingData()
        {
            List<ILearnMapping> mappingData = new List<ILearnMapping>();
            List<TechDomainModel.TechnicalSpecialistInternalTraining> compAndTrainData = new List<TechDomainModel.TechnicalSpecialistInternalTraining>();
            try
            {
                DateTime getBeforeDate = DateTime.Now.AddDays(-1);
                getBeforeDate = getBeforeDate.Date;
                // var getILearnData = _dbContext.IlearnData; //For patch work need to remove
                var getILearnData = _dbContext.IlearnData.Where(x => x.CompletedDate >= getBeforeDate).ToList();
                var ILearnMapping = _dbContext.ILearnMapping.ToList();
                var joinedILearnData = getILearnData.Join(ILearnMapping,
                  LD => new { Id = LD.TrainingObjectId },
                  LM => new { Id = LM.IlearnObjectId },
                   (LD, LM) => new DomainModel.ILearn.ILearnAndMapping()
                   {
                       Training_Object_ID = LD.TrainingObjectId,
                       Training_Title = LD.TrainingTitle,
                       Transcript_Completed_Date = LD.CompletedDate,
                       GRM_ePin_ID = Convert.ToInt32(LD.TechnicalSpecialistId),
                       Transcript_Score = LD.Score?.ToString(),
                       TrainingType = LM.TrainingType,
                       Title = LM.Title,
                       IlearnId = LM.IlearnId
                   })?.ToList();

                joinedILearnData?.ForEach(eachData =>
                {
                    if (Convert.ToInt32(eachData.TrainingType) == Convert.ToInt32(MasterType.InternalTraining))
                    {
                        TechDomainModel.TechnicalSpecialistInternalTraining obj = new TechDomainModel.TechnicalSpecialistInternalTraining
                        {
                            Epin = Convert.ToInt32(eachData?.GRM_ePin_ID),
                            RecordType = CompCertTrainingType.IT.ToString(),
                            Expiry = null,
                            Score = !string.IsNullOrEmpty(eachData.Transcript_Score) ? eachData.Transcript_Score.Equals("0.00") ? "Completed" : eachData.Transcript_Score.Replace(".00", "") : null,
                            TrainingDate = eachData.Transcript_Completed_Date,//This is fot IT
                            Competency = !string.IsNullOrEmpty(eachData.Transcript_Score) && eachData.Transcript_Score.Equals("0.00") ? "Completed" : eachData.Transcript_Score == null ? eachData.Transcript_Score : "Pass", //For Sarah Review Changes
                            Title = eachData.Title,//Master Data purpose
                            ILearnID = Convert.ToInt32(eachData.IlearnId),
                            IsILearn = true
                        };
                        compAndTrainData.Add(obj);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return compAndTrainData;
        }


        public List<TechDomainModel.TechnicalSpecialistCertification> AddCertificateData()
        {
            List<ILearnMapping> mappingData = new List<ILearnMapping>();
            List<TechDomainModel.TechnicalSpecialistCertification> certificateData = new List<TechDomainModel.TechnicalSpecialistCertification>();
            try
            {
                DateTime getBeforeDate = DateTime.Now.AddDays(-1);
                getBeforeDate = getBeforeDate.Date;
                // var getILearnData = _dbContext.IlearnData; //For patch work need to remove
                var getILearnData = _dbContext.IlearnData.Where(x => x.CompletedDate >= getBeforeDate).ToList();
                var ILearnMapping = _dbContext.ILearnMapping.ToList();
                var joinedILearnData = getILearnData.Join(ILearnMapping,
                  LD => new { Id = LD.TrainingObjectId },
                  LM => new { Id = LM.IlearnObjectId },
                   (LD, LM) => new DomainModel.ILearn.ILearnAndMapping()
                   {
                       Training_Object_ID = LD.TrainingObjectId,
                       Training_Title = LD.TrainingTitle,
                       Transcript_Completed_Date = LD.CompletedDate,
                       GRM_ePin_ID = Convert.ToInt32(LD.TechnicalSpecialistId),
                       TrainingType = LM.TrainingType,
                       Title = LM.Title,
                       IlearnId = LM.IlearnId
                   })?.ToList();

                joinedILearnData?.ForEach(eachData =>
                {
                    if (Convert.ToInt32(eachData.TrainingType) == Convert.ToInt32(MasterType.Certifications))
                    {
                        TechDomainModel.TechnicalSpecialistCertification obj = new TechDomainModel.TechnicalSpecialistCertification
                        {
                            Epin = Convert.ToInt32(eachData?.GRM_ePin_ID),
                            RecordType = CompCertTrainingType.Ce.ToString(),
                            ExpiryDate = null,
                            EffeciveDate = eachData.Transcript_Completed_Date,//This is fot IT
                            Title = eachData.Title,//Master Data purpose
                            ILearnID = Convert.ToInt32(eachData.IlearnId),
                            VerificationStatus = "Verified",
                            VerificationType = "ILearn",
                            VerificationDate = eachData.Transcript_Completed_Date,
                            VerifiedBy = "iLearn",
                            IsILearn = true,
                            IsExternal = false
                        };
                        certificateData.Add(obj);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return certificateData;
        }

        public List<TechDomainModel.TechnicalSpecialistTraining> AddTrainingData()
        {
            List<ILearnMapping> mappingData = new List<ILearnMapping>();
            List<TechDomainModel.TechnicalSpecialistTraining> trainingData = new List<TechDomainModel.TechnicalSpecialistTraining>();
            try
            {
                DateTime getBeforeDate = DateTime.Now.AddDays(-1);
                getBeforeDate = getBeforeDate.Date;
                // var getILearnData = _dbContext.IlearnData; //For patch work need to remove
                var getILearnData = _dbContext.IlearnData.Where(x => x.CompletedDate >= getBeforeDate).ToList();
                var ILearnMapping = _dbContext.ILearnMapping.ToList();
                var joinedILearnData = getILearnData.Join(ILearnMapping,
                  LD => new { Id = LD.TrainingObjectId },
                  LM => new { Id = LM.IlearnObjectId },
                   (LD, LM) => new DomainModel.ILearn.ILearnAndMapping()
                   {
                       Training_Object_ID = LD.TrainingObjectId,
                       Training_Title = LD.TrainingTitle,
                       Transcript_Completed_Date = LD.CompletedDate,
                       GRM_ePin_ID = Convert.ToInt32(LD.TechnicalSpecialistId),
                       TrainingType = LM.TrainingType,
                       Title = LM.Title,
                       IlearnId = LM.IlearnId
                   })?.ToList();

                joinedILearnData?.ForEach(eachData =>
                {
                    if (Convert.ToInt32(eachData.TrainingType) == Convert.ToInt32(MasterType.Trainings))
                    {
                        TechDomainModel.TechnicalSpecialistTraining obj = new TechDomainModel.TechnicalSpecialistTraining
                        {
                            Epin = Convert.ToInt32(eachData?.GRM_ePin_ID),
                            RecordType = CompCertTrainingType.Tr.ToString(),
                            ExpiryDate = null,
                            EffeciveDate = eachData.Transcript_Completed_Date,//This is fot IT
                            Title = eachData.Title,//Master Data purpose
                            ILearnID = Convert.ToInt32(eachData.IlearnId),
                            VerificationStatus = "Verified",
                            VerificationType = "ILearn",
                            VerificationDate = eachData.Transcript_Completed_Date,
                            VerifiedBy = "iLearn",
                            IsILearn = true,
                            IsExternal = false
                        };
                        trainingData.Add(obj);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            return trainingData;
        }
    }
}
