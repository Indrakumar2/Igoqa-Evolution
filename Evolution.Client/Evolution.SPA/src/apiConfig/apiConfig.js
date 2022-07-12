import { configuration } from '../appConfig';

export function RequestPayload(data = {}, headers = {}) {
    this.data = data;
    this.headers = headers;
}

export const homeAPIConfig = {
    homeBaseURL: configuration.apiBaseUrl,
    dashBoardCount: 'dashboard?IsRecordCountOnly=true&AssignmentStatus=P',
    assignments: 'assignments?',
    inActivessignments: 'assignments/inactive',
    visitStatus: 'visits',
    contract: 'contracts',
    project:'projects',
    assignmentDocumentApproval:'assignments/DocumentApproval',
    supplierPO:'supplierPOs',
    timesheet: 'timesheets',
    getTimeSheetDocumentApproval:'/GetTimesheetForDocumentApproval',
    getVisitDocumentApproval:'/GetVisitForDocumentApproval',
    contractBudget: 'contracts/budgets',
    projectBudget: 'projects/budgets',
    assignmentBudget: 'assignments/budgets',
    fetchBudget: 'Home/budgets',
    myTasks:'Home/TS/mytasks?assignedTo=',
    documentsForApproval:'documentApproval?IsForApproval=true&status=C',
    documentType:'module/document/types',
    documentApproval:'documentApproval',
    getCustomerName:'/GetCustomerName',
    techSpechDashboardMessage:'companies/{0}/invoicedetail',
    documentation:'master/documentation',
    myTasksAssignUsers:'Home/TS/myTasksAssignUsers',
    mytasksReassign: 'Home/TS/mytasksReassign',
    mytasksReassignARS: 'Home/RSEARCH/mytasksReassign',
    dashboardCompanyList:'companies/GetCompanyList'
};

export const companyAPIConfig = {
    companyBaseURL: configuration.apiBaseUrl,
    companyDetails: 'companies',
    companyContractDetail: 'contracts/searchCompanyContracts?contractHoldingCompanyCode',
    companyDetail: '/detail',
    costSaleReference: 'expense/types',
    currencies: 'currencies?isActive=true',
    companyPlaceholder: 'email/placeholders',
    companyOffices: 'offices',
    companyDocuments: 'CompanyDocuments?moduleCode=COMP&moduleRefCode=',
    expectedMargins:'/companies/{0}/expectedMargins'
};

