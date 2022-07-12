using Evolution.Common.Models.Responses;
using Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.TechnicalSpecialist.Domain.Interfaces.TechnicalSpecialist
{
    public interface ITechnicalSpecialistSensitiveDocumentService
    {
        //Response SaveSensitiveDocuments(string ePin, IList<TechnicalSpecialistSensitiveDocument> sensitiveDocuments, bool commitChange = true);

        //Response ModifySensitiveDocuments(string ePin, IList<TechnicalSpecialistSensitiveDocument> sensitiveDocuments, bool commitChange = true);

        //Response DeleteSensitiveDocuments(string ePin, IList<TechnicalSpecialistSensitiveDocument> sensitiveDocument, bool commitChange = true);

        Response GetSensitiveDocuments(TechnicalSpecialistSensitiveDocument searchModel);
    }
}
