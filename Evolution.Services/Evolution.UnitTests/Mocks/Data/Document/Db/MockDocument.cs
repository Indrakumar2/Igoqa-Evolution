using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbModel =Evolution.DbRepository.Models;

namespace Evolution.UnitTests.Mocks.Data.Document.Db
{
    public static class MockDocument
    {
        public static IQueryable<DbModel.Document> GetProjectDocumentsMockData()
        {
            return new List<DbModel.Document>()
            {
                new DbModel.Document()
                {
                      Id=1,DocumentName="Document",DocumentType="Customer Report Forms",
                    IsVisibleToCustomer=false,IsVisibleToOutsideOfCompany=false,IsVisibleToTechSpecialist=false,CreatedDate=DateTime.UtcNow,
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,Status="C",DocumentUniqueName="PROJ-18-12-2018-bf9dd31c-2530-495e-a8f9-8872157c5df8.temp",
                    ModuleCode="PROJ",ModuleRefCode="1",CreatedBy="Indu",Size=100


                },
                new DbModel.Document()
                {
                     Id=2,DocumentName="Document",DocumentType="Evolution Email",
                    IsVisibleToCustomer=false,IsVisibleToOutsideOfCompany=false,IsVisibleToTechSpecialist=false,CreatedDate=DateTime.UtcNow,
                    LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,Status="CR",DocumentUniqueName="PROJ-18-12-2018-9cb18148-b3d2-4eb7-b144-c4c039d6a874.temp",
                    ModuleCode="PROJ",ModuleRefCode="1",CreatedBy="Indu.N",Size=120

                }
            }.AsQueryable();
        }
    }
}