export const customerAPIConfig = {
    custBaseUrl: configuration.apiBaseUrl,
    customer: 'customers/',
    customerDetail: 'detail',
    customerDetails: 'customers',
    custContractDetail: 'contracts?contractCustomerCode=',
    custContracts:'contracts/customerContracts',
    custProjectDetail: 'projects/GetSelectiveProjectData?contractCustomerCode=',
    customerContractDocuments: 'contracts/documents/GetCustomerContractDocuments?customerId=',
    projects: 'projects/',
    documents: 'documents',
    customerDocuments: 'CustomerDocuments?moduleCode=CUST&moduleRefCode=',
    getProjectDocuments: '/GetCustomerProjectDocuments?customerId=',
    customerContacts:'/customers/{0}/contacts',
    customerReference:'/customers/{0}/assignmentreferences',
    //added for Reports
    customerCoordinator:'/customers/customerCoordinator',
    visitTimesheetCustomers:'/customers/visitTimesheetCustomers',
    visitTimesheetKPICustomers:'/customers/visitTimesheetKPICustomers',
    visitTimesheetContracts:'/contracts/visitTimesheetContracts',
    visitTimesheetUnApprovedContracts: '/contracts/visitTimesheetUnApprovedContracts',
    visitTimesheetKPIContracts: '/contracts/visitTimesheetKPIContracts',
    visitTimesheetKPIProjects: '/projects/GetProjectKPI',
    visitTimesheetUnapprovedCustomers:'/customers/visitTimesheetUnapprovedCustomers',
    serverDownloadReport: '/Batch/GetGeneratedReport',
    getExportTechnicalspecalistCVASZIP:'Batch/getexporttechnicalspecalistCVasZIP',
    deleteReportFiles: '/Batch/DeleteReportFiles',
    downloadServerFile: '/UserReport/DownloadServerFile',
};
export const commonAPIConfig = {
    baseUrl: configuration.apiBaseUrl,
    documents: 'documents',
    download: '/download?documentUniqueName=',
    paste: '/Paste',
    uniqueName: '/UniqueName',
    uploadFileAsStream: '/UploadFileAsStream?documentUniqueName=',
    changeStatus: '/ChangeStatus',
    gooogleMap: 'directions',
    emailTemplate:'/GetTemplate',
};
export const contractAPIConfig = {
    contractBaseUrl: configuration.apiBaseUrl,
    api: 'api/',
    detail: 'detail',
    companies: 'companies/',
    customers: 'customers/',
    contracts: 'contracts/',
    contractSearch: 'contracts',
    searchContracts: 'contracts/searchContracts',
    projects: 'projects',
    contacts: '/contacts',
    getContacts: '/GetContacts', // Optimization
    addresses: '/addresses',
    getAddress: '/GetAddress', // Optimization
    invoicedetail: '/invoicedetail',
    taxes: '/taxes',
    rateSchedule: 'schedules',
    rates: '/rates',
    documents: 'documents',
    moduleCode: '/CON',
    contractCode: '/CON/',
    TempData: '/temp',
    code: '?code=',
    copy: '/Copy',
    notes: '/notes',
    contractNumber: '?contractNumber=',
    parentContractNumber: '?parentContractNumber=',
    paste: '/Paste',
    uniqueName: '/UniqueName',
    uploadFileAsStream: '/UploadFileAsStream?referenceCode=',
    changeStatus: '/ChangeStatus',
    download: '/download?documentUniqueName=',
    contractDocuments: 'ContractDocuments?moduleCode=CNT&moduleRefCode=',
    contractDocumentsOfProject: 'contracts/documents/GetContractSpecificDocuments',
    contractRates:'/contracts/rates?ContractNumber=',     //Added for Contract Rates Expired Validation
    isRFCExists:'/contracts/RFCExists',
    isDuplicateFrameworkSchedules:'contracts/DuplicateFrameworkSchedules',
    rfcCopyBatch:'/batch',
    getBatchStatus:'/getBatchStatus',
};

export const projectAPIConfig = {
    baseUrl: configuration.apiBaseUrl,
    projectSearch: 'projects',
    detail: '/detail',
    company: 'company/',
    companies: 'companies/',
    division: 'divisions/',
    divisions: 'divisions',
    costCenter: 'costcenters',
    offices: 'offices',
    assignments: 'assignments',
    supplierPo: 'supplierPOs',
    documents: '/documents',
    document: 'documents',
    moduleCode: '/PRG',
    projectCode: '/PRG/',
    projectCopy: 'Copy',
    TempData: '/temp',
    code: '?code=',
    customers: 'customers/',
    contracts: 'contracts/',
    contractNumber: '?contractNumber=',
    user: 'users',
    getSelectiveProjectData: '/GetSelectiveProjectData',
    projectDocuments: 'ProjectDocuments?moduleCode=PRJ&moduleRefCode=',
    projectDocumentsofContracts: 'projects/documents/GetContractProjectDocuments?contractNumber=',
    projectNotifications:'/projects/{0}/notifications'
};

