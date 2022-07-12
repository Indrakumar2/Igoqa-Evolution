using Evolution.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.Master.Domain.Interfaces.Services
{
    public interface ILanguageReferenceTypeService
    {
       

         Response Save(IList<Models.LanguageReferenceType> datas);

        Response Search(Models.LanguageReferenceType search);

        Response Modify(IList<Models.LanguageReferenceType> datas);

        Response Delete(IList<Models.LanguageReferenceType> datas);

    }
}
