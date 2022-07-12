namespace Evolution.Common.Enums
{
    public enum ModuleType
    {
        Master = 100,
        Company = 1000,
        Customer = 2000,
        Project = 3000,
        TechnicalSpecialist = 4000,
        Visit = 5000,
        Document = 6000,
        Timesheet = 7000,
        Gateway = 9000,
        Authentication = 9101,
        Contract = 10000,
        Supplier = 10001,
        Security = 10100,
        Assignment = 10101,
        SqlAuditLog = 10102,
        Draft = 10103,
        Email = 10104,
        ResourceSearch = 10105,
        Calendar = 10106
    }

    public enum ResponseType
    {
        Success = 1,
        PartiallySuccess = 61,
        Error = 11,
        Warning = 21,
        Exception = 31,
        Validation = 41,
        DbException = 51
    }

    /// <summary>
    /// Message Type will have a number range for each module.
    /// </summary>
    public enum MessageType
    {
        Success = 1,
        Error = 11,
        Warning = 21,
        Exception = 31,
        Validation = 41,
        DbException = 51,

        #region Master

        MasterInvalidCurrency = 101,
        MasterInvalidCertification = 102,
        MasterInvalidTraining = 103,
        MasterInvalidCountry = 104,
        MasterInvalidCounty = 105,
        MasterInvalidCity = 106,
        MasterInvalidInternalTraining = 107,
        MasterInvalidCompetency = 108,
        MasterInvalidTaxonomyService = 109,
        InvalidMasterData = 110,
        InvalidMasterDataId = 111,
        CountryAlreadyExsists = 112,
        InvalidRegion = 113,
        CountyAlreadyExsists = 114,
        InvalidLanguageRefType = 115,
        InvalidRefType = 116,
        MasterInvalidTSStampCountryCode = 117,


        #endregion

        #region Common Message
        UnableToProcess = 501,
        #endregion

        #region Customer Message Types
        Customer = 2000,
        Customer_NoRecordToUpdate = 2001,
        Customer_InvalidCustomerCode = 2002,
        Customer_UpdateCountMismatch = 2003,
        Customer_CodeGenerationError = 2004,
        Customer_NoNewRecords = 2005,
        Customer_MIIWAIdAlreadyExists = 2006,
        Customer_DuplicateCustomer = 2007,

        Customer_Note_InvalidCustomerCode = 2051,
        Customer_Note_NoRecordToDelete = 2052,
        Customer_Note_PartialDelete = 2053,
        Customer_Note_PartialUpdate = 2054,
        Customer_Note_UpdateCountMismatch = 2055,
        Customer_Note_NoRecordToUpdate = 2056,
        Customer_Note_InvalidNoteId = 2057,
        Customer_Note_NoNewRecords = 2058,
        Customer_Note_Partialinserted = 2059,

        Customer_Document_CustomerCodeNotExists = 2101,
        Custmer_Document_NoRecordToDelete = 2102,
        Customer_Document_PartialDelete = 2103,
        Customer_Document_NoRecordToUpdate = 2104,
        Customer_Document_UpdateCountMismatch = 2105,
        Customer_Document_DocumentNotExists = 2106,
        Customer_Document_PartialUpdate = 2107,
        Customer_Document_Partialinserted = 2108,
        Customer_Document_NoNewRecords = 2109,
        Customer_Document_CustomerCodeIsEmpty = 2110,
        Customer_Document_AddFailureInMongoDB = 2111,
        Customer_Document_DeleteFailureInMongoDB = 2112,

        Customer_Contact_NoRecordToDelete = 2151,
        Customer_Contact_PartialDelete = 2152,
        Customer_Contact_InvalidAddressId = 2153,
        Customer_Contact_NoRecordToUpdate = 2154,
        Customer_Contact_UpdateCountMismatch = 2155,
        Customer_Contact_InvalidContactId = 2156,
        Customer_Contact_PartialUpdate = 2157,
        Customer_Contact_NoNewRecords = 2158,
        Customer_Contact_PartialInsert = 2159,
        Customer_Contact_InUse_Contract = 2160,
        Customer_Contact_InUse_Project = 2161,
        Customer_Contact_InUse_Assignment = 2162,
        //used in contract
        Customer_Contact_Invalid = 2163,
        Customer_InvoiceContact_Invalid = 2164,
        Customer_Contact_Portal_Exists = 2165,

        Customer_AssRef_InvalidCustomerCode = 2201,
        Customer_AssRef_NoRecordToDelete = 2202,
        Customer_AssRef_PartialDelete = 2203,
        Customer_AssRef_NoRecordToUpdate = 2204,
        Customer_AssRef_UpdateCountMismatch = 2205,
        Customer_AssRef_InvalidAssignmentReferenceId = 2206,
        Customer_AssRef_PartialUpdate = 2207,
        Customer_AssRef_Partialinserted = 2208,
        Customer_AssRef_NoNewRecords = 2209,
        Customer_AssRef_AlreadyExists = 2210,
        Customer_AssRef_InvalidAssignmentReferenceType = 2211,

        CustAddr_InvalidCustomerCode = 2251,
        CustAddr_NoRecordToDelete = 2252,
        CustAddr_PartialDelete = 2253,
        CustAddr_NoRecordToUpdate = 2254,
        CustAddr_UpdateCountMismatch = 2255,
        CustAddr_InvalidAddressId = 2256,
        CustAddr_PartialUpdate = 2257,
        CustAddr_Partialinserted = 2258,
        CustAddr_NoNewRecords = 2259,
        CustAddr_CityIsInvalid = 2260,
        CustAddr_InUse_Contact = 2261,
        CustAddr_InvalidPayload = 2262,
        CustAddr_ExisitingAddress = 2263,
        //used in contract
        CustAddr_Address = 2264,
        CustAddr_Code_Invalid = 2265,
        CustAddr_Invoice_Address = 2266,
        CustAddr_InUse_Contract = 2267,



        Customer_ACRef_InvalidCustomerCode = 2301,
        Customer_ACRef_NoRecordToDelete = 2302,
        Customer_ACRef_PartialDelete = 2303,
        Customer_ACRef_NoRecordToUpdate = 2304,
        Customer_ACRef_UpdateCountMismatch = 2305,
        Customer_ACRef_InvalidAccountReferenceId = 2306,
        Customer_ACRef_PartialUpdate = 2307,
        Customer_ACRef_Partialinserted = 2308,
        Customer_ACRef_NoNewRecords = 2309,
        Customer_ACRef_InvalidCompanyCode = 2310,
        Customer_ACRef_AlreadyExists = 2311,

        #endregion

        #region Company Message Types

        RecordUpdatedAlready = 1003,
        CompanyCodeCannotBeNullOrEmpty = 1001,
        InvalidCompanyCode = 1002,
        CompanyInvalidRecordStatus = 1004,
        CompanyNameCannotBeNullOrEmpty = 1005,
        CompanyMiiwaIdCannotBeNullOrZero = 1006,
        CompanyMappingIssue = 1007,
        CompanyAlreadyExist = 1008,
        CompanyUpdateCountMismatch = 1009,
        CompanyLogoText = 1010,
        InvalidCompanyCodeWithCode = 1011,
        InvalidADUser = 1012,

        //Company Office
        CompOffice_InvalidCompanyCode = 1101,

        //OfficeRecordNotFound = 1101,
        OfficeCompanyCodeEmpty = 1102,
        OfficeInvalidCompanyCode = 1103,
        OfficeIdIsInvalidOrNotExists = 1104,
        OfficePartialDataSaved = 1105,
        OfficeInvalidRecordStatus = 1106,
        OfficeMappingIssue = 1107,
        OfficeBasedOnCompany = 1108,
        OfficeInsertFailed = 1109,
        OfficeUpdateFailed = 1110,
        OfficeDeleteFailed = 1111,
        OfficeDeleted = 1112,
        OfficeAddressHasBeenUpdatedByOther = 1113,
        OfficeAddressAlreadyExsists = 1114,
        OfficeAddressCannotBeDeleted = 1115,
        OfficeCountryStateCityNotExists = 1116,
        OfficeCountryStateNotExists = 1117,
        OfficeCountryNotExists = 1118,
        ParentOfficeName_Not_Exists = 1119,
        FrameworkOfficeName_Not_Exists = 1120,
        AccountReferenceAlreadyExists = 1121,
        InvalidCompanyOfficeName = 1122,

        //Company Payroll
        CompPayroll_InvalidCompanyCode = 1153,
        //PayrollRecordNotFound = 1151,
        PayrollCompanyCodeEmpty = 1152,
        PayrollNotExists = 1153,
        PayrollCannotBeDeleted = 1154,
        PayrollIdIsInvalidOrNotExists = 1155,
        PayrollInvalidRecordStatus = 1156,
        PayrollMappingIssue = 1157,
        PayrollBasedOnCompany = 1158,
        PayrollInsertFailed = 1159,
        PayrollUpdateFailed = 1160,
        PayrollDeleteFailed = 1161,
        PayrollDeleted = 1162,
        PayrollRecordUpdatedAlready = 1163,
        PayrollTypeExists = 1164,
        ExportPrefixNotExists = 1165,
        PayrollTypeAreadyExists = 1166,
        //PayrollValidationError = 1166,

        //Company Payroll period
        CompPayrollPeriod_AlreadyExists = 1053,
        CompPayrollPeriod_NoRecord = 1051,
        CompPayrollPeriod_CompanyOrPayroll = 1052,
        CompPayrollPeriod_InvalidPayload = 1054,
        CompPayollPeriod_PayrollNotFound = 1055,
        CompPayollPeriod_UpdateCountMismatch = 1056,
        CompPayrollPeriodIdIsInvalidOrNotExists = 1057,

        // PayrollTypeRecordNotFound = 1051,
        //PayrollTypeCompanyCodeEmpty = 1052,
        //PayrollTypeInvalidCompanyCode = 1053,
        //PayrollTypeSqlException = 1054,
        //PayrollTypePartialDataSaved = 1055,
        //PayrollTypeInvalidRecordStatus = 1056,
        //PayrollTypeMappingIssue = 1057,
        PayrollTypeBasedOnCompany = 1058,
        PayrollTypeInsertFailed = 1059,
        PayrollTypeinsertFailed = 1060,
        PayrollTypeDeleteFailed = 1061,
        PayrollTypeDeleted = 1062,
        PayrollTypeRecordUpdatedAlready = 1063,
        PayrollPeriodOverlapsEachOther = 1064,

        // Division
        DivisonIdIsInvalidOrNotExists = 1201,
        DivisionNullCompanyCode = 1202,
        DivisionInvalidCompanyCode = 1203,
        DivisionInvaidRecordStatus = 1204,
        DivionDataNotFound = 1205,
        DivisionDataMissing = 1206,
        DivisionCannotBeDeleted = 1208,
        DivisionPartialDataSave = 1209,
        DivisionPartialDataUpdate = 1210,
        DivisionPartialDataDeletion = 1211,
        DivisionInvalidDataSchema = 1212,
        DivisionHasBeenUpdatedByOther = 1213,
        DivisionNoRecordsToModify = 1214,
        DivisionDataAlreadyExists = 1215,
        DivisionNotExists = 1216,

        //Sub Division
        InvalidSubDivision = 1230,

        //Profile Status
        InvalidProfileStatus = 1240,

        //Profile Action
        InvalidProfileAction = 1250,

        //Employeement Type
        InvalidEmployementType = 1260,

        // CostCenter
        CostCenterIdIsInvalidOrNotExists = 1301,
        CostCenterCannotBeDeleted = 1302,
        CostCenterInvalidCompanyCode = 1303,
        CostCenterInvaidRecordStatus = 1304,
        CostCenterDataNotFound = 1305,
        CostCenterDataMissing = 1306,
        CostCenterDivisionInvalid = 1308,
        CostCenterPartialDataSave = 1309,
        CostCenterPartialDataUpdate = 1310,
        CostCenterPartialDataDeletion = 1311,
        CostCenterInvalidDataSchema = 1312,
        CostCenterHasBeenUpdatedByOther = 1313,
        CostCenterNoRecordsToModify = 1314,
        CostCenterDataAlreadyExists = 1315,
        CostCenterNotExists = 1316,

        // Company Invoices and Company Message
        CompanyMessageAlreadyExists = 1751,
        CompanyMessageDoesNotExists = 1752,


        // Company Remittance Text
        RemittanceSqlException = 1401,
        RemittanceCompanyCodeCannotbeNull = 1402,
        RemittanceInvalidRecordStatus = 1403,
        RemittancePartialDataSave = 1404,
        RemittancePartialDataUpdate = 1405,
        RemittancePartialDataDelete = 1406,
        RemittanceInvalidSchema = 1407,
        RemittanceMissingData = 1408,
        RemittanceAlreadyUpdated = 1409,
        RemittanceNoRecordsToModify = 1410,
        RemittanceNotExists = 1411,
        RemittanceCanNotBeDelete = 1412,

        FooterCanNotBeDelete = 1451,

        // Company Invoice text 

        InvoiceTextSqlException = 1501,
        InvoiceTextCompanyCodeCannotbeNull = 1502,
        InvoiceTextInvalidRecordStatus = 1503,
        InvoiceTextPartialDataSave = 1504,
        InvoiceTextPartialDataUpdate = 1505,
        InvoiceTextPartialDataDelete = 1506,
        InvoiceTextInvalidSchema = 1507,
        InvoiceTextMissingData = 1508,
        InvoiceTextAlreadyUpdated = 1509,
        InvoiceNoRecordsToModify = 1510,

        // Expected Margin text
        ExpectedIdIsInvalidOrNotExists = 1601,
        ExpectedMarginNullCompanyCode = 1602,
        ExpectedMarginInvalidCompanyCode = 1603,
        ExpectedMarginInvaidRecordStatus = 1604,
        ExpectedMarginDataNotFound = 1605,
        ExpectedMarginDataMissing = 1606,
        ExpectedMarginCannotBeDeleted = 1608,
        ExpectedMarginPartialDataSave = 1609,
        ExpectedMarginPartialDataUpdate = 1610,
        ExpectedMarginPartialDataDeletion = 1611,
        ExpectedMarginInvalidDataSchema = 1612,
        ExpectedMarginHasBeenUpdatedByOther = 1613,
        ExpectedMarginNoRecordsToModify = 1614,
        ExpectedMarginDataAlreadyExists = 1615,
        ExpectedMarginNotExists = 1616,


        // Documents
        DocumentIdIsInvalidOrNotExists = 1701,
        DocumentUniqueNameNotExists = 1702,
        DocumentNoRecordsToModify = 1703,
        DocumentsUpdatedPartially = 1704,
        DocumentsDeletedPartially = 1705,
        DocumentsSavedPartially = 1706,
        DocumentHasBeenUpdatedByOther = 1707,
        DocumentNoRecordsToSave = 1708,
        DocumentNoRecordsToDelete = 1709,
        DocumentNotInsertedInMongo = 1711,
        TempDocumentCouldNotDelete = 1712,

        // Notes

        NotesIdIsInvalidOrNotExists = 1801,
        NotesInvalidSchema = 1802,
        NotesNoRecordsToModify = 1803,
        NotesUpdatedPartially = 1804,
        NotesDeletedPartially = 1805,
        NotesSavedPartially = 1806,
        NotesHasBeenUpdatedByOther = 1807,
        NotesNoRecordsToSave = 1708,
        NotesNoRecordsToDelete = 1709,

        //CompanyTax
        TaxUpdateCountMismatch = 1901,
        TaxCompanyCodeEmpty = 1902,
        TaxInvalidCompanyCode = 1903,
        TaxCannotBeDeleted = 1904,
        TaxAlreadyExist = 1905,
        TaxInvalidRecordStatus = 1906,
        TaxNotExists = 1907,
        TaxIdIsInvalidOrNotExists = 1908,
        TaxUpdateFailed = 1909,
        TaxDeleteFailed = 1910,
        TaxBasedOnCompany = 1911,
        TaxRecordUpdatedAlready = 1912,
        TaxDeleted = 1913,
        TaxWithholdingInvalid = 1914,
        TaxSalesInvalid = 1915,

        //CompanyQualification
        QualificationRecordNotFound = 1951,
        QualificationCompanyCodeEmpty = 1952,
        QualificationInvalidCompanyCode = 1953,
        QualificationSqlException = 1954,
        QulaificationPartialDataSaved = 1955,
        QulaificationInvalidRecordStatus = 1956,
        QualificationBasedOnCompany = 1957,
        QualificationInsertFailed = 1958,
        QualificationUpdateFailed = 1959,
        QualificationDeleteFailed = 1960,
        QualificationMappingIssue = 1961,
        QualificationRecordUpdatedAlready = 1962,
        QualificationDeleted = 1963,


        #endregion

        #region document Pending Message Types

        DocumentPendingSqlException = 3001,

        #endregion

        #region Project

        //Project Module 

        ProjectContractNumberNotExists = 3001,
        ProjectCompanyDivisionNotExists = 3002,
        ProjectCompanyDivisionCostCenterNotExists = 3003,
        ProjectCompanyOfficeNameNotExists = 3004,
        ProjectTypeNotExists = 3005,
        ProjectIndustrySectorNotExists = 3006,
        Project_DbContextNotInitialized = 3007,
        InvalidProjectNumber = 3008,
        InvalidProjectManagedCoordinator = 3009,
        InvalidProjectCoordinator = 3016,
        InvalidProjectInvoicingCurrency = 3017,
        InvalidProjectLogoText = 3018,
        ProjectNumberNotGenerated = 3019,
        ProjectUpdatedByOtherUser = 3020,
        ProjectStatusClosed = 3021,
        ProjectCannotBeDeleted = 3022,
        ProjectSupplierPO=3023,
        ProjectBudgetValueExceedsContract=3024,
        ProjectBudgetHoursExceedsContract=3092,
        ProjectBudgetValueBelowAssignment =3093,
        ProjectBudgetHoursBelowAssignment =3094,


        // ProjectClientNotification 3010-3015
        ProjectContactNameNotExists = 3010,
        ProjectClientNotificationRecordUpdatedAlready = 3011,
        ProjectClientNotificationIdIsInvalidOrNotExists = 3012,
        ProjectClientNotificationExistsForContact = 3013,

        //Project Invoice attachement  3025 - 3035
        ProjectDocumentTypeNotExists = 3025,
        ProjectInvoiceAttachementIdIsInvalidOrNotExists = 3026,
        ProjectInvoiceAttachementExists = 3027,
        ProjectInvoiceAttachementRecordUpdatedAlready = 3028,

        // Project Invoice reference  3051 - 3061
        ProjectInvoiceReferenceTypeNotExists = 3051,
        ProjectInvoiceReferenceTypeIdIsInvalidOrNotExists = 3052,
        ProjectInvoiceReferenceExists = 3053,
        ProjectInvoiceReferenceRecordUpdatedAlready = 3054,

        //Project Notes 3071-3081
        ProjectNotesIdIsInvalidOrNotExsists = 3071,

        //Project Documents 3081-3091
        ProjectDocumentsRecordUpdatedAlready = 3081,
        ProjectDocumentsIdIsInvalidOrNotExsists = 3091,

        #endregion

        #region Tech Spec
        // TechSpec Module

        TechSpec_SqlException = 4001,
        TechSpec_InvalidRecordStatus = 4002,
        TechSpec_DataPartiallyUpdated = 4003,
        TechSpec_DataPartiallySaved = 4004,
        TechSpec_DataPartiallyDeleted = 4005,
        TechSpec_RecordUpdatedAlready = 4006,
        TechSpec_DbContextNotInitialized = 4007,
        #endregion

        #region Visit 
        //Visit Module
        Visit_SqlException = 5001,
        Visit_InvalidRecordStatus = 5002,
        Visit_DataPartiallySaved = 5003,
        Visit_DataPartiallyUpdated = 5004,
        Visit_DataPartiallyDeleted = 5005,
        Visit_RecordUpdatedAlready = 5006,
        Visit_DbContextNotInitialized = 5007,
        InvalidVisitNumber = 5008,
        VisitNotExists = 5009,
        VisitIsBeingUsed = 5010,
        VisitUpdateCountMismatch = 5011,

        VisitSupplierPerformanceInvalid = 5041,
        VisitSupplierPerformanceUpdateCountMisMatch = 5042,

        //Visit Reference
        VisitReferenceInvalid = 5051,
        VisitReferenceUpdateCountMisMatch = 5052,
        VisitReferenceDuplicateRecord = 5053,

        VisitTechnicalSpecialistNotExists = 5061,
        VisitTechnicalSpecialistUpdateCountMisMatch = 5062,
        VisitTechnicalSpecialistIsBeingUsed = 5063,
        VisitTSDuplicateRecord = 5064,

        VisitTSExpenseTypeInvalid = 5071,
        VisitTSChargeCurrencyInvalid = 5072,
        VisitTSPayCurrencyInvalid = 5073,
        VisitTSVisitContractInvalid = 5074,
        VisitTSTimeUpdateCountMisMatch = 5075,
        VisitExpenseInvalid = 5076,
        VisitTravelInvalid = 5077,
        VisitTimeInvalid = 5078,
        VisitExpenseUpdateCountMisMatch = 5079,
        VisitTravelUpdateCountMisMatch = 5080,
        VisitConsumableUpdateCountMisMatch = 5081,
        VisitRecordCount = 5082,
        #endregion

        #region Timesheet

        Timesheet_SqlException = 7001,
        Timesheet_InvalidRecordStatus = 7002,
        Timesheet_DataPartiallySaved = 7003,
        Timesheet_DataPartiallyUpdated = 7004,
        Timesheet_DataPartiallyDeleted = 7005,
        Timesheet_RecordUpdatedAlready = 7006,
        Timesheet_DbContextNotInitialized = 7007,
        TimesheetNotExists = 7008,
        TimesheetIsBeingUsed = 7009,
        TimesheetUpdateCountMismatch = 7010,

        TimesheetReferenceInvalid = 7051,
        TimesheetReferenceUpdateCountMisMatch = 7052,
        TimesheetReferenceDuplicateRecord = 7053,

        TimesheetTechnicalSpecialistNotExists = 7061,
        TimeTechnicalSpecialistUpdateCountMisMatch = 7062,
        TimesheetTechnicalSpecialistIsBeingUsed = 7063,
        TimesheetTSDuplicateRecord = 7064,


        TimesheetTSExpenseTypeInvalid = 7071,
        TimesheetTSChargeCurrencyInvalid = 7072,
        TimesheetTSPayCurrencyInvalid = 7073,
        TimesheetTSTimesheetContractInvalid = 7074,
        TimesheetTSTimeUpdateCountMisMatch = 7075,
        TimesheetExpenseInvalid = 7076,
        TimesheetTravelInvalid = 7077,
        TimesheetTimeInvalid = 7078,
        TimesheetExpenseUpdateCountMisMatch = 7079,
        TimesheetTravelUpdateCountMisMatch = 7080,
        TimesheetConsumableUpdateCountMisMatch = 7081,


        #endregion

        #region Document
        //Document Approval
        DocumentRecordNotFound = 8001,
        DocumentCompanyCodeOrCoordinatorEmpty = 8002,
        DocumentInvalidCompanyCode = 8003,
        DocumentApprovalSqlException = 8004,
        DocumentPartialDataSaved = 8005,
        DocumentInvalidRecordStatus = 8006,
        DocumentMappingIssue = 8007,
        DocumentBasedOnCompany = 8008,
        DocumentInsertFailed = 8009,
        DocumentUpdateFailed = 8010,
        DocumentDeleteFailed = 8011,
        DocumentnDeleted = 8012,
        DocumentRecordUpdatedAlready = 8013,

        DocumentAppovalIdNotExists = 8101,
        DocumentAppovalUpdatedByOtherUser = 8102,
        DocumentCoordinatorNotFound = 8103,
        DocumentUploaderNotFound = 8104,

        Document_CustomerCode = 6001,
        Document_UniqueCode = 6002,
        Document_NotExists = 6003,
        DocumentPath_NotExists = 6004,
        Document_InvalidUniqueName = 6005,
        Document_UploadFailed = 6006,
        Document_ChangeStatusFailed = 6007,
        Document_Not_Supported = 6008,
        InvalidPayLoad = 6009,
        Madatory_Param = 6010,
        Madatory_Param_DocumentType = 6011,
        #endregion

        #region Gateway
        Gateway_UnableToProcessRequest = 9001,
        #endregion

        #region Authentication
        Auth_InvalidCredential = 9101,
        Auth_UserIsNotActive = 9102,
        Auth_InvalidRefreshToken = 9103,
        Auth_ExpiredRefreshToken = 9104,
        Auth_TokenAssocFailed = 9105,
        Auth_InvalidAccessToken = 9106,
        Auth_InvalidClientOrAudienceCode = 9107,
        Auth_User_Role_Not_Associated = 9108,
        Auth_User_Question_Answer_Not_Passed = 9109,
        Auth_Security_Question_NotSetup = 9110,
        Auth_Security_Question_Answer_Vald_Failed = 9111,
        Auth_ForgetPassword_Not_For_User = 9112,
        Auth_User_Locked = 9113,
        User_Denied_During_Evolution_Lock = 9114,
        Auth_Account_Locked = 9115, 
        #endregion

        #region Admin
        Admin_UserDoesNotExist = 9200,
        #endregion

        #region Contract

        //contract
        InvalidContractNumber = 10001,
        InvalidContractInvoicePaymentTerm = 10002,
        InvalidContractRemittance = 10003,
        InvalidContractFooter = 10004,
        InvalidContractInvoicingCurrency = 10005,
        ContractUpdatedByOtherUser = 10006,
        ContractCannotBeDeleted = 10007,
        ContractAlreadyExists = 10008,
        ContractNumberNotGenerated = 10009,
        ContractStatusClosed = 10010,
        InvalidCustomerContractNumber = 10011,
        InvalidFrameworkContractNumber = 10012,
        InvalidParentContractNumber = 10013,
        ChildContractExists = 10014,
        RelatedContractExists = 10015,
        ContractStartDateIsGreaterThanProjecrStartDate = 10016,
        RelatedFrameworkContractNotExists = 10017,
        ContractBudgetValueBelowProject = 10627,
        ContractBudgetHoursBelowProject = 10628,

        //contract Invoice attachement  10050 - 10075
        DocumentTypeNotExists = 10051,
        ContractInvoiceAttachementIdIsInvalidOrNotExists = 10052,
        ContractInvoiceAttachementExists = 10053,
        ContractInvoiceAttachementRecordUpdatedAlready = 10054,

        //contract Invoice reference  10076 - 10100
        ContractInvoiceReferenceTypeNotExists = 10076,
        ContractInvoiceReferenceTypeIdIsInvalidOrNotExists = 10077,
        ContractInvoiceReferenceExists = 10078,
        ContractInvoiceReferenceRecordUpdatedAlready = 10079,

        // Contract Fixed Exchange Rates 10501 - 10600
        ContractNumberDoesNotExists = 10501,
        InvalidCurrencyFromOrCurrencyTo = 10502,
        FixedExchangeRateDoesNotExists = 10503,
        FixedExchangeRateAlreadyExists = 10504,
        FixedExchangeRateAlreadyUpdated = 10505,
        FixedExchangeRateFromAndToCurrencySame = 10506,

        // Contract Schedule - 10601 - 10606 
        ScheduleContractNumberDoesNotExists = 10601,
        InvalidChargeCurrency = 10602,
        ScheduleNameAlreadyExists = 10603,
        ScheduleNameDoesNotExists = 10604,
        ScheduleAlreadyUdpated = 10605,
        ScheduleCannotBeDeleted = 10606,
        BaseScheduleDoesNotExists = 10607,
        ContractScheduleInvalidId = 10608,
        ContractAssignmentScheduleNameAlreadyExists = 10609,

        // Contract Schedule rates - 10621 - 10630 
        ContractScheduleRateChargeTypeNotExists = 10621,
        ContractScheduleRateIdIsInvalidOrNotExists = 10622,
        ContractScheduleRateRecordUpdatedAlready = 10623,
        ContractScheduleRateStandardInspectionTypeChargeRateNotExists = 10624,
        ContractScheduleRateBaseRateIdDoesNotExists = 10625,
        ContractScheduleRateCannotBeDeleted = 10626,

        //contract document 
        ContractDocumentRecordUpdatedAlready = 10201,
        ContractDocumnetIdIsInvalidOrNotExsists = 10202,
        #endregion

        #region Supplier
        //Supplier
        InvalidCity = 11001,
        SupplierUpdatedByOtherUser = 11002,
        InvalidSupplier = 11003,
        SupplierCannotBeDeleted = 11004,
        DuplicateSupplierName = 11005,

        //Supplier Contact
        SupplierContactUpdatedByOtherUser = 11051,
        InvalidSupplierContact = 11052,
        SupplierContactCannotBeDeleted = 11053,


        //Supplier Document
        SupplierDocumentUpdatedByOtherUser = 11061,
        InvalidSupplierDocument = 11062,

        #endregion

        #region Supplier Purchase Order

        // Supplier PO
        SupplierPOInvalidProjectNumber = 12001,
        SupplierPOInvalidMainSupplier = 12002,
        SupplierPODoesNotExist = 12003,
        SupplierPOCantBeDeleted = 12004,
        SupplierPOAlreadyUpdated = 12005,
        SupplierCannotBeUpdated = 12006,

        // Supplier PO Note 
        SupplierPoNoteDoesNotExist = 12051,

        // Sub Supplier
        InvalidSuppierPurchaseOrder = 12101,
        SupplierPurchaseOrderDoesNotExist = 12102,
        SubSupplierAlreadyAssociated = 12103,
        SubSupplierDoesNotExist = 12104,
        SubSupplierAlreadyUpdated = 12105,
        SubSupplierCannotBeDeleted = 12106,
        SubSupplierCannotBeUpdated = 12107,

        //SupplierPO Document
        SupplierPODocumentAlreadyUpdated = 12211,
        SupplierPODocumnetIdIsInvalidOrNotExsists = 12212,
        #endregion

        #region FileExtractor
        FileDoesNotExist = 20001,
        InvalidFileForProcess = 20002,
        FileException = 20003,
        #endregion

        #region DocumentSave

        DocumnetIdIsInvalidOrNot = 20051,
        DocumentRecordUpdated = 20052,
        DocumentUniqueNameNotExist = 20053,
        DocumentUniqueNameExist = 20054,
        AllDocumentUploadFilepathFull = 20059,
        #endregion

        #region TechnicalSpecialist 
        TsLoginCredentialLogInNameAlreadyExists = 30001,
        TsLoginCredentialLogInNameNotExists = 30003,
        TechnicalSpecialistInvalidEpin = 30005,
        TechnicalSpecialistStampInfoHasBeenUpdatedByOther = 30006,
        TechnicalSpecialistStampIdIsInvalidOrNotExists = 30007,
        TechnicalSpecialistStampCountryNotExists = 30008,
        ContactInfoHasBeenUpdatedByOther = 30009,
        TechnicalSpecialistPrimaryAddressIsExist = 30010,
        TechnicalSpecialistHasBeenUpdatedByOther = 30011,
        TechnicalSpecialistIdIsInvalidOrNotExists = 30012,
        TechnicalSpecialistInvalidProfileAction = 30013,
        TechnicalSpecialistCategorySubcategoryServiceNotExists = 30014,
        TechnicalSpecialistTaxonomyIsInvalidOrNotExists = 30015,
        TechnicalSpecialistInvalidTraining = 30016,
        TechnicalSpecialistInvalidCompetency = 30017,
        TechnicalSpecialistPassportOriginCountryNotExists = 30018,
        TechnicalSpecialistCannotBeDeleted = 30019,
        TechnicalSpecialistInvalidTrainingAndCompetencyId = 30020,
        TechnicalSpecialistInvalidTrainingOrCompetencyData = 30021,
        TechnicalSpecialistLanguageIsInvalidOrNotExists = 30022,
        MasterDataNotExists = 30023,
        InvalidDraftID = 30024,
        DraftHasBeenUpdatedByOther = 30025,
        TechnicalSpecialistCompetencyCannotBeDeleted = 30026,
        TechnicalSpecialistInternalTrainingCannotBeDeleted = 30027,

        TechnicalSpecialistPayScheduleNameIsInvalidOrNotExists = 30029,
        TechnicalSpecialistCompetencyHasBeenUpdatedByOther = 30030,
        TechnicalSpecialistInternalTrainingHasBeenUpdatedByOther = 30031,

        TsEPinAlreadyExist = 30033,
        TsUpdatedByOther = 30034,
        TsEPinDoesNotExist = 30035,
        TsEPinIsBeingUsed = 30036,
        TsUpdateRequestNotExist = 30037,
        TsStampUpdateRequestNotExist = 30038,
        TsStampAlreadyExist = 30039,
        TsStampNumberIsBeingUsed = 30040,
        TsStampNumDoesNotExist = 30041,

        InvalidTechSpecialist = 30052,


        EducationalQulificationAlreadyExist = 30063,
        TstsEduQulificationUpdateRequestNotExist = 30064,
        TsEduQulificationIsBeingUsed = 30065,

        TsWorkHistoryUpdatedByOther = 14002,
        langCapabilitiesAlreadyExist = 14003,



        #endregion

        #region TechPaySchedule

        TsPayScheduleUpdatedByOther = 30051,
        TsPayScheduleIdDoesNotExist = 30052,
        TsPayScheduleAlreadyExist = 30053,
        TsPayScheduleUpdateRequestNotExist = 30054,
        TsPayScheduleIsBeingUsed = 30055,
        TsPayScheduleIsInvalid = 30060,
        #endregion

        #region TechPayRate

        TsPayRateUpdatedByOther = 30056,
        TsPayRateIdDoesNotExist = 30057,
        TsPayRateUpdateRequestNotExist = 30058,
        TsPayRateIsBeingUsed = 30059,
        TsPayRateExpectedFrom = 30061,
        TsPayRateExpectedToDate= 30062,

        #endregion

        #region TechQualification
        QualificationUpdatedByOtherUser = 30101,
        InvalidTechSpecialistQualification = 30102,
        InvalidQualification = 30103,
        InvalidQualificationCountry = 30104,
        InvalidQualificationCounty = 30105,
        InvalidQualificationCity = 30106,
        QualificationDocument = 30107,
        #endregion

        #region TechCodeAndStandard
        CodeUpdatedByOtherUser = 30151,
        InvalidTechSpecialistCode = 30152,
        InvalidCodeAndStandard = 30153,
        InvalidCodeMasterData = 30154,

        TsCodeAndStandardExist = 30042,
        TsCodesDoesNotExist = 30043,
        TsCodesAlreadyExist = 30044,
        TsCodeUpdateRequestNotExist = 30045,
        TsCoderIsBeingUsed = 30046,
        #endregion

        #region TechTaxonomy
        InvalidTaxonomycategory = 301353,
        InvalidSubCategory = 301354,
        InvalidService = 301355,
        TaxonomyUpdatedByOtherUser = 301356,
        InvalidTaxonomyInfo = 301357,
        TsTaxonomyAlreadyExist = 301358,
        #endregion

        #region
        ContactUpdatedByOtherUser = 301359,
        InvalidContactInfo = 301360,

        #endregion

        #region TechSpecialistWorkHistory
        InvalidWorkHistory = 14000,
        TechSpecialistWorkHistoryUpdatedByOtherUser = 14001,
        #endregion

        #region TechspecilaistComputerKnowledge
        InvalidComputerKnoweledgeInfo = 15000,
        InvalidTechSplComputerElectronicKnoweledgeInfo = 15001,
        ComputerKnoweledgeInfoUpdatedByOtherUser = 15002,
        TechnicalSpecialistExpenseTypeNotExists = 15003,
        TechnicalSpecialistCertificateNameTypeNotExists = 15006,

        TsComputerElectronicKnowledgeExist = 30047,
        TsComputerElectronicKnowledgeDoesNotExist = 30048,
        TsComputerElectronicKnowledgeAlreadyExist = 30049,
        TsComputerElectronicKnowledgeUpdateRequestNotExist = 30050,
        TsComputerElectronicKnowledgeIsBeingUsed = 30080,
        InvalidComputerKnowledge = 30081,

        Madatory_Param_ProfileAction = 30082,
        Madatory_Param_SubDivisionName = 30083,
        Madatory_Param_ProfileStatus = 30084,
        Madatory_Param_FirstName = 30085,
        Madatory_Param_LastName = 30086,
        Madatory_Param_Primary_MobileNumber = 30087,
        Madatory_Param_Primary_Email = 30088,
        Madatory_Param_Country = 30089,
        Madatory_Param_State = 30090,
        Madatory_Param_CityOrPostalCode = 30091,
        Madatory_Param_Address = 30092,
        Madatory_Param_UserName = 30093,
        Madatory_Param_Password = 30094,
        Madatory_Param_Professional_Educational_uploadCV = 30095,
        Madatory_Param_Commodity_Equipment_Knowledge = 30096,
        Madatory_Param_PayScheduleWithOneRate = 30097,
        Madatory_Param_PayRateWithOneSchedule = 30098,
        Madatory_Param_Salutation = 30099,
        Madatory_Param_SecurityQuestion = 30100,
        Madatory_Param_SecurityAnswer = 30108,
        Madatory_Param_LanguageCapabilities = 30109,
        Madatory_Param_Workhistory = 30110,
        Madatory_Param_TechSpechStartDate = 30111,
        Madatory_Param_EmergencyContactName = 30112,
        Madatory_Param_EmergencyContact_MobileNumber = 30113,
        Madatory_Param_EmergencyContact_TaxReference = 30114,
        Madatory_Param_EmergencyContact_PayrollReference = 30115,
        Madatory_Param_EmergencyContact_CompanyPayrollName = 30116,
        Madatory_Param_EmergencyContact_PaySchedule = 30117,
        Madatory_Param_EmergencyContact_EmploymentType = 30118,
        Madatory_Param_CompanyCode = 30119,
        Duplicate_Record_LanguageCapability = 30120,
        Duplicate_Record_HARD_SOFT_STAMP = 30121,
        Hard_Stamp_Number_Validation= 30122,
        Soft_Stamp_Number_Validation=30123,
        #endregion

        #region CustomerApproval
        CustomerApprovalInfoUpdatedByOtherUser = 16001,
        InvalidCustomerApprovalInfo = 16002,
        InvalidTechSpecialistCustomer = 16003,
        TsCustomerApprovalUpdateRequestNotExist = 16004,
        TsCustomerNameAlreadyExist = 16005,
        TsCustomerNameIsBeingUsed = 16006,
        TsCustomerApprovalDoesNotExist = 16007,
        InvalidCustomerCode = 16008,
        TsCustomerApprovalUpdatedByOther = 16009,
        sCustomerApprovalExpiry = 16010,
        #endregion

        #region languageCapability

        InvalidLanguageCode = 16020,
        TsLanguageDoesNotExist = 16021,
        TsLanguage = 16022,
        TsWritingCapabilityLevel = 16023,
        TsSpeakingCapabilityLevel = 16024,
        TsComprehensionCapabilityLevel = 16025,
       
        #endregion

        #region CommodityEquipmentKnowledge
        InvalidCommodityEquipmentKnowledgeInfo = 17001,
        CommodityEquipmentKnowledgeInfoUpdatedByOtherUser = 17002,
        InvalidCommodity = 17003,
        InvalidEquipment = 17004,
        TsComdtyEquipmentKnowledgeAlreadyExist = 17005,


        #endregion

        #region Notes
        InvalidNotes = 18000,
        #endregion

        #region ExtraCode
        InvalidId = 17020,
        NoFailedDocuments = 20055,
        NotSupportedDocument = 20056,
        InvalidFileExtention = 20057,
        MasterDocumentTypesListNotFound= 20058,
        #endregion

        #region Security
        UserAlreadyExist = 20101,
        UserUpdateCountMismatch = 20102,
        UserNotExists = 20103,
        UserIdCannotBeNullOrEmpty = 20104,
        UserIsBeingUsed = 20105,
        RequestedUpdateUserNotExists = 20106,
        UserLogonNameInvalid = 20107,
        UserEmailInvalid = 20108,

        RoleAlreadyExist = 20111,
        RoleUpdateCountMismatch = 20112,
        RoleNotExists = 20113,
        RoleIdCannotBeNullOrEmpty = 20114,
        RoleIsBeingUsed = 20115,
        RequestedUpdateRoleNotExists = 20116,

        ActivityAlreadyExist = 20121,
        ActivityUpdateCountMismatch = 20122,
        ActivityNotExists = 20123,
        ActivityIdCannotBeNullOrEmpty = 20124,
        ActivityIsBeingUsed = 20125,
        RequestedUpdateActivityNotExists = 20126,

        ModuleAlreadyExist = 20131,
        ModuleUpdateCountMismatch = 20132,
        ModuleNotExists = 20133,
        ModuleIdCannotBeNullOrEmpty = 20134,
        ModuleIsBeingUsed = 20135,
        RequestedUpdateModuleNotExists = 20136,

        ModuleActivityAlreadyExsisits = 20201,

        RoleActivityAlreadyExist = 20141,
        RoleActivityUpdateCountMismatch = 20142,
        InvalidRoleActivityId = 20143,
        RoleActivityIdCannotBeNullOrEmpty = 20144,

        ApplicationNotExists = 20171,
        ApplicationNameInvalid = 20172,

        UserRoleNotExist = 20176,
        UserRoleAlreadyExist = 20177,
        UserTypeAlreadyExist = 20178,

        UserTypeNotExist = 20179,
        UserTypeNotAssignedForDefaultCompany = 20180,
        UserRoleNotAssignedForDefaultCompany = 20181,
        UserTypeUpdateMismatch = 20182,
        UserTypeNotAssignedForAdditionalCompany = 20183,
        #endregion

        #region TechnicalSpecialistCertificate 

        TsCertificationUpdatedByOther = 19002,
        TsCertificationIdDoesNotExist = 19003,
        TsCertificationUpdateRequestNotExist = 19004,
        TsCertificationIsBeingUsed = 19005,

        #endregion

        #region TechnicalSpecialist Training 

        TsTrainingUpdatedByOther = 19006,
        TsTrainingIdDoesNotExist = 19007,
        TsTrainingUpdateRequestNotExist = 19008,
        TsTrainingIsBeingUsed = 19009,

        #endregion

        #region TechnicalSpecialist Note 

        TsNoteIdDoesNotExist = 19050,

        #endregion

        #region TechnicalSpecialistInternalTraining

        TsInternalTrainingUpdatedByOther = 19051,
        TsInternalTrainingIdDoesNotExist = 19052,
        TsInternalTrainingUpdateRequestNotExist = 19053,
        TsInternalTrainingIsBeingUsed = 19054,
        TsInternalTrainingExpiryDate = 19015,

        #endregion

        #region TechnicalSpecialist Competency

        TsCompetencyUpdatedByOther = 19055,
        TsCompetencyIdDoesNotExist = 19056,
        TsCompetencyUpdateRequestNotExist = 19057,
        TsCompetencyIsBeingUsed = 19058,
        TsCompetencyExpiryDate = 19059,
        TsDVACharters=19060,

        #endregion

        #region TechnicalSpecialistTrainingAndCompetencyType

        TsIntTrainingAndCompetencyTypeUpdatedByOther = 19010,
        TsIntTrainingAndCompetencyTypeIdDoesNotExist = 19011,
        TsIntTrainingAndCompetencyTypeUpdateRequestNotExist = 19012,
        TsIntTrainingAndCompetencyTypeIsBeingUsed = 19013,

        #endregion

        #region TechnicalSpecialistTimeOffRequest
        TsInvalidLeaveCategory = 19014,
        #endregion

        #region Assignment

        AssignmentCompanyAddress = 20301,
        AssignmentSupplierPONumber = 20302,
        AssignmentCustomerContact = 20303,
        AssignmentContractHoldingCompany = 20304,
        AssignmentContractHoldingCompanyCoordinator = 20305,
        AssignmentOperatingCompanyCoordinator = 20306,
        AssignmentStatus = 20307,
        AssignmentType = 20308,
        AssignmentLifeCycle = 20309,
        AssignmentNotExists = 20310,
        AssignmentIsBeingUsed = 20311,
        AssignmentReviewAndModeration = 20312,
        AssignmentWithNumberNotExists = 20313,
        AssignmentFromDate = 20314,
        AssignmentWorkCountry = 20315,
        AssignmentWorkCounty = 20316,
        AssignmentFromTo = 20317,
        AssignmentProjectStartDate=20318,
        AssignmentBudgetValueExceedsProject = 20319,
        AssignmentBudgetHoursExceedsProject = 20320,

        #endregion

        #region AssignmentContractRateSchedule

        AssignmentContractRateScheduleNotExists = 21001,
        AssignmentContractRateScheduleAlreadyExist = 21002,
        AssignmentContractRateScheduleUpdateCountMisMatch = 21003,
        AssignmentContractRateScheduleDuplicateRecord = 21004,
        AssignmentContractRateScheduleInvalidAssignment = 21005,
        AssignmentContractRateScheduleInvalidContractSchedule = 21006,

        #endregion

        #region AssignmentContributionCalculation
        AssignmentContributionCalculationInvalidAssignment = 27000,
        AssignmentContributionCalculationNotExists = 27001,
        AssignmentContributionRevenueCostInvalidId = 27002,
        AssignmentContributionCalculationUpdateCountMismatch = 27003,
        #endregion

        #region AssignmentContributionRevenueCost

        AssignmentContributionRevenueCostNotExists = 27101,
        AssignmentContributionRevenueCostAlreadyExist = 27102,
        AssignmentContributionRevenueCostUpdateCountMisMatch = 27103,
        AssignmentContributionRevenueCostDuplicateRecord = 27104,
        AssignmentContributionRevenueCostInvalidContributionCalculation = 27105,

        #endregion

        #region AssignmentTechnicalSpecialistSchedule

        AssignmentTechnicalSpecialistScheduleNotExists = 21101,
        AssignmentTechnicalSpecialistScheduleAlreadyExist = 21102,
        AssignmentTechnicalSpecialistScheduleUpdateCountMisMatch = 21103,
        AssignmentTechnicalSpecialistScheduleInvalidPaySchedule = 21104,
        AssignmentTechnicalSpecialistScheduleInvalidAssignmentTechnicalSpecialist = 21105,
        AssignmentTechnicalSpecialistScheduleInvalidContractSchedule = 21106,
        AssignmentTechnicalSpecialistScheduleDuplicateRecord = 21107,
        AssignmentTechnicalSpecialistScheduleInvalidId = 21108,

        #endregion

        #region AssignmentTechnicalSpecialist
        AssignmentTechnicalSpecialistNotExists = 22001,
        AssignmentTechnicalSpecialistAlreadyExist = 22002,
        AssignmentTechnicalSpecialistInvalidAssignment = 22003,
        AssignmentTechnicalSpecialistInvalidTechnicalSpecialist = 22004,
        AssignmentTechnicalSpecialistDuplicateRecord = 22005,
        AssignmentTechnicalSpecialistUpdateCountMisMatch = 22006,
        AssignmentTechnicalSpecialistInvalidId = 22007,
        AssignmentTechnicalSpecialistIsBeingUsed = 22008,
        AssignmentTechnicalSpecialistContractScheduleAlreadyAssociated = 22009,
        #endregion

        #region AssignmentAdditionalExpense
        AssignmentAdditionalExpenseNotExists = 23001,
        AssignmentAdditionalExpenseAlreadyExist = 23002,
        AssignmentAdditionalExpenseUpdateCountMismatch = 23003,
        InvalidExpenseType = 23004,
        AssignmentAdditionalExpenseDuplicateRecord = 23005,
        InvalidOperatingCompanyCode = 23006,
        #endregion

        #region AssignmentInterCompanyDiscount
        InterCompanyDiscountInvalidCompanyCode = 23011,
        InterCompanyDiscountInvalidAssignment = 23012,
        DiscountSumExceeds = 23013,
        #endregion

        #region AssignmentRefrence
        AssignmentRefrenceNotExists = 24001,
        AssignmentRefrenceAlreadyExist = 24002,
        AssignmentRefrenceInvalidAssignment = 24003,
        AssignmentRefrenceInvalidRefrence = 24004,
        AssignmentRefrenceDuplicateRecord = 24005,
        AssignmentRefrenceUpdateCountMisMatch = 24006,
        #endregion

        #region AssignmentSubSupplierTechnicalSpecialist
        AssignmentSubSupplierTechnicalSpecialistNotExists = 25001,
        AssignmentSubSupplierTechnicalSpecialistAlreadyExist = 25002,
        AssignmentSubSupplierTechnicalSpecialistInvalidAssignment = 25003,
        AssignmentSubSupplierTechnicalSpecialistInvalidTechnicalSpecialist = 25004,
        AssignmentSubSupplierTechnicalSpecialistDuplicateRecord = 25005,
        AssignmentSubSupplierTechnicalSpecialistUpdateCountMisMatch = 25006,
        AssignmentSubSupplierTechnicalSpecialistInvalidSubSupplier = 25007,
        #endregion

        #region AssignmentSubSupplier
        AssignmentSubSupplierNotExists = 24051,
        AssignmentSubSupplierUpdateCountMisMatch = 24052,
        AssignmentSubSupplierDuplicateRecord = 24053,
        AssignmentSubSupplierInvalidSupplierContact = 24054,
        AssignmentSubSupplierInvalidMainSupplierContact = 24055,
        AssignmentSubSupplierInvalidTSPin = 24056,
        AssignmentSubSupplierTsCannotBeDeleted = 24057,
        AssignmentSubSupplierHasFirstVisit = 24058,
        AssignmentSubSupplierIdAndContact = 24059,

        #endregion
        //D661 issue 8 Start
        #region AssignmentNote 
        AssignmentNoteNotExists = 24059,
        #endregion
        //D661 issue 8 End
        #region MyTask

        MyTaskNotExists = 24100,
        MyTaskAlreadyExist = 24101,

        #endregion

        #region Assignment Taxonomy

        InvalidTaxonomyCategory = 28101,
        InvalidTaxonomySubCategory = 28102,
        InvalidTaxonomyService = 28103,
        TaxonomyDoesntExists = 28104,
        TaxonomyAlreadyExits = 28105,
        TaxonomyAlreadyUpdated = 28106,
        TaxonomyDuplicateRecord = 28107,
        #endregion

        #region Audit Log
        AuditModuleNotExists = 29001,
        #endregion

        #region Resource Search

        OperatingCompanyCodeRequired = 29002,
        ContractholdingCompanyCodeRequired = 29003,
        OperatingCompanyCoordinatorRequired = 29004,
        ContractholdingCompanyCoordinatorRequired = 29005,
        InvalidResourceSearchId = 29006,
        InvalidPreAssignmentId = 29007,

        OverrideResourceIdDoesNotExist = 29008,
        ResourceSearchUpdatedByOther = 29009,
        PreAssignmentAssigned= 29010,
        #endregion

        #region Email Subject
        ForgetPasswordEmailSubject = 30000,
        #endregion

        #region Calendar
        InvalidCalendarRecord = 40000,
        InvalidTimesheetCalendarRecord= 40001,
        VisitCalendarDataInOutsideRange=40002,
        TimesheetCalendarDataInOutsideRange = 40003,
        InvalidCalendarStarttime = 40004,

        #endregion
    }
}