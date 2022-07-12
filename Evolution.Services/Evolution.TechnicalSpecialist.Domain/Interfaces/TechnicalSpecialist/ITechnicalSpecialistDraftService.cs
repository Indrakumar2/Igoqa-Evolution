using System;
using System.Collections.Generic;
using System.Text;
using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using DomainModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistDraftService
    {
        Response SaveDraft(string jsontext, string AssignedByUser, bool commitChange = true);

        Response ModifyDraft(string jsontext, string technicalSpecialistDerafId,bool commitChange = true);

        Response ModifyListOfDrafts(IList<DomainModel.TechnicalSpecialistDraft> technicalSpecialistDrafts, bool commitChange = true);

        Response DeleteDraft(string technicalSpecialistDrafId, bool commitChange = true);

        Response GetDraft(TechnicalSpecialistDraft searchModel);
       
    }
}
