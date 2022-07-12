using System;
using System.Collections.Generic;
using DomModel = Evolution.TechnicalSpecialist.Domain.Models.TechSpecialist;

namespace Evolution.UnitTests.Mocks.Data.TechnicalSpecialist.Domain
{
    public class MockTechnicalSpecialistModel
    {

        #region Language Capability
        public static IList<DomModel.TechnicalSpecialistLanguageCapability> GetLanguageCapabilityMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistLanguageCapability>()
            {
                new DomModel.TechnicalSpecialistLanguageCapability()
                {
                   Id=1, Epin=54,Language ="BENGALI",  SpeakingCapabilityLevel="Good", ComprehensionCapabilityLevel="Normal", WritingCapabilityLevel="Poor",
                   DispalyOrder=1, LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },
                new DomModel.TechnicalSpecialistLanguageCapability()
                {
                   Id=6,  Epin=54,Language ="BENGALI", SpeakingCapabilityLevel="Very Good", ComprehensionCapabilityLevel="Normal", WritingCapabilityLevel="Avrage",
                   DispalyOrder=1, LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                },
            };
        }

        #endregion

        #region WorkHistory
        public static IList<DomModel.TechnicalSpecialistWorkHistory> GetWorkHistoryMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistWorkHistory>()
            {
                 new DomModel.TechnicalSpecialistWorkHistory()
                {
                   Id=9,Epin=54,  ClientName ="test", ProjectName="GRM", JobTitle="SSE", IsCurrentCompany=true,
                   Responsibility="Responsibility", Description="test123",DisplayOrder=1,FromDate=DateTime.UtcNow,ToDate=DateTime.UtcNow, LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

                },

                 new DomModel.TechnicalSpecialistWorkHistory()
                {
                   Id=10,Epin=54,  ClientName ="test", ProjectName="GRM", JobTitle="SSE", IsCurrentCompany=true,
                   Responsibility="Responsibility", Description="test123",DisplayOrder=1,FromDate=DateTime.UtcNow,ToDate=DateTime.UtcNow, LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,
    },
            };
        }
        #endregion

        #region TechnicalSpecialistStamp
        public static IList<DomModel.TechnicalSpecialistStamp> GetTechnicalSpecialistStampMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistStamp>()
            {
                new DomModel.TechnicalSpecialistStamp()
                {
                   Epin=54,IsSoftStamp = true,  CountryCode="UK",Id=1, CountryName="United Kingdom", IssuedDate=DateTime.UtcNow,
                   ReturnedDate=DateTime.UtcNow, DisplayOrder=1, LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,StampNumber="83432984",

                },
                new DomModel.TechnicalSpecialistStamp()
                {
                   Epin=54,IsSoftStamp = false,  CountryCode="UK", CountryName="United Kingdom", IssuedDate=DateTime.UtcNow,
                   ReturnedDate=DateTime.UtcNow, DisplayOrder=1, LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,StampNumber="83432984",
                },
            };
        }
        #endregion

        #region ComputerElectronicKnowledge
        public static IList<DomModel.TechnicalSpecialistComputerElectronicKnowledge> GetComputerElectronicKnowledgeMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistComputerElectronicKnowledge>()
            {
                new DomModel.TechnicalSpecialistComputerElectronicKnowledge()
                {
                   Id=1, Epin=54, ComputerKnowledge ="Adobe Flash",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },
                new DomModel.TechnicalSpecialistComputerElectronicKnowledge()
                {
                   Id=2, Epin=54, ComputerKnowledge ="Adobe Flash",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                },
            };
        }

        #endregion

        #region CodeAndStandard
        public static IList<DomModel.TechnicalSpecialistCodeAndStandard> GetTechnicalSpecialistCodeAndStandardMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistCodeAndStandard>()
            {
                new DomModel.TechnicalSpecialistCodeAndStandard()
                {
                  Id=1,Epin=54,CodeStandardName="ACI",
                  LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,


                },
                new DomModel.TechnicalSpecialistCodeAndStandard()
                {
                  Id=3,Epin=54, CodeStandardName="ACI",
                  LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

    },
            };
        }
        #endregion

        #region PaySchdule
        public static IList<DomModel.TechnicalSpecialistPaySchedule> GetTechnicalSpecialistPaySchduleMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistPaySchedule>()
            {
                new DomModel.TechnicalSpecialistPaySchedule()
                {
                  Id=1,Epin=54,PayScheduleName="PaySchedule", PayCurrency="AED", PayScheduleNote="Test",IsActive=true,DisplayOrder=1,
                  LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,


                },
                new DomModel.TechnicalSpecialistPaySchedule()
                {
                  Id=2,Epin=54,PayScheduleName="PaySchedule", PayCurrency="ALL", PayScheduleNote="Test",IsActive=true,DisplayOrder=1,
                  LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,

    },
            };
        }
        #endregion

        #region Competency
        public static IList<DomModel.TechnicalSpecialistCompetency> GetCompetencyMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistCompetency>()
            {
                new DomModel.TechnicalSpecialistCompetency()
                {
                   ID=1, Epin=54, Competency="",Detail="",DisplayOrder=1,Duration="",EffectiveDate= DateTime.UtcNow,ExpiryDate=DateTime.UtcNow,
                   Name="",Notes="",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },
                new DomModel.TechnicalSpecialistCompetency()
                {
                   ID=1, Epin=54, Competency="",Detail="",DisplayOrder=1,Duration="",EffectiveDate= DateTime.UtcNow,ExpiryDate=DateTime.UtcNow,
                   Name="",Notes="",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                },
            };
        }

        #endregion

        #region EducationalQualification
        public static IList<DomModel.TechnicalSpecialistEducationalQualification> GetEducationalQualificationMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistEducationalQualification>()
            {
                new DomModel.TechnicalSpecialistEducationalQualification()
                {
                   Id=1,Epin=54,Address="",
                   CountryName ="United Kingdom" ,
                   CountyName="Andorra la Vella" ,
                   CityName="Brighton" ,
                   Institution ="HBTI",Percentage=90,DisplayOrder=1, Place="Kanpur",Qualification="MCA",
                   FromDate = DateTime.UtcNow,ToDate=DateTime.UtcNow,
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },
                new DomModel.TechnicalSpecialistEducationalQualification()
                {
                   Id=2,Epin=54,Address="",
                   CountryName ="United Kingdom" ,
                   CountyName="Andorra la Vella" ,
                   CityName="Brighton" ,
                   Institution ="HBTI",Percentage=90,DisplayOrder=1, Place="Kanpur",Qualification="MCA",
                   FromDate = DateTime.UtcNow,ToDate=DateTime.UtcNow,
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                },
            };
        }

        #endregion

        #region Contact Information
        public static IList<DomModel.TechnicalSpecialistContact> GetContactMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistContact>()
            {
                new DomModel.TechnicalSpecialistContact()
                {
                   Id=1,Epin=54,Address="",
                   Country ="United Kingdom" ,
                   County="Andorra la Vella" ,
                   City="Brighton" ,
                   ContactType=Common.Enums.ContactType.Address,EmailAddress="singh.shru@gmail.com",EmergencyContactName="Ch",FaxNumber="9999999999",
                   IsPrimary=true,MobileNumber="9999999999",PostalCode="560093",TelephoneNumber="9999999999",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },
                new DomModel.TechnicalSpecialistContact()
                {
                   Id=2,Epin=54,Address="",
                   Country ="United Kingdom" ,
                   County="Andorra la Vella" ,
                   City="Brighton" ,
                   ContactType=Common.Enums.ContactType.Address,EmailAddress="singh.shru@gmail.com",EmergencyContactName="Ch",FaxNumber="9999999999",
                   IsPrimary=true,MobileNumber="9999999999",PostalCode="560093",TelephoneNumber="9999999999",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                },
            };
        }

        #endregion

        #region PaySchdule
        public static IList<DomModel.TechnicalSpecialistCompetency> GetTechnicalSpecialistCompetencyMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistCompetency>()
            {
                new DomModel.TechnicalSpecialistCompetency()
                {
                 ID=1,Epin=54,  Competency="Competency",  EffectiveDate=DateTime.UtcNow, DisplayOrder=1,  Duration="2Years",  ExpiryDate=DateTime.UtcNow, Notes="Test Note", RecordType="Competency",  ScoreField=20,
                 LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,
                },
                new DomModel.TechnicalSpecialistCompetency()
                {
                 ID=5,Epin=54,  Competency="Competency",  EffectiveDate=DateTime.UtcNow, DisplayOrder=1,  Duration="2Years",  ExpiryDate=DateTime.UtcNow, Notes="Test Note", RecordType="Competency", ScoreField=20,
                 LastModification=DateTime.UtcNow,ModifiedBy=null,UpdateCount=0,
                },
            };
        }
        #endregion
        #region InternalTraining
        public static IList<DomModel.TechnicalSpecialistInternalTraining> GetInternalTrainingMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistInternalTraining>()
            {
                new DomModel.TechnicalSpecialistInternalTraining()
                {
                   ID=1, Epin=54, Competency="",Detail="",DisplayOrder=1,EffectiveDate= DateTime.UtcNow,ExpiryDate=DateTime.UtcNow,
                   Name="",Notes="", 
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },
                new DomModel.TechnicalSpecialistInternalTraining()
                {
                   ID=1, Epin=54, Competency="",Detail="",DisplayOrder=1,EffectiveDate= DateTime.UtcNow,ExpiryDate=DateTime.UtcNow,
                   Name="",Notes="",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                },
            };
        }

        #endregion
        #region Training
        public static IList<DomModel.TechnicalSpecialistTraining> GetTrainingMockedDomainData()
        {
            return new List<DomModel.TechnicalSpecialistTraining>()
            {
                new DomModel.TechnicalSpecialistTraining()
                {
                   Id=8, Epin=54, Training="ACA",VerifiedBy="Mark Peacock",ExpiryDate=DateTime.UtcNow,
                   Name="",Notes="",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,

                },
                new DomModel.TechnicalSpecialistTraining()
                {
                   Id=11, Epin=54, Training="ACA",VerifiedBy="Mark Peacock",ExpiryDate=DateTime.UtcNow,
                   Name="",Notes="",
                   LastModification=DateTime.UtcNow,ModifiedBy="test",UpdateCount=0,
                },
            };
        }

        #endregion

    }
}
