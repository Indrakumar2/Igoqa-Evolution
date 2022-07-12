using System;
using System.Collections.Generic;
using System.Linq;
using DbModel = Evolution.DbRepository.Models;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace Evolution.UnitTests.Mocks.Data.TechnicalSpecialist.Db
{
    public class MockTechnicalSpecialist
    {
        #region Language Capability
        public static IQueryable<DbModel.TechnicalSpecialistLanguageCapability> GetLanguageCapabilityMockData()
        {
            return new List<DbModel.TechnicalSpecialistLanguageCapability>()
            {
                new DbModel.TechnicalSpecialistLanguageCapability()
                {
                   Id=1,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54},LanguageId=3795, Language=new DbModel.Data(){Id=3795,Name="BENGALI",MasterDataTypeId=53} , SpeakingCapabilityLevel="Good", ComprehensionCapabilityLevel="Normal", WritingCapabilityLevel="Poor",
                   DispalyOrder=1, LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },

                 new DbModel.TechnicalSpecialistLanguageCapability()
                {
                   Id=6,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54},LanguageId=3795,Language =new DbModel.Data(){Id=3795,Name="BENGALI",MasterDataTypeId=53}, SpeakingCapabilityLevel="Very Good", ComprehensionCapabilityLevel="Normal", WritingCapabilityLevel="Avrage",
                   DispalyOrder=1, LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                }
            }.AsQueryable();

        }

        #endregion

        #region StampInfo

        public static IQueryable<DbModel.TechnicalSpecialistStamp> GetTechnicalSpecialistStampMockData()
        {
            return new List<DbModel.TechnicalSpecialistStamp>()
            {
                new DbModel.TechnicalSpecialistStamp()
                {
                   Id=1,TechnicalSpecialistId=54,IsSoftStamp =true,CountryId=1, Country=new DbModel.Country{ Id=1,Code ="UK",Name="United Kingdom"}, IssuedDate=DateTime.UtcNow, ReturnDate=DateTime.UtcNow,
                   DisplayOrder=1, LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,StampNumber="83432984"

                },

                 new DbModel.TechnicalSpecialistStamp()
                {
                   Id=2,TechnicalSpecialistId=54,IsSoftStamp =false,CountryId=1, Country=new DbModel.Country{ Id=1,Code ="UK",Name="United Kingdom"}, IssuedDate=DateTime.UtcNow, ReturnDate=DateTime.UtcNow,
                   DisplayOrder=2, LastModification=DateTime.UtcNow,ModifiedBy="Sh",UpdateCount=0,StampNumber="83432984"

                },
            }.AsQueryable();

        }

        public static Mock<DbSet<DbModel.TechnicalSpecialistStamp>> GetTechnicalSpecialistStampDbSet(IQueryable<DbModel.TechnicalSpecialistStamp> data)
        {
            var mockSet = new Mock<DbSet<DbModel.TechnicalSpecialistStamp>>();
            mockSet.As<IQueryable<DbModel.TechnicalSpecialistStamp>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<DbModel.TechnicalSpecialistStamp>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<DbModel.TechnicalSpecialistStamp>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<DbModel.TechnicalSpecialistStamp>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        #region TechnicalSpecialist
        public static IQueryable<DbModel.TechnicalSpecialist> GetTechnicalSpecialistMockData()
        {
            return new List<DbModel.TechnicalSpecialist>()
            {
                new DbModel.TechnicalSpecialist()
                {
                   Id=54, Pin= 54,
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },

                 new DbModel.TechnicalSpecialist()
                {
                   Id=54, Pin= 54,
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                }
            }.AsQueryable();
        }
        #endregion
        #region WorkHistory
        public static IQueryable<DbModel.TechnicalSpecialistWorkHistory> GetWorkHistoryMockData()
        {
            return new List<DbModel.TechnicalSpecialistWorkHistory>()
            {
                new DbModel.TechnicalSpecialistWorkHistory()
                {
                   Id=9,TechnicalSpecialistId=54,  ClientName ="test", ProjectName="GRM", JobTitle="SSE", IsCurrentCompany=true,
                   Responsibility="Responsibility", Description="test123",DisplayOrder=1,DateFrom=DateTime.UtcNow,DateTo=DateTime.UtcNow, LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

                },

                 new DbModel.TechnicalSpecialistWorkHistory()
                {
                   Id=10,TechnicalSpecialistId=54,  ClientName ="test", ProjectName="GRM", JobTitle="SSE", IsCurrentCompany=true,
                   Responsibility="Responsibility", Description="test123",DisplayOrder=1,DateFrom=DateTime.UtcNow,DateTo=DateTime.UtcNow, LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,
     }
            }.AsQueryable();

        }

        #endregion

        #endregion

        //#region ComputerElectronicKnowledge
        //public static IQueryable<DbModel.TechnicalSpecialistComputerElectronicKnowledge> GetComputerElectronicKnowledgeMockData()
        //{
        //    return new List<DbModel.TechnicalSpecialistComputerElectronicKnowledge>()
        //    return new List<DbModel.TechnicalSpecialistCodeAndStandard>()
        //    {
        //        new DbModel.TechnicalSpecialistComputerElectronicKnowledge()
        //        new DbModel.TechnicalSpecialistCodeAndStandard()
        //        {
        //           Id=1,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54},ComputerKnowledgeId=3767, ComputerKnowledge=new DbModel.Data(){Id=3767,Name="Adobe Flash",MasterDataTypeId=48} ,
        //           LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
        //           Id=1,TechnicalSpecialistId=54,CodeStandard=new DbModel.Data{Id=3765,Code="1",Name="ACI"},
        //          LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

        //        },

        //         new DbModel.TechnicalSpecialistComputerElectronicKnowledge()
        //        {
        //           Id=1,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54},ComputerKnowledgeId=3767, ComputerKnowledge=new DbModel.Data(){Id=3767,Name="Adobe Flash",MasterDataTypeId=48} ,
        //           LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
        //        }
        //            Id=3,TechnicalSpecialistId=54,CodeStandard=new DbModel.Data{Id=3765,Code="1",Name="ACI"},
        //          LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,     }
        //    }.AsQueryable();

        //}

        //#endregion

        #region TechnicalSpecialistPaySchedule
        public static IQueryable<DbModel.TechnicalSpecialistPaySchedule> GetTechnicalSpecialistPayScheduleMockData()
        {
            return new List<DbModel.TechnicalSpecialistPaySchedule>()
            {
                new DbModel.TechnicalSpecialistPaySchedule()
                {
                  Id=1,TechnicalSpecialistId=54,PayScheduleName="PaySchedule", PayCurrency="AED", PayScheduleNote="Test",IsActive=true,DisplayOrder=1,
                  LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

                },

                 new DbModel.TechnicalSpecialistPaySchedule()
                {
                    Id=2,TechnicalSpecialistId=54,PayScheduleName="New Record", PayCurrency="ALL", PayScheduleNote="Test PaySchedule",IsActive=true,DisplayOrder=1,
                  LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,
    }
            }.AsQueryable();

        }

        #endregion

        #region TechnicalSpecialistCodeAndStandard
        public static IQueryable<DbModel.TechnicalSpecialistCodeAndStandard> GetTechnicalSpecialistCodeAndStandardMockData()
        {
            return new List<DbModel.TechnicalSpecialistCodeAndStandard>()
            {
                new DbModel.TechnicalSpecialistCodeAndStandard()
                {
                   Id=1,TechnicalSpecialistId=54,CodeStandard=new DbModel.Data{Id=3765,Code="1",Name="ACI"},
                  LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

                },

                 new DbModel.TechnicalSpecialistCodeAndStandard()
                {
                    Id=3,TechnicalSpecialistId=54,CodeStandard=new DbModel.Data{Id=3765,Code="1",Name="ACI"},
                  LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,     }
            }.AsQueryable();

        }

        #endregion

        #region ComputerElectronicKnowledge
        public static IQueryable<DbModel.TechnicalSpecialistComputerElectronicKnowledge> GetComputerElectronicKnowledgeMockData()
        {
            return new List<DbModel.TechnicalSpecialistComputerElectronicKnowledge>()
            {
                new DbModel.TechnicalSpecialistComputerElectronicKnowledge()
                {
                   Id=1,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54},ComputerKnowledgeId=3767, ComputerKnowledge=new DbModel.Data(){Id=3767,Name="Adobe Flash",MasterDataTypeId=48} ,
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },

                 new DbModel.TechnicalSpecialistComputerElectronicKnowledge()
                {
                   Id=1,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54},ComputerKnowledgeId=3767, ComputerKnowledge=new DbModel.Data(){Id=3767,Name="Adobe Flash",MasterDataTypeId=48} ,
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                }
            }.AsQueryable();

        }

        #endregion

        //#region Competency
        //public static IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> GetCompetencyMockData()
        //{
        //    return new List<DbModel.TechnicalSpecialistTrainingAndCompetency>()
        //    {
        //        new DbModel.TechnicalSpecialistTrainingAndCompetency()
        //        {
        //            Id=1, TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54}, Competency="",Score=null,DisplayOrder=1,Duration="",EffectiveDate= DateTime.UtcNow,ExpiryDate=DateTime.UtcNow,
        //           Name="",Notes="",                

        //        },
        //         new DbModel.TechnicalSpecialistTrainingAndCompetency()
        //        {
        //           Id=1,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54},ComputerKnowledgeId=3767, ComputerKnowledge=new DbModel.Data(){Id=3767,Name="Adobe Flash",MasterDataTypeId=48} ,
        //           LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
        //        }
        //    }.AsQueryable();

        //}

        //#endregion

        #region EducationalQualification
        public static IQueryable<DbModel.TechnicalSpecialistEducationalQualification> GetEducationalQualificationMockData()
        {
            return new List<DbModel.TechnicalSpecialistEducationalQualification>()
            {
                new DbModel.TechnicalSpecialistEducationalQualification()
                {
                   Id=1,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54}, Address="",
                   CountryId=1, Country =new DbModel.Country(){Id=1,Name="United Kingdom"} ,
                   CountyId =20, County =new DbModel.County(){Id=20,Name="Andorra la Vella"} ,
                   CityId=5, City =new DbModel.City(){Id=5,Name="Brighton"} ,
                   Institution ="HBTI",Percentage=90,DisplayOrder=1, Place="Kanpur",Qualification="MCA",
                   DateFrom= DateTime.UtcNow,DateTo=DateTime.UtcNow,
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },

                 new DbModel.TechnicalSpecialistEducationalQualification()
                {
                   Id=2,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54}, Address="",
                   CountryId=1, Country =new DbModel.Country(){Id=1,Name="United Kingdom"} ,
                   CountyId =20, County =new DbModel.County(){Id=20,Name="Andorra la Vella"} ,
                   CityId=5, City =new DbModel.City(){Id=5,Name="Brighton"} ,
                   Institution ="HBTI",Percentage=90,DisplayOrder=1, Place="Kanpur",Qualification="MCA",
                   DateFrom= DateTime.UtcNow,DateTo=DateTime.UtcNow,
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                }
            }.AsQueryable();

        }

        #endregion

        #region Contact Information
        public static IQueryable<DbModel.TechnicalSpecialistContact> GetContactMockData()
        {
            return new List<DbModel.TechnicalSpecialistContact>()
            {
                new DbModel.TechnicalSpecialistContact()
                {
                   Id=1,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54}, Address="",
                   CountryId=1, Country =new DbModel.Country(){Id=1,Name="United Kingdom"} ,
                   CountyId =20, County =new DbModel.County(){Id=20,Name="Andorra la Vella"} ,
                   CityId=5, City =new DbModel.City(){Id=5,Name="Brighton"} ,
                   ContactType="Address",EmailAddress="singh.shru@gmail.com",EmergencyContactName="Ch",FaxNumber="9999999999",
                   IsPrimary=true,MobileNumber="9999999999",PostalCode="560093",TelephoneNumber="9999999999",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },

                 new DbModel.TechnicalSpecialistContact()
                {
                   Id=2,TechnicalSpecialistId=54,TechnicalSpecialist=new DbModel.TechnicalSpecialist(){ Id=1,Pin=54}, Address="",
                   CountryId=1, Country =new DbModel.Country(){Id=1,Name="United Kingdom"} ,
                   CountyId =20, County =new DbModel.County(){Id=20,Name="Andorra la Vella"} ,
                   CityId=5, City =new DbModel.City(){Id=5,Name="Brighton"} ,
                   ContactType="Address",EmailAddress="singh.shru@gmail.com",EmergencyContactName="Ch",FaxNumber="9999999999",
                   IsPrimary=true,MobileNumber="9999999999",PostalCode="560093",TelephoneNumber="9999999999",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                }
            }.AsQueryable();

        }

        #endregion

        //#region TechnicalSpecialistCodeAndStandard
        //public static IQueryable<DbModel.TechnicalSpecialistCodeAndStandard> GetTechnicalSpecialistCodeAndStandardMockData()
        //{
        //    return new List<DbModel.TechnicalSpecialistCodeAndStandard>()
        //   {
        //       new DbModel.TechnicalSpecialistCodeAndStandard()
        //       {
        //          Id=1,TechnicalSpecialistId=54,CodeStandard=new DbModel.Data{Id=3765,Code="1",Name="ACI"},
        //         LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

        //       },

        //        new DbModel.TechnicalSpecialistCodeAndStandard()
        //       {
        //           Id=3,TechnicalSpecialistId=54,CodeStandard=new DbModel.Data{Id=3765,Code="1",Name="ACI"},
        //         LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,     }
        //   }.AsQueryable();

        //}

        //#endregion
        #region TechnicalSpecialistCompetency
        public static IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> GetTechnicalSpecialistCompetencyMockData()
        {
            return new List<DbModel.TechnicalSpecialistTrainingAndCompetency>()
           {
               new DbModel.TechnicalSpecialistTrainingAndCompetency()
               {
                  Id=1,TechnicalSpecialistId=54,  Competency="Competency", DateofTraining=DateTime.UtcNow, DisplayOrder=1,  Duration="2Years", Expiry=DateTime.UtcNow, Notes="Test Note", RecordType="Competency", Score=20,
                 LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

               },

                new DbModel.TechnicalSpecialistTrainingAndCompetency()
               {
                   Id=5,TechnicalSpecialistId=54,  Competency="Competency", DateofTraining=DateTime.UtcNow, DisplayOrder=1,  Duration="2Years", Expiry=DateTime.UtcNow, Notes="Test Note", RecordType="Competency", Score=20,
                 LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,
 }
           }.AsQueryable();

        }

        #endregion
        #region TechnicalSpecialistInternalTraining
        public static IQueryable<DbModel.TechnicalSpecialistTrainingAndCompetency> GetTechnicalSpecialistInternalTrainingMockData()
        {
            return new List<DbModel.TechnicalSpecialistTrainingAndCompetency>()
           {
               new DbModel.TechnicalSpecialistTrainingAndCompetency()
               {
                  Id=3,TechnicalSpecialistId=54,  Competency="InternalTraining", DateofTraining=DateTime.UtcNow, DisplayOrder=1,  Duration="2Years", Expiry=DateTime.UtcNow, Notes="Test Note", RecordType="InternalTraining", Score=20,
                 LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

               },

                new DbModel.TechnicalSpecialistTrainingAndCompetency()
               {
                   Id=4,TechnicalSpecialistId=54,  Competency="InternalTraining", DateofTraining=DateTime.UtcNow, DisplayOrder=1,  Duration="2Years", Expiry=DateTime.UtcNow, Notes="Test Note", RecordType="InternalTraining", Score=20,
                 LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,
 }
           }.AsQueryable();

        }

        #endregion
        #region TechnicalSpecialistCertificate
        public static IQueryable<DbModel.TechnicalSpecialistCertificationAndTraining> GetTechnicalSpecialistCertificationAndTrainingMockData()
        {
            return new List<DbModel.TechnicalSpecialistCertificationAndTraining>()
           {
               new DbModel.TechnicalSpecialistCertificationAndTraining()
               {
                  Id=8,TechnicalSpecialistId=54,Duration="2Years",Notes="Test Note",RecordType="Training",
                 NameId=3770,Name =new DbModel.Data{Id=3770,Code="1",Name="ACA"},
                 VerifiedById=3,  VerifiedBy=new DbModel.User{Id=3,Name="Mark Peacock"},
                   LastModification =DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

               },
                new DbModel.TechnicalSpecialistCertificationAndTraining()
               {

                 Id=11,TechnicalSpecialistId=54,Duration="2Years",Notes="Test Note",RecordType="Training",
                 NameId=3770,Name =new DbModel.Data{Id=3770,Code="1",Name="ACA"},
                 VerifiedById=3,  VerifiedBy=new DbModel.User{Id=3,Name="Mark Peacock"},
                   LastModification =DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,
 }
           }.AsQueryable();

        }

        #endregion


    }
}
