using AutoMapper;
using Evolution.Common.Enums;
using Evolution.Common.Extensions;
using Evolution.Common.Models.Messages;
using Evolution.Common.Models.Responses;
using Evolution.ILearn.Domain.Interfaces;
using Evolution.ILearn.Domain.Models;
using Evolution.Logging.Interfaces;
using Evolution.TechnicalSpecialist.Domain.Interfaces.Data;
using Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using DomainModel = Evolution.ILearn.Domain.Models;
using DomainModelTech = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using EFCore.BulkExtensions;


namespace Evolution.ILearn.Core.Services
{
   public class ILearnService : ILearnInterface
    {
        private readonly IMapper _mapper = null;
        private readonly IAppLogger<ILearnService> _logger = null;
        private readonly ILearnRespositoryInterface _learnRespositoryInterface = null;
        private readonly ITechnicalSpecialistTrainingAndCompetencyRepository _technicalSpecialistTrainingAndCompetencyRepository = null;
        private readonly ITechnicalSpecialistTrainingAndCompetencyTypeRepository _technicalSpecialistTrainingAndCompetencyTypeRepository = null;
        private readonly ITechnicalSpecialistTrainingAndCompetancyTypeService _tsTrainingAndCompetancyTypeService = null;
        private readonly ITechnicalSpecialistCertificationAndTrainingRepository _technicalSpecialistCertificationAndTrainingRepository = null;
        private DbModel.EvolutionSqlDbContext _dbContext = null;