//Added For Supplier
export const supplierAPIConfig = {
    supplierBaseUrl: configuration.apiBaseUrl,
    supplier:'suppliers/',
    fetchSupplier:'suppliers/{0}/detail',
    supplierSearch:'suppliers',
    detail:'detail',
    supplierDocumentsDelete:'SupplierDocuments?moduleCode=SUP&moduleRefCode={0}'
};
export const timesheetAPIConfig={ 
    timesheetsBaseUrl: configuration.apiBaseUrl,
    timesheets:'timesheets',
    searchTimesheets:'/timesheets/SearchTimesheets',
    GetTimesheets:'/GetTimesheets',
    techSpecRateSchedules:"/assignments/{0}/technicalSpecialists/schedules/GetTechSpecRateSchedule",
    technicalSpecialist:'/timesheets/{0}/technicalSpecialist',
    technicalSpecialistGrossmargin:'/timesheets/{0}/detail/TechnicalSpecialistWithGrossMargin',
    references:'/timesheets/{0}/referencetypes',
    documents:'/documents',
    notes:'/timesheets/{0}/notes',
    timesheetInfo:'/timesheet?timesheetId={0}',
    technicalSpecialistTime: '/timesheets/{0}/technicalSpecialistAccountItemTime',
    technicalSpecilistExpense: '/timesheets/{0}/technicalSpecialistAccountItemExpense',
    technicalSpecialistTravel: '/timesheets/{0}/technicalSpecialistAccountItemTravel',
    technicalSpecialistConsumable: '/timesheets/{0}/technicalSpecialistAccountItemConsumables',
    detail:'/timesheets/{0}/detail',
    getAssignmentTimesheetDocuments:'/timesheets/documents/GetAssignmentTimesheetDocuments',
    TechSpecRateSchedule: '/assignments/{0}/technicalSpecialists/schedules/GetTechSpecRateSchedule',
    approve:'/timesheets/{0}/detail/approve',
    reject:'/timesheets/{0}/detail/reject',
    customerReportNotification:'/timesheets/{0}/detail/CustomerReportNotification',
    GetTimesheetValidationData: '/timesheets/GetTimesheetValidationData',
    GetExpenseChargeExchangeRates:'/GetExpenseChargeExchangeRates',
    timesheetDocumentDelete:'documents?moduleCode=TIME&moduleRefCode={0}'
};
export const visitAPIConfig={
    visitBaseUrl:configuration.apiBaseUrl,
    visits:'visits',
    GetVisits:'/GetVisits',
    GetSearchVisits:'/GetSearchVisits',
    GetVisitByID: '/GetVisitByID?visitId={0}',
    GetHistoricalVisits: '/GetHistoricalVisits?visitAssignmentId={0}',
    visitStatus: 'visitstatus',
    unusedReason: 'unusedReason',
    // Detail: '/detail',
    Detail:'/visits/{0}/detail',
    InterCompanyDiscounts: '/intercompanydiscounts',
    VisitInterCompanyDiscounts: '/visits/{0/intercompanydiscounts',
    Documents: '/documents',
    Notes: '/{0}/notes',
    SupplierPerformance: '/{0}/supplierperformance',
    TechnicalSpecialist: '/{0}/technicalspecialistsaccounts',
    VisitReference: '/{0}/references',
    visitReferenceTypes:'/invoice/referencetypes',    
    assignment:'assignments/',
    visitDocuments:'visits/documents/GetVisitDocuments',
    technicalSpecialistList: '/technicalSpecialists',
    supplierList: '/subsuppliers',
    TechSpecRateSchedule: '/assignments/{0}/technicalSpecialists/schedules/GetTechSpecRateSchedule',
    technicalSpecialistTime: '/visits/{0}/technicalSpecialistAccountItemTime',
    technicalSpecilistExpense: '/visits/{0}/technicalSpecialistAccountItemExpense',
    technicalSpecialistTravel: '/visits/{0}/technicalSpecialistAccountItemTravel',
    technicalSpecialistConsumable: '/visits/{0}/technicalSpecialistAccountItemConsumables',
    technicalSpecialistGrossmargin:'/visits/{0}/detail/TechnicalSpecialistWithGrossMargin',
    GetFinalVisitId: '/GetFinalVisitId',    
    GetVisitValidationData: '/GetVisitValidationData',
    AssignmentDocuemnts: 'Assignmentdocuments?moduleCode=ASGMNT&moduleRefCode={0}',
    approve:'/visits/{0}/detail/approve',
    reject:'/visits/{0}/detail/reject',
    subSupplierVisit:'assignments/{0}/subsuppliers/GetSubSupplierForVisit',
    customerReportNotification:'/visits/{0}/detail/CustomerReportNotification', 
    visitDocumentsDelete:'/VisitDocuments?moduleCode=VST&moduleRefCode={0}',    
    UploadEmailDocuments: 'documents/UploadEmailDocuments'
};

