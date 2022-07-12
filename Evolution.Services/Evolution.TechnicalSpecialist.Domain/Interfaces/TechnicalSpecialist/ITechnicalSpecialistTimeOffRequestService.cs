using Evolution.Common.Enums;
using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using DbModel = Evolution.DbRepository.Models.SqlDatabaseContext;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
   public interface ITechnicalSpecialistTimeOffRequestService
    {

        Response Get(DomainModel.TechnicalSpecialistTimeOffRequest searchModel);

        Response Get(IList<long> ptoIds,string[] includes);

        Response Add(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, bool commitChange = true, bool isDbValidationRequired = true);

        Response Add(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, ref IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest,
                     ref IList<DbModel.TechnicalSpecialist> dbTechnicalSpecialists,ref long? eventId, ref IList<DbModel.Data> dbLeaveCategroryType, bool commitChange = true, bool isDbValidationRequired = true);
        Response IsRecordValidForProcess(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, ValidationType validationType);

        Response IsRecordValidForProcess(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, 
                                         ValidationType validationType, ref IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest, 
                                         ref IList<DbModel.TechnicalSpecialist> technicalSpecialists,ref IList<DbModel.Data> dbLeaveCategroryType);

        Response IsRecordValidForProcess(IList<DomainModel.TechnicalSpecialistTimeOffRequest> technicalSpecialistTimeOffRequest, ValidationType validationType, 
                                         IList<DbModel.TechnicalSpecialistTimeOffRequest> dbTechnicalSpecialistTimeOffRequest,
                                         IList<DbModel.TechnicalSpecialist> technicalSpecialists, IList<DbModel.Data> dbLeaveCategroryType);
    }
}
