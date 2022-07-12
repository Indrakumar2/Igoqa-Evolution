using System.Collections.Generic;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;


namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistCertificationAndTrainingService
    {
        
        Response GetTechnicalSpecialistTraining(TechnicalSpecialistTraining searchModel,string recordType);
        Response SaveTechnicalSpecialistTraining(int ePin, IList<TechnicalSpecialistTraining> technicalSpecialistTraining, string recordType, bool commitChange = true, bool isResultSetRequired = false);
        Response ModifyTechnicalSpecialistTraining(int ePin, IList<TechnicalSpecialistTraining> technicalSpecialistTraining, string recordType, bool commitChange = true, bool isResultSetRequired = false);
        Response DeleteTechnicalSpecialistTraining(int ePin, IList<TechnicalSpecialistTraining> technicalSpecialistTraining, string recordType, bool commitChange = true, bool isResultSetRequired = false);

        Response GetTechnicalSpecialistCertification(TechnicalSpecialistCertification searchModel, string recordType);
        Response SaveTechnicalSpecialistCertification(int ePin, IList<TechnicalSpecialistCertification> technicalSpecialistCertifications, string recordType, bool commitChange = true, bool isResultSetRequired = false);
        Response ModifyTechnicalSpecialistCertification(int ePin, IList<TechnicalSpecialistCertification> technicalSpecialistCertifications, string recordType, bool commitChange = true, bool isResultSetRequired = false);
        Response DeleteTechnicalSpecialistCertification(int ePin, IList<TechnicalSpecialistCertification> technicalSpecialistCertifications, string recordType, bool commitChange = true, bool isResultSetRequired = false);


    }
}