export const assignmentAPIConfig = {    
    assignmentStatus: 'AssignmentStatus',
    assignmentType: 'AssignmentType',
    assignmentLifeCycle: 'assignments/lifecycles',
    supplierPO: 'GetSupplierPO',
    assignmentSupplierPOLOV: 'GetAssignmentSupplierPO', // Optimization
    assignment:'Assignments/Search',
    assignmnetReferenceTypes:'/invoice/referencetypes',
    contractDocumentsOfAssignment: 'contracts/documents/GetContractSpecificDocuments',
    projectDocumentsOfAssignment:'projects/documents/GetProjectSpecificDocuments',
    supplierPoDocumentsOfAssignment:'SupplierPO/documents/GetAssignmentSupplierPOocuments',
    //supplierInforamation tab API added 
    supplierContact:'suppliers/{0}/contacts',
    subSuppliers:'supplierpos/{0}/subsuppliers',
    mainSupplier:'supplierPOs',
    //supplierInforamation tab API  ended
    assignmentId:'?assignmentId=',
    techSpec:'technicalSpecialists/',
    payRateSchedule:'payRateSchedules',
    assignmentPaySchedule:'technicalSpecialists/{0}/payRateSchedules',
    assignmentContractRates:'contracts/schedules/{0}/rates',
    assignmentPayRates:'technicalSpecialists/{0}/PayRateSchedules/{1}/PayRates',
    visitDocumentsOfAssignment:'visits/documents/GetAssignmentVisitDocuments',
    timesheetDocumentsOfAssignment:'timesheets/documents/GetAssignmentTimesheetDocuments',
    assignmentDetail:'/assignments/{0}/detail',
    visitStatus: 'visitstatus',
    assignmentInstruction: '/assignments/{0}/instructions',
    assignmentReference: '/assignments/{0}/referencetypes',
    additionalExpenses:'/assignments/{0}/additionalExpenses',
    assignmentsInfo:'/assignments/GetAssignments?assignmentId=',
    taxonomyBusinessUnit:'/TaxonomyBusinessUnits',
    assignmentContractScheduleRates:'contracts/schedules/rates', // ITK D-714
    assignmentDocuments: 'AssignmentDocuments?moduleCode=ASGMNT&moduleRefCode=',
    assignmentReviewAndModerationProcess:'/reviewandmoderation/process',
    projectAssignmentCreation:'projects/GetProject',
    // assignmentTaxonomyUrl:'systemSetting'
};

export const GrmAPIConfig = {
    grmBaseUrl: configuration.apiBaseUrl,
    technicalspecialist: 'technicalspecialist/',
    technicalSpecialists:'TechnicalSpecialists/',
    technicalSpecilists:'technicalSpecilists/',
    quickSearch:'quicksearch',
    quickSearchsSearch:'quickSearchs/search',
    detail: 'detail',
    drafts: '{0}/{1}/drafts',
    search:'Search',
    draft:'/draft',
    info:'Info',
    basicInfo:'BasicInfo',
    techSpecDocuments: 'technicalSpecilistDocuments?moduleCode=TS&moduleRefCode=',
    subModuleRefCode:'&subModuleRefCode=',
    timeoffrequest:'0/TimeOffRequests' , 
    preAssignmentSave:'preAssignmentSearch',
    mySearch:'Home/mySearches',
    preAssignmentTSSearch:'preAssignmentSearchs/Search',
    preAssignmentGeoLocation:'preAssignmentSearchs/GeoLocation',
    arsSearch:'ARS/Assignment/Info',
    arsResourceNotes:'ARS/Notes',
    ARS:'ARS',
    technicalSpecialistCalendar:'TechnicalSpecialistCalendar',
    technicalSpecialistCalendarSearch:'TechnicalSpecialistCalendar/SearchGet',
    getCalendarByTechnicalSpecialistId:'TechnicalSpecialistCalendar/GetCalendarByTechnicalSpecialistId',
    exportCV:'ExportCV',
    generateBatchforExportCV : 'GenerateBatchForExportCV',
    DocumentApi : 'documents/',
    exportCVasZIP:'exportCVasZIP',
    intertekWorkHistoryReport:'IntertekWorkHistoryReport',
  /** Won Lost Reports API CONFIG */
    wonLostReports:'reports/wonLost',
    /** D684 */
    taxonomyHistory:'{0}/IsTaxonomyHistoryExists',
    companySpecificMatrixReport:'companySpecificMatrixReport',
    companySpecificMatrixReportExport:'companySpecificMatrixReport/export',
    taxonomyReport:'taxonomyReport',
    calendarScheduleDetails:'calendarScheduleDetails',
};

export const masterData = {
    baseUrl: configuration.apiBaseUrl,
    state: 'states',
    country: 'countries',
    city: 'cities',
    assignmentReference: 'assignments/referencetypes',
    documentType: 'module/document/types',
    moduleName: '?moduleName=',
    contract: 'contract',
    salutation: 'salutations',
    prefix: 'euvatprefixes',
    tax: 'taxes',
    businessUnit: 'projecttype', // Used in companyAction.js
    marginType: 'company/margintypes', 
    divisionName: 'divisions',
    payrolls: 'payrolls',
    exportPrefixes: 'payroll/exportprefixes',
    paymentterms: 'paymentterms',
    chargeTypes: 'expense/types',
    currencies: 'currencies?isActive=true',
    profileActions: 'master/profileaction',
    adminSchedule: 'charges/schedules',
    adminInspectionGroup: '/inspection/groups',
    adminInspectionType: '/inspection/types',
    adminChargeRates: 'charges/Inspection/types/rates',
    logo: 'logo',
    industrySector: 'industrysectors',
    managedServiceTypes:'managedservicetypes',
    computerKnowledge:'computerKnowledge',
    languages:'Languages',
    certificates:'certificates',
    trainings:'trainings',
    equipment:'equipment',
    commodityEquipment:'CommodityEquipment',
    commodity:'commodity',
    codeStandard:'codeStandard',
    subdivision:'subdivision',
    profilestatus:'profilestatus',
    companies:'companies',
    employmenttype:'employmenttype',
    tecnicalDiscipline:'tecnicalDiscipline',
    taxonomyCategory:'TaxonomyCategory',
    taxonomySubCategory:'TaxonomySubCategory',
    taxonomyServices:'TaxonomyServices',
    competency:'Competency',
    internaltrainings:'Internaltrainings',
    customerCommodities:'CustomerCommodities',
    technicalSpecialistCustomers:'TechnicalSpecialistCustomers',
    payrollTypes:'payroll/types',
    user: 'users',
    timeoffcategory:'timeoffrequest/category',
    technicalManagers: 'userBasicInfo',
    /**PreAssignment dispositionType */
    dispositionType:'dispositionTypes',
    supplierPerformanceType: 'supplierperformancetype',
    miCoordinators: 'users/micoordinators',
    miCoordinatorsStatus: 'users/micoordinators/status',
    coordinators: 'users/coordinators',
    reportCoordinators:'users/reportCoordinators', //D-1385
    exchangeRate:'/GetExchangeRates',
    ssrsCoordinators:'users/ssrsCoordinators',
    techSpechStampCountryCode:'/TechSpechStampCountryCodes',
    systemSettings:'systemSetting'
};

export const loginApiConfig = {
    authLogin: `${ configuration.apiBaseUrl }authenticate`,
    logOutUrl: `${ configuration.apiBaseUrl }token/access/revoke`,
    refreshToken: `${ configuration.apiBaseUrl }token/renew`,
    userMenu: `${ configuration.apiBaseUrl }user/{userSamaName}/menu`,
    userType: `${ configuration.apiBaseUrl }user/{userSamaName}/usertype`,
    securityQuestion:`${ configuration.apiBaseUrl }user/security/question?`,
    validateAnswer:`${ configuration.apiBaseUrl }user/security/answer/validate?`,
    signInByQuestion:`${ configuration.apiBaseUrl }authenticate/byquestion`,
    resetPassword:`${ configuration.apiBaseUrl }authenticate/resetpassword`,
};
//Added for Supplier Po
export const supplierPOApiConfig = {
    supplierPoBaseUrl: configuration.apiBaseUrl,
    supplierPo:'supplierpos/',
    supplierSearch:'suppliers',
    detail:'/detail',
    //To-Do:Aswani:-HAve to change Url
    visitDocumentsOfSupplierPo: 'visits/documents/GetSupplierPoVisitDocuments',
    supplierPOID:'?supplierPOId=',
    supplierPOSearch:'supplierPOs',
    supplierPODetails:'supplierPOs/{0}/detail',
    subSupplier:'/subsuppliers',
    supplierPODocuments: 'SupplierPODocuments?moduleCode=SUPPO&moduleRefCode=',
};

//Security
export const roleAPIConfig = {
    roleBaseUrl:configuration.apiBaseUrl,
    Role:'security/roles',
    Module: 'security/modules',
    Activity: 'security/activities',
    ModuleActivity: 'security/module/activities',
    RoleActivity: 'security/role/activities',
    AnnonceMents:'announcements'
};

export const userAPIConfig={
    userBaseUrl:configuration.apiBaseUrl,
    user:'security/users',
    role:'security/roles',
    userDetail:'security/userdetails',
    userRoleCompany:'security/user/rolecompany',
    userPermission:'security/user/{userSamaName}/permission',
    companyOffice:'companies/{companyCode}/offices',
    menuRightsCompany:'users/{0}/usercompanyroles/{1}',
    getViewAllRightsCompanies:'users/GetViewAllAssignments'
};

export const auditAPIConfig={
    auditBaseUrl:configuration.apiBaseUrl,
    audit:'audit/Search',
    module:'audit/Module' ,
    auditEvent:"audit/AuditEvent",
    auditLog:"audit/AuditLog"
};
export const adminAPIConfig={
    adminBaseUrl:configuration.apiBaseUrl,
    assignmentLifecycle:'api/master/{masterdatatypeId}/data'
};
//Evolution Reports
export const EvolutionReportsAPIConfig={
    reportBaseUrl:configuration.apiBaseUrl,
    EvolutionReportBaseUrl: 'Report/DownloadReport',
    AssignmentInterCompanyInstructionReports:'AssignmentInterCompanyInstructionReportNoTS',
    SupplierVisitPerformanceReport:'SupplierPerformanceReport',
    AssignmentTechSpecInstructionReport:'AssignmentTechSpecInstructionReport',
    AssignmentTechSpecInstructionReportFileName:'Assignment Resource Instruction',
    Visit:'Visit',
    Timesheet:'Timesheet',
    UnapprovedVisitsReport:'UnapprovedVisitsReport',
    ApprovedVisitsReport:'ApprovedVisitsReportExcel',
    ApprovedVisitsReportExcelNDT: 'ApprovedVisitsReportExcelNDT',
    VisitsKPIReport:'VisitsKPIReport',
    UnapprovedTimesheetReport:'UnapprovedTimesheetReport',
    ApprovedTimesheetReport:'ApprovedTimesheetsReportExcel',
    ApprovedTimesheetReportNDT:'ApprovedTimesheetsReportExcelNDT',
    TimesheetKPIReport:'TimesheetsKPIReport',
    ProjectBasedOnStatus:'projects/GetProjectBasedOnStatus?'
};
export const BatchAPIConfig = {
    getBatch: '/getBatch',
};