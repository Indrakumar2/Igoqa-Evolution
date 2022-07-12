using Evolution.GenericDbRepository.Interfaces;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;
using System;
using System.Collections.Generic;
using DomainModel = Evolution.ILearn.Domain.Models;
using Evolution.Common.Models.Responses;
using TechDomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.ILearn.Domain.Interfaces
{
    public interface ILearnRespositoryInterface : IGenericRepository<DbModel.IlearnData>, IDisposable
    {
        IList<DbModel.IlearnData> Save(DomainModel.LearnData learnData);

        List<TechDomainModel.TechnicalSpecialistCompetency> AddCompantencyData();

        List<TechDomainModel.TechnicalSpecialistInternalTraining> AddInternalTrainingData();

        List<TechDomainModel.TechnicalSpecialistCertification> AddCertificateData();

        List<TechDomainModel.TechnicalSpecialistTraining> AddTrainingData();

    }
}
