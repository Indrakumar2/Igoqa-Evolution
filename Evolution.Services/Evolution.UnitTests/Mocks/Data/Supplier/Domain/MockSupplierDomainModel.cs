using Evolution.Document.Domain.Models.Document;
using System;
using System.Collections.Generic;
using System.Text;
using DomModel = Evolution.Supplier.Domain.Models.Supplier;

namespace Evolution.UnitTests.Mocks.Data.Supplier.Domain
{
    public static class MockSupplierDomainModel
    {
        #region supplier
        public static IList<DomModel.Supplier> GetSupplierDomModel()
        {
            return new List<DomModel.Supplier>()
            {
                new DomModel.Supplier()
                {
                     SupplierId=1,SupplierName="CAMERON FRANCE",SupplierAddress="CAMERON FRANCE",City="Brighton",State="karnataka",
                       PostalCode ="00123",Country="United Kingdom",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null
                },


                new DomModel.Supplier()
                {
                     SupplierId=2,SupplierName="Dresser Produits Industriels",SupplierAddress="Dresser Produits Industriels ,Italy",City="Houston",
                       PostalCode ="001234",Country="United Kingdom",LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,State="Delhi"
                }
           };
           
        }
        #endregion

        #region supplierNote
        public static IList<DomModel.SupplierNote> GetSupplierNoteDomModel()
        {
            return new List<DomModel.SupplierNote>()
            {
                new DomModel.SupplierNote()
                {
                    SupplierNoteId=1,SupplierId=1,CreatedOn=DateTime.UtcNow,SupplierName="Elmar UK",
                    CreatedBy="M.Peacock",Notes="Notes",LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0
                },


                new DomModel.SupplierNote()
                {
                     SupplierNoteId=2,SupplierId=2,CreatedOn=DateTime.UtcNow,SupplierName="Stanref Int. Plc.",
                    CreatedBy="M.Peacock",Notes="Notes",LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0
                }
           };

        }
        #endregion



        #region supplierContact
        public static IList<DomModel.SupplierContact> GetSupplierContactDomModel()
        {
            return new List<DomModel.SupplierContact>()
            {
                new DomModel.SupplierContact()
                {
                    SupplierContactId=1,SupplierId=1,SupplierContactName="John Cleary",SupplierTelephoneNumber="0132456",SupplierFaxNumber="12345",SupplierMobileNumber="123456",
                    SupplierEmail="Jean-Luc.Llorca@c-a-m.com",OtherContactDetails=null,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,SupplierName="CAMERON FRANCE"
                },


                new DomModel.SupplierContact()
                {
                      SupplierContactId=2,SupplierId=2,SupplierContactName="Sofia Redford",SupplierTelephoneNumber="0132456",SupplierFaxNumber="12345",SupplierMobileNumber="123456",
                    SupplierEmail="john.cleary@lff.co.uk",OtherContactDetails=null,LastModification=DateTime.UtcNow,UpdateCount=0,ModifiedBy=null,SupplierName="CAMERON FRANCE NEW"
                }
           };

        }
        #endregion
        #region supplierDocument
        public static IList<ModuleDocument> GetSupplierDocumentDomModel()
        {
            return new List<ModuleDocument>()
            {
                new ModuleDocument()
                {
                    Id=1,DocumentName="Document",DocumentType="Email",DocumentSize=100,IsVisibleToCustomer=true,
                   IsVisibleToTS=true,LastModification=DateTime.UtcNow,UpdateCount=0,
                    ModifiedBy=null,
                },


                new ModuleDocument()
                {
                     Id=2,DocumentName="Document Report",DocumentType="Genral",DocumentSize=100,IsVisibleToCustomer=true,
                   IsVisibleToTS=true,LastModification=DateTime.UtcNow,UpdateCount=0,
                    ModifiedBy=null,
                }
           };

        }
        #endregion
    
    }
}
