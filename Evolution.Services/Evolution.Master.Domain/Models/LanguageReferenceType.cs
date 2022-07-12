using Evolution.Common.Models.Base;

namespace Evolution.Master.Domain.Models
{
    public  class LanguageReferenceType:BaseModel
    {
         public int? LanguageReferenceTypeId {get; set;}

         public string ReferenceType {get; set;}

         public string Language {get; set;}

         public string Text {get; set;}


          

    }
}