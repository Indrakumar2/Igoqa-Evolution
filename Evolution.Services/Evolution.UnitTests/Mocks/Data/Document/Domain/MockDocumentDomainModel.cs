using Evolution.Common.Enums;
using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;

namespace Evolution.UnitTests.Mocks.Data.Document.Domain
{
   public static class MockDocumentDomainModel
    {
        public static IList<ModuleDocument> GetDocumentsMockedDomainData()
        {

            return new List<ModuleDocument>()
            {
                new ModuleDocument()
                {

                    Id=1,DocumentName="Document",DocumentType="Evolution Email",
                    IsVisibleToCustomer=false,IsVisibleToTS=false,DocumentSize=100,Status="CR",
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,DocumentUniqueName="PROJ-18-12-2018-bf9dd31c-2530-495e-a8f9-8872157c5df8.temp",
                    CreatedBy="Indu",CreatedOn=DateTime.UtcNow,IsVisibleOutOfCompany=true,ModuleCode="PROJ",ModuleRefCode="1"

                },

                new ModuleDocument()
                {

                    Id=2,DocumentName="Project",DocumentType="Email",
                    IsVisibleToCustomer=false,IsVisibleToTS=false,DocumentSize=100,Status="C",
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,DocumentUniqueName="PROJ-17-12-2018-c6e9bd13-448d-4c34-93a7-2f06ab1ac162.temp",
                      CreatedBy="Indu",CreatedOn=DateTime.UtcNow,IsVisibleOutOfCompany=true,ModuleCode="PROJ",ModuleRefCode="1"

                }
            };
        }
        public static IList<DocumentUniqueNameDetail> GetUniqueNameMockedDomainData()
        {
            return new List<DocumentUniqueNameDetail>()
            {
                new DocumentUniqueNameDetail()
                {
                    ModuleCode="PRJ",ModuleCodeReference="1",DocumentName="Project related Data",RequestedBy="Ganga",UniqueName="Unique-Name",
                    Status="CR"

                }
            };
        }

    }
}