        public ILearnService(ILearnRespositoryInterface learnRespositoryInterface, 
                                IAppLogger<ILearnService> logger, IMapper mapper, 
                                DbModel.EvolutionSqlDbContext dbContext,
                                ITechnicalSpecialistTrainingAndCompetencyRepository technicalSpecialistTrainingAndCompetencyRepository,
                                ITechnicalSpecialistTrainingAndCompetencyTypeRepository technicalSpecialistTrainingAndCompetencyTypeRepository,
                                ITechnicalSpecialistTrainingAndCompetancyTypeService tsTrainingAndCompetancyTypeService,
                                ITechnicalSpecialistCertificationAndTrainingRepository technicalSpecialistCertificationAndTrainingRepository)
        {
            this._dbContext = dbContext;
            this._logger = logger;
            _learnRespositoryInterface = learnRespositoryInterface;
            _mapper = mapper;
            _technicalSpecialistTrainingAndCompetencyRepository = technicalSpecialistTrainingAndCompetencyRepository;
            _technicalSpecialistTrainingAndCompetencyTypeRepository = technicalSpecialistTrainingAndCompetencyTypeRepository;
            _tsTrainingAndCompetancyTypeService = tsTrainingAndCompetancyTypeService;
            _technicalSpecialistCertificationAndTrainingRepository = technicalSpecialistCertificationAndTrainingRepository;
        }

       
        public Response Save(List<LearnData> IlearnDatas)
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            int count = 0;
            _learnRespositoryInterface.AutoSave = false;
            try
            {
                    if (IlearnDatas.Count > 0)
                    {
                        var dbRecordToBeInserted = _mapper.Map<IList<DbModel.IlearnData>>(IlearnDatas);
                        _learnRespositoryInterface.Add(dbRecordToBeInserted);
                        _learnRespositoryInterface.ForceSave();
                         count = dbRecordToBeInserted.Count();
                    }
            }
            catch(Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), IlearnDatas);
            }
            finally
            {
                _learnRespositoryInterface.AutoSave = true;
            }
            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception, count);
        }

        public Response AddCompantancyData()
        {
            Exception exception = null;
            IList<TechnicalSpecialistCompetency> recordToBeAdd = null;
            IList<ValidationMessage> validationMessages = new List<ValidationMessage>();
            List<TechnicalSpecialistInternalTrainingAndCompetencyType> technicalSpecialistInternalTrainingAndCompetencyTypes = new List<TechnicalSpecialistInternalTrainingAndCompetencyType>();
            int count = 0;
            try
            {
                _technicalSpecialistTrainingAndCompetencyRepository.AutoSave = false;
                recordToBeAdd = _learnRespositoryInterface.AddCompantencyData();
                if (recordToBeAdd.Count > 0)
                {
                    IList<int> tsEpin = recordToBeAdd.Select(x => x.Epin).ToList();// Get the ILearn Epins
                    var dbTsInfo = _dbContext.TechnicalSpecialist.Where(x => tsEpin.Contains(x.Pin)).ToList();//(check the ILearn epins are Valid or Not)
                    IList<int> dbtsEpin = dbTsInfo.Select(x => x.Pin).ToList();// Get all DB epins based on Recordto  be added
                    var finalRecordToBeAdded = recordToBeAdd.Where(x => dbtsEpin.Contains(x.Epin)).ToList();// ONly add valid epins
                    var dbCompetencyIds = _dbContext.Data.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Competency))?.Select(a => a.Id)?.ToList();
                    finalRecordToBeAdded = finalRecordToBeAdded.Where(x => dbCompetencyIds.Contains(x.ILearnID)).ToList();

                    if (finalRecordToBeAdded.Count > 0)
                    {
                        var dbRecordToBeInserted = _mapper.Map<IList<DbModel.TechnicalSpecialistTrainingAndCompetency>>(finalRecordToBeAdded, opt =>
                        {
                            opt.Items["isAssignId"] = false;
                            opt.Items["DbTechSpecialist"] = dbTsInfo;
                        });
                        dbRecordToBeInserted = dbRecordToBeInserted?.Where(x2 => x2.TechnicalSpecialistId != 0).ToList();
                        dbRecordToBeInserted?.ToList().ForEach(x => {
                            x.IsIlearn = true;
                        });
                        _technicalSpecialistTrainingAndCompetencyRepository.Add(dbRecordToBeInserted);
                        var savedCnt = _technicalSpecialistTrainingAndCompetencyRepository.ForceSave();//Save Competency Data in DB
                        var dbTsCompetencies = dbRecordToBeInserted;
                        if (savedCnt > 0)
                        {
                            // competency Type Save Start
                            count = savedCnt;
                            int i = 0;
                            finalRecordToBeAdded.ForEach(x =>
                            {
                                TechnicalSpecialistInternalTrainingAndCompetencyType tempData = new TechnicalSpecialistInternalTrainingAndCompetencyType
                                {
                                    TechnicalSpecialistInternalTrainingAndCompetencyId = dbRecordToBeInserted[i].Id, //Removed Epin,Score,TrainingDate Where Condition Bcz some cases epin,traingDate and Score fields are same but Title is different 
                                    TypeId = x.ILearnID
                                };
                                technicalSpecialistInternalTrainingAndCompetencyTypes.Add(tempData);
                                i++;
                            });

                            if (technicalSpecialistInternalTrainingAndCompetencyTypes.Count > 0)
                            {
                                var finalData = technicalSpecialistInternalTrainingAndCompetencyTypes.Where(a => a.TypeId != null).ToList();
                                var TypeIDs = finalData.Select(x => x.TypeId).ToList();
                                var dbDatas = _dbContext.Data.Where(x => x.MasterDataTypeId == System.Convert.ToInt32(MasterType.Competency) && TypeIDs.Contains(x.Id)).ToList();
                                _technicalSpecialistTrainingAndCompetencyTypeRepository.AutoSave = false;
                                var mappedRecords2 = _mapper.Map<IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType>>(technicalSpecialistInternalTrainingAndCompetencyTypes, opt =>
                                {
                                    opt.Items["isAssignId"] = false;
                                    opt.Items["DBMasterTrainingAndCompetency"] = dbDatas;
                                });
                                _technicalSpecialistTrainingAndCompetencyTypeRepository.Add(mappedRecords2);
                            }
                            var finaltypeCount = _technicalSpecialistTrainingAndCompetencyTypeRepository.ForceSave();
                            //  competency Type Save Start END
                            Console.WriteLine("competencytypetable inserted successfully");

                        if (finaltypeCount > 0)
                        {
                            var dbTsIntTrainingAndCompetency = _dbContext.TechnicalSpecialistTrainingAndCompetency.Where(x => dbtsEpin.Contains(x.TechnicalSpecialistId)).ToList();
                            var dbTsCompetencyTrainingType = _dbContext.TechnicalSpecialistTrainingAndCompetencyType;
                            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsTrainingandCompetnecy = new List<DbModel.TechnicalSpecialistTrainingAndCompetency>();
                            IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsTrainingandCompetnecyUpdate = new List<DbModel.TechnicalSpecialistTrainingAndCompetency>();

                                if (dbTsIntTrainingAndCompetency.Count > 0)
                                {
                                    List<TechnicalSpecialistCompetency> finalResult = new List<TechnicalSpecialistCompetency>();
                                  
                                    var Joined = dbTsIntTrainingAndCompetency.Join(dbTsCompetencyTrainingType,
                                               CM => new { CM.Id },
                                               CT => new { Id = CT.TechnicalSpecialistTrainingAndCompetencyId },
                                                (CM, CT) => new { CM, CT }).Where(x => x.CM.Id == x.CT.TechnicalSpecialistTrainingAndCompetencyId)
                                                .Select(a => new {
                                                    dataId = a.CT.TrainingOrCompetencyDataId,
                                                    competencyId = a.CT.TechnicalSpecialistTrainingAndCompetencyId,
                                                    tsId = a.CM.TechnicalSpecialistId,
                                                    trainingDate = a.CM.TrainingDate,
                                                }).ToList();

                                    var Result = finalRecordToBeAdded.Join(Joined,
                                        Ad => new { Id = Ad.ILearnID },
                                        Jo => new { Id = Jo.dataId },
                                        (Ad, Jo) => new { Ad, Jo }).Where(y => y.Ad.ILearnID == y.Jo.dataId && y.Ad.Epin == y.Jo.tsId && y.Ad.EffectiveDate > y.Jo.trainingDate)
                                          .Select(x => new {
                                              trainingDate = x.Ad.EffectiveDate,
                                              x.Jo.competencyId,
                                          }).ToList();

                                    if (Result.Count() > 0)
                                     {
                                        Result.Distinct().ToList().ForEach(comp =>
                                        {
                                            var modify = dbTsIntTrainingAndCompetency.Where(x => x.Id == comp.competencyId).FirstOrDefault();
                                            if (modify != null)
                                            {
                                                    modify.Expiry = comp.trainingDate;
                                                    dbTsTrainingandCompetnecy.Add(modify);
                                            }
                                        });
                                     }
                                    dbTsTrainingandCompetnecyUpdate = dbTsTrainingandCompetnecy.Distinct().ToList();
                          _dbContext.BulkUpdate(dbTsTrainingandCompetnecyUpdate);
                          _dbContext.SaveChanges();
                           Console.WriteLine("CompetencyTable Table Updated Successfully");
                       }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            finally
            {
                _technicalSpecialistTrainingAndCompetencyRepository.AutoSave = true;
                _technicalSpecialistTrainingAndCompetencyTypeRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception, count);
        }

        public Response AddInternalTrainingData()
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            IList<TechnicalSpecialistInternalTraining> recordToBeAdd = null;
            List<TechnicalSpecialistInternalTrainingAndCompetencyType> technicalSpecialistInternalTrainingAndCompetencyTypes = new List<TechnicalSpecialistInternalTrainingAndCompetencyType>();
            int count = 0;
            try
            {
                _technicalSpecialistTrainingAndCompetencyRepository.AutoSave = false;
                recordToBeAdd = _learnRespositoryInterface.AddInternalTrainingData();
                if (recordToBeAdd.Count > 0)
                {
                    IList<int> tsEpin = recordToBeAdd.Select(x => x.Epin).ToList();// Get the ILearn Epins
                    var dbTsInfo = _dbContext.TechnicalSpecialist.Where(x => tsEpin.Contains(x.Pin)).ToList();//(check the ILearn epins are Valid or Not)
                    IList<int> dbtsEpin = dbTsInfo.Select(x => x.Pin).ToList();// Get all DB epins based on Recordto  be added
                    var finalRecordToBeAdded = recordToBeAdd.Where(x => dbtsEpin.Contains(x.Epin)).ToList();// ONly add valid epins
                    var dbInternalTrainingIds = _dbContext.Data.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.InternalTraining))?.Select(a => a.Id)?.ToList();
                    finalRecordToBeAdded = finalRecordToBeAdded.Where(x => dbInternalTrainingIds.Contains(x.ILearnID)).ToList();
                    if (finalRecordToBeAdded.Count > 0)
                    {
                        var dbRecordToBeInserted = _mapper.Map<IList<DbModel.TechnicalSpecialistTrainingAndCompetency>>(finalRecordToBeAdded, opt =>
                       {
                           opt.Items["isAssignId"] = false;
                           opt.Items["DbTechSpecialist"] = dbTsInfo;
                       });
                        dbRecordToBeInserted = dbRecordToBeInserted?.Where(x2 => x2.TechnicalSpecialistId != 0).ToList();
                        dbRecordToBeInserted?.ToList().ForEach(x =>
                        {
                            x.IsIlearn = true;
                        });
                        _technicalSpecialistTrainingAndCompetencyRepository.Add(dbRecordToBeInserted);//Save Internal Training Data
                        var savedCnt = _technicalSpecialistTrainingAndCompetencyRepository.ForceSave();
                        Console.WriteLine("Internal Traning Table Inserted Successfully");
                        if (savedCnt > 0)
                        {
                            // Training Type Save Start
                            count = savedCnt;
                            int i = 0;
                            finalRecordToBeAdded.ForEach(x =>
                            {
                                TechnicalSpecialistInternalTrainingAndCompetencyType tempData = new TechnicalSpecialistInternalTrainingAndCompetencyType
                                {
                                    TechnicalSpecialistInternalTrainingAndCompetencyId = dbRecordToBeInserted[i].Id, //Removed Epin,Score,TrainingDate Where Condition Bcz some cases epin,traingDate and Score fields are same but Title is different 
                                    TypeId = x.ILearnID
                                };
                                technicalSpecialistInternalTrainingAndCompetencyTypes.Add(tempData);
                                i++;
                            });

                            if (technicalSpecialistInternalTrainingAndCompetencyTypes.Count > 0)
                            {
                                var finalData = technicalSpecialistInternalTrainingAndCompetencyTypes.Where(x => x.TypeId != null);
                                var TypeIDs = finalData.Select(x => x.TypeId).ToList();
                                var dbDatas = _dbContext.Data.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.InternalTraining) && TypeIDs.Contains(x.Id)).ToList(); 
                                _technicalSpecialistTrainingAndCompetencyTypeRepository.AutoSave = false;
                                var mappedRecords2 = _mapper.Map<IList<DbModel.TechnicalSpecialistTrainingAndCompetencyType>>(technicalSpecialistInternalTrainingAndCompetencyTypes, opt =>
                                {
                                    opt.Items["isAssignId"] = false;
                                    opt.Items["DBMasterTrainingAndCompetency"] = dbDatas;
                                });

                                _technicalSpecialistTrainingAndCompetencyTypeRepository.Add(mappedRecords2);
                            }
                            var finaltypeCount = _technicalSpecialistTrainingAndCompetencyTypeRepository.ForceSave();
                            Console.WriteLine("Internal Traning Type Table Inserted Successfully");
                            //Training Type  Save END
                            if (finaltypeCount > 0)
                            {
                                var dbTsIntTrainingAndCompetency = _dbContext.TechnicalSpecialistTrainingAndCompetency.Where(x => dbtsEpin.Contains(x.TechnicalSpecialistId)).ToList();
                                var dbTsCompetencyTrainingType = _dbContext.TechnicalSpecialistTrainingAndCompetencyType;
                                IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsTrainingandCompetnecy = new List<DbModel.TechnicalSpecialistTrainingAndCompetency>();
                                IList<DbModel.TechnicalSpecialistTrainingAndCompetency> dbTsTrainingandCompetnecyUpdate = new List<DbModel.TechnicalSpecialistTrainingAndCompetency>();

                                if (dbTsIntTrainingAndCompetency.Count > 0)
                                {
                                    List<TechnicalSpecialistCompetency> finalResult = new List<TechnicalSpecialistCompetency>();

                                    var Joined = dbTsIntTrainingAndCompetency.Join(dbTsCompetencyTrainingType,
                                               CM => new { CM.Id },
                                               CT => new { Id = CT.TechnicalSpecialistTrainingAndCompetencyId },
                                                (CM, CT) => new { CM, CT }).Where(x => x.CM.Id == x.CT.TechnicalSpecialistTrainingAndCompetencyId)
                                                .Select(a => new {
                                                    dataId = a.CT.TrainingOrCompetencyDataId,
                                                    competencyId = a.CT.TechnicalSpecialistTrainingAndCompetencyId,
                                                    tsId = a.CM.TechnicalSpecialistId,
                                                    trainingDate = a.CM.TrainingDate,
                                                }).ToList();

                                    var Result = finalRecordToBeAdded.Join(Joined,
                                        Ad => new { Id = Ad.ILearnID },
                                        Jo => new { Id = Jo.dataId },
                                        (Ad, Jo) => new { Ad, Jo }).Where(y => y.Ad.ILearnID == y.Jo.dataId && y.Ad.Epin == y.Jo.tsId && y.Ad.TrainingDate > y.Jo.trainingDate)
                                        .Select(x => new {
                                            trainingDate = x.Ad.TrainingDate,
                                            x.Jo.competencyId,
                                        }).ToList();

                                    if (Result.Count() > 0)
                                    {
                                        Result.Distinct().ToList().ForEach(comp =>
                                        {
                                            var modify = dbTsIntTrainingAndCompetency.Where(x => x.Id == comp.competencyId).FirstOrDefault();
                                            if (modify != null)
                                            {
                                                modify.Expiry = comp.trainingDate;
                                                dbTsTrainingandCompetnecy.Add(modify);
                                            }
                                        });
                                    }
                                    dbTsTrainingandCompetnecyUpdate = dbTsTrainingandCompetnecy.Distinct().ToList();
                                    _dbContext.BulkUpdate(dbTsTrainingandCompetnecyUpdate);
                                    _dbContext.SaveChanges();
                                    Console.WriteLine("Internal Table Table Updated Successfully");
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            finally
            {
                _technicalSpecialistTrainingAndCompetencyRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception, count);
        }

        public Response AddCertificateData()
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            IList<TechnicalSpecialistCertification> recordToBeAdd = null;
            int count = 0;
            try
            {
                _technicalSpecialistCertificationAndTrainingRepository.AutoSave = false;
                recordToBeAdd = _learnRespositoryInterface.AddCertificateData();
                if (recordToBeAdd.Count > 0)
                {
                    IList<int> tsEpin = recordToBeAdd.Select(x => x.Epin).ToList();// Get the ILearn Epins
                    var dbTsInfo = _dbContext.TechnicalSpecialist.Where(x => tsEpin.Contains(x.Pin)).ToList();//(check the ILearn epins are Valid or Not)
                    IList<int> dbtsEpin = dbTsInfo.Select(x => x.Pin).ToList();// Get all DB epins based on Recordto  be added
                    var finalRecordToBeAdded = recordToBeAdd.Where(x => dbtsEpin.Contains(x.Epin)).ToList();// ONly add valid epins
                    var dbCertificateData = finalRecordToBeAdded.Select(x => x.ILearnID).ToList();
                    var dbMasterCertificateIds = _dbContext.Data.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Certifications) && dbCertificateData.Contains(x.Id))?.ToList();
                    if (finalRecordToBeAdded.Count > 0)
                    {
                        var dbRecordToBeInserted = _mapper.Map<IList<DbModel.TechnicalSpecialistCertificationAndTraining>>(finalRecordToBeAdded, opt =>
                        {
                            opt.Items["isAssignId"] = false;
                            opt.Items["DbTechSpecialist"] = dbTsInfo;
                            opt.Items["DbTrainingTypes"] = dbMasterCertificateIds;
                        });
                        dbRecordToBeInserted = dbRecordToBeInserted?.Where(x2 => x2.TechnicalSpecialistId != 0).ToList();
                        dbRecordToBeInserted?.ToList().ForEach(x =>
                        {
                            x.IsIlearn = true;
                        });
                        _technicalSpecialistCertificationAndTrainingRepository.Add(dbRecordToBeInserted);//Save Internal Training Data
                        var savedCnt = _technicalSpecialistCertificationAndTrainingRepository.ForceSave();
                        Console.WriteLine("Certification Table Inserted Successfully");
                        if (savedCnt > 0)
                        {
                            List<KeyValuePair<string, object>> updateValueProps = new List<KeyValuePair<string, object>>();
                            List<KeyValuePair<DbModel.TechnicalSpecialistCertificationAndTraining, List<KeyValuePair<string, object>>>> tsCerificateupdate = new List<KeyValuePair<DbModel.TechnicalSpecialistCertificationAndTraining, List<KeyValuePair<string, object>>>>();
                            var dbTsCertification = _dbContext.TechnicalSpecialistCertificationAndTraining.Where(x => dbtsEpin.Contains(x.TechnicalSpecialistId)).ToList();
                            if (dbTsCertification.Count > 0)
                            {
                                finalRecordToBeAdded.ToList().ForEach(tsCertificate =>
                                {
                                    var recordModify = dbTsCertification.FirstOrDefault(x => x.TechnicalSpecialistId == tsCertificate.Epin && x.CertificationAndTrainingId == tsCertificate.ILearnID && x.EffeciveDate < tsCertificate.EffeciveDate);
                                    if (recordModify != null)
                                    {
                                         updateValueProps = new List<KeyValuePair<string, object>> {
                                            new KeyValuePair<string, object>("ExpiryDate", tsCertificate.EffeciveDate)
                                           };
                                        tsCerificateupdate.Add(new KeyValuePair<DbModel.TechnicalSpecialistCertificationAndTraining, List<KeyValuePair<string, object>>>(recordModify, updateValueProps));
                                    }
                                });
                                _technicalSpecialistCertificationAndTrainingRepository.Update(tsCerificateupdate, a => a.ExpiryDate);
                                _technicalSpecialistCertificationAndTrainingRepository.ForceSave();
                                Console.WriteLine("Certification Table Updated Successfully");

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            finally
            {
                _technicalSpecialistCertificationAndTrainingRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception, count);
        }

        public Response AddTrainingData()
        {
            Exception exception = null;
            List<ValidationMessage> validationMessages = null;
            IList<TechnicalSpecialistTraining> recordToBeAdd = null;
            int count = 0;
            try
            {
                _technicalSpecialistCertificationAndTrainingRepository.AutoSave = false;
                recordToBeAdd = _learnRespositoryInterface.AddTrainingData();
                if (recordToBeAdd.Count > 0)
                {
                    IList<int> tsEpin = recordToBeAdd.Select(x => x.Epin).ToList();// Get the ILearn Epins
                    var dbTsInfo = _dbContext.TechnicalSpecialist.Where(x => tsEpin.Contains(x.Pin)).ToList();//(check the ILearn epins are Valid or Not)
                    IList<int> dbtsEpin = dbTsInfo.Select(x => x.Pin).ToList();// Get all DB epins based on Recordto  be added
                    var finalRecordToBeAdded = recordToBeAdd.Where(x => dbtsEpin.Contains(x.Epin)).ToList();// ONly add valid epins
                    var dbTrainingData = finalRecordToBeAdded.Select(x => x.ILearnID).ToList();
                    IList<DbModel.User> dbCertVarifiedByUser = _dbContext.User.Where(x => x.SamaccountName == "iLearn").ToList();
                    var dbMasterTrainingIds = _dbContext.Data.Where(x => x.MasterDataTypeId == Convert.ToInt32(MasterType.Trainings) && dbTrainingData.Contains(x.Id))?.ToList();
                    if (finalRecordToBeAdded.Count > 0)
                    {
                        var dbRecordToBeInserted = _mapper.Map<IList<DbModel.TechnicalSpecialistCertificationAndTraining>>(finalRecordToBeAdded, opt =>
                        {
                            opt.Items["isAssignId"] = false;
                            opt.Items["DbTechSpecialist"] = dbTsInfo;
                            opt.Items["DbTrainingTypes"] = dbMasterTrainingIds;
                            opt.Items["DbVarifiedByUser"] = dbCertVarifiedByUser;
                        });
                        dbRecordToBeInserted = dbRecordToBeInserted?.Where(x2 => x2.TechnicalSpecialistId != 0).ToList();
                        dbRecordToBeInserted?.ToList().ForEach(x =>
                        {
                            x.IsIlearn = true;
                        });
                        _technicalSpecialistCertificationAndTrainingRepository.Add(dbRecordToBeInserted);//Save Internal Training Data
                        var savedCnt = _technicalSpecialistCertificationAndTrainingRepository.ForceSave();
                        Console.WriteLine("Training Table Inserted Successfully");
                        if (savedCnt > 0)
                        {
                            List<KeyValuePair<string, object>> updateValueProps = new List<KeyValuePair<string, object>>();
                            List<KeyValuePair<DbModel.TechnicalSpecialistCertificationAndTraining, List<KeyValuePair<string, object>>>> tsTrainingupdate = new List<KeyValuePair<DbModel.TechnicalSpecialistCertificationAndTraining, List<KeyValuePair<string, object>>>>();
                            var dbTsCertification = _dbContext.TechnicalSpecialistCertificationAndTraining.Where(x => dbtsEpin.Contains(x.TechnicalSpecialistId)).ToList();
                            if (dbTsCertification.Count > 0)
                            {
                                finalRecordToBeAdded.ToList().ForEach(tsTraining =>
                                {
                                    var recordModify = dbTsCertification.FirstOrDefault(x => x.TechnicalSpecialistId == tsTraining.Epin && x.CertificationAndTrainingId == tsTraining.ILearnID && x.EffeciveDate < tsTraining.EffeciveDate);
                                    if (recordModify != null)
                                    {
                                        updateValueProps = new List<KeyValuePair<string, object>> {
                                            new KeyValuePair<string, object>("ExpiryDate", tsTraining.EffeciveDate)
                                           };
                                        tsTrainingupdate.Add(new KeyValuePair<DbModel.TechnicalSpecialistCertificationAndTraining, List<KeyValuePair<string, object>>>(recordModify, updateValueProps));
                                    }
                                });
                                _technicalSpecialistCertificationAndTrainingRepository.Update(tsTrainingupdate, a => a.ExpiryDate);
                                _technicalSpecialistCertificationAndTrainingRepository.ForceSave();
                                Console.WriteLine("Training Table Updated Successfully");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ResponseType.Exception.ToId(), ex.ToFullString(), null);
            }
            finally
            {
                _technicalSpecialistCertificationAndTrainingRepository.AutoSave = true;
            }

            return new Response().ToPopulate(ResponseType.Success, null, null, validationMessages?.ToList(), null, exception, count);
        }
   }
}
