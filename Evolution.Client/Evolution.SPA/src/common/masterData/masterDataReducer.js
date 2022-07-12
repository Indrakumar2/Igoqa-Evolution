import { masterDataActionTypes } from '../../constants/actionTypes';

const initialState = {
    countryMasterData: [],
    currencyMasterData: [],
    invoicePaymentTerms: [],
    referenceType: [],
    stateMasterData: [],
    cityMasterData: [],
    adminSchedule: [],
    adminInspectionGroup: [],
    adminInspectionType: [],
    adminChargeRate: [],
    businessUnit: [],
    marginType: [],
    industrySector: [],
    managedServiceType: [],
    documentTypeMasterData: [],
    coordinators:[],
    techSpecSubCategory:[],
    techSpecServices:[],
    techSpecCategory:[],
    expenseType:[],
    payrollName:[],
    attachmentTypeData: [], 
    logo:[],
    salutationMasterData: [],
    profileActionMasterData: [],
    technicalManager: [],
    resourceCoordinator:[],// Added for D496 CR 
    supplierPerformanceMasterData: [],
    profileStatus:[]
};

export default (state = initialState, actions) => {
    const { type, data } = actions;
    switch (type) {
        case masterDataActionTypes.FETCH_ATTACHMENT_TYPE_DATA:
            state = {
                ...state,
                attachmentTypeData: data
            };
            return state;
        case masterDataActionTypes.FETCH_DOCUMENT_TYPE_MASTER_DATA:
            state = {
                ...state,
                documentTypeMasterData: data
            };
            return state;
        case masterDataActionTypes.FETCH_COUNTRY_MASTER_DATA:
            state = {
                ...state,
                countryMasterData: data
            };
            return state;
        case masterDataActionTypes.FETCH_CURRENCY_MASTER_DATA:
            state = {
                ...state,
                currencyMasterData: data
            };
            return state;
        case masterDataActionTypes.FETCH_INVOICE_PAYMENT_TERMS:
            state = {
                ...state,
                invoicePaymentTerms: data,
            };
            return state;
        case masterDataActionTypes.FETCH_REFERENCE_TYPE:
            state = {
                ...state,
                referenceType: data,
            };
            return state;

        case masterDataActionTypes.FETCH_STATE_MASTER_DATA:
            state = {
                ...state,
                stateMasterData: data,
            };
            return state;

        case masterDataActionTypes.FETCH_CITY_MASTER_DATA:
            state = {
                ...state,
                cityMasterData: data,
            };
            return state;

        case masterDataActionTypes.CLEAR_STATE_CITY_DATA:
            state = {
                ...state,
                stateMasterData: [],
                cityMasterData: []
            };
            return state;

        case masterDataActionTypes.CLEAR_CITY_DATA:
            state = {
                ...state,
                cityMasterData: []
            };
            return state;

        case masterDataActionTypes.MASTER_CLEAR_DATA:
            state = {
                ...state,
                countryMasterData: [],
                stateMasterData: [],
                cityMasterData: [],
                salutationMasterData: [],
                documentTypeMasterData: [],
                invoicePaymentTerms: [],
                currencyMasterData: [],
                marginType: [],
                businessUnit: [],
                industrySector:[],
                managedServiceType:[], 
                techSpecCategory : [],
                expenseType: [],
                referenceType: []
            };
            return state;

        case masterDataActionTypes.FETCH_ADMIN_SCHEDULE:
            state = {
                ...state,
                adminSchedule: data,
            };
            return state;
        case masterDataActionTypes.FETCH_ADMIN_INSPECTION_GROUP:
            state = {
                ...state,
                adminInspectionGroup: data,
            };
            return state;
        case masterDataActionTypes.FETCH_ADMIN_INSPECTION_TYPE:
            state = {
                ...state,
                adminInspectionType: data,
            };
            return state;
        case masterDataActionTypes.FETCH_ADMIN_CHARGE_RATE:
            state = {
                ...state,
                adminChargeRate: data,
            };
            return state;
            case masterDataActionTypes.FETCH_EMPLOYMENT_STATUS:
            state ={
                ...state,
                employmentStatus:data
            };
            return state;
            case masterDataActionTypes.FETCH_TECHNICAL_DICIPLINE:
            state ={
                ...state,              
               technicalDicipline:data
            };
            return state;
            case masterDataActionTypes.FETCH_TECHNICAL_DICIPLINE_CATEGORY:
            state ={
                ...state,              
                techSpecCategory:data
            };
            return state;
            case masterDataActionTypes.FETCH_TECHNICAL_DICIPLINE_SUBCATEGORY:
            state ={
                ...state,              
                techSpecSubCategory:data
            };
            return state;
            case masterDataActionTypes.FETCH_TECHNICAL_DICIPLINE_SERVICES:
            state ={
                ...state,              
                techSpecServices:data
            };
            return state; 
            case masterDataActionTypes.FETCH_TAXONOMY_INTERNAL_TRAINING:
            state ={
                ...state,              
                taxonomyInternalTraining:data
            };             
            return state;  
            case masterDataActionTypes.FETCH_TAXONOMY_COMPETENCY:
            state ={
                ...state,              
                taxonomyCompetency:data
            };             
            return state;
            case masterDataActionTypes.FETCH_TAXONOMY_CUSTOMER_APPROVED:
            state ={
                ...state,              
                taxonomyCustomerApproved:data
            };             
            return state; 
            case masterDataActionTypes.FETCH_TAXONOMY_CUSTOMER_APPROVED_COMMODITY:
            state ={
                ...state,              
                taxonomyCustomerCommodity:data
            };             
            return state;            
            case masterDataActionTypes.FAILED_FETCH_COMPUTER_KNOWLEDGE:
            state ={
                ...state,              
                computerKnowledge:data
            }; 
            return state; 
            case masterDataActionTypes.FAILED_FETCH_LANGUAGES:
            state ={
                ...state,              
                languages:data
            }; 
            return state; 
            case masterDataActionTypes.FAILED_FETCH_CERTIFICATES:
            state ={
                ...state,              
                certificates:data
            }; 
            return state; 
            case masterDataActionTypes.FAILED_FETCH_TRAININGS:
            state ={
                ...state,              
                training:data
            }; 
            return state; 
            case masterDataActionTypes.FAILED_FETCH_EQUIPMENT:
            state ={
                ...state,              
                equipment:data
            }; 
            return state; 
            case masterDataActionTypes.FAILED_FETCH_COMMODITY:
            state ={
                ...state,              
                commodity:data
            }; 
            return state;
            case masterDataActionTypes.FAILED_FETCH_CODESTANDARD:
            state ={
                ...state,              
                codeStandard:data
            }; 
            return state;
            case masterDataActionTypes.FAILED_FETCH_SUBDIVISION:
            state ={
                ...state,              
                subDivision:data
            }; 
            return state;
            case masterDataActionTypes.FAILED_FETCH_PROFILESTATUS:
            state ={
                ...state,              
                profileStatus:data
            }; 
            return state;
            case masterDataActionTypes.FAILED_FETCH_COMPANIES:
            state ={
                ...state,              
                companies:data
            }; 
            return state;
            case masterDataActionTypes.CLEAR_SUB_CATEGORY:          
            state = {
                ...state,
                techSpecSubCategory: [],
                techSpecServices: []
            };
            return state;
            case masterDataActionTypes.FETCH_LANGUAGE:
            state = {
                ...state,
                techSpecLanguage: []
            };
            return state;
            case masterDataActionTypes.FETCH_COMPETENCY:
            state = {
                ...state,
                techSpecCompetency: []
            };
            return state;
            case masterDataActionTypes.FETCH_INTERNALTRANINING:
            state = {
                ...state,
                techSpecInternalTraning: []
            };
            return state;
            case masterDataActionTypes.FETCH_CUSTOMERCOMMODITIES:
            state = {
                ...state,
                techSpecCustomerCommdities: []
            };
            return state;
            case masterDataActionTypes.FETCH_TECHNICALSPECIALISTCUSTOMER:
            state = {
                ...state,
                techSpecCustomer: []
            };
            return state;
            case masterDataActionTypes.FETCH_PAYROLLTYPES:
            state = {
                ...state,
                techSpecPayrollType: []
            };
            return state;
            case masterDataActionTypes.CLEAR_SERVICE:
            state = {
                ...state,
                techSpecServices: []
            };
            return state;
        case masterDataActionTypes.FETCH_BUSINESS_UNIT_SUCCESS:
            state = {
                ...state,
                businessUnit: data,
            };
            return state;
        case masterDataActionTypes.FETCH_MARGIN_TYPE_SUCCESS:
            state = {
                ...state,
                marginType: data,
            };
            return state;
        case masterDataActionTypes.FETCH_INDUSTRY_SECTORS_SUCCESS:
            state = {
                ...state,
                industrySector: data
            };
            return state;
        case masterDataActionTypes.FETCH_MANAGED_SERVICES:
            state = {
                ...state,
                managedServiceType: data
            };
            return state;
            case  masterDataActionTypes.FETCH_PAYROLLNAME:
            state = {
                ...state,
                payrollName:  data,
            };
            return state;
            case  masterDataActionTypes.FETCH_EXPENSETYPE:
            state = {
                ...state,
                expenseType:  data,
            };
            return state;
        case masterDataActionTypes.FETCH_LOGO_SUCCESS:
            state = {
                ...state,
                logo:data
            };
            return state;
        case masterDataActionTypes.FETCH_COORDINATOR_SUCCESS:
            state = {
                ...state,
                coordinators:data
            };
            return state;
        case masterDataActionTypes.SALUTATION:
            state = {
                ...state,
                salutationMasterData: data
            };
            return state;
        case masterDataActionTypes.PROFILE_ACTION:
            state = {
                ...state,
                profileActionMasterData: data
            };
            return state;
        case masterDataActionTypes.TECHNICAL_MANAGER:
            state = {
                ...state,
                technicalManager: data
            };
            return state;
            // Started for D496 CR 
        case masterDataActionTypes.FETCH_RESOURCE_COORDINATOR:
                state = {
                    ...state,
                    resourceCoordinator: data
                };
            return state;
            //End for D496 CR
            case masterDataActionTypes.FETCH_CUSTOMER:
            state = {
                ...state,
                customerList: data
            };
            return state;
            case masterDataActionTypes.CLEAR_CUSTOMER:
            state = {
                ...state,
                customerList: []
            };
            return state;

            case masterDataActionTypes.CLEAR_ADMIN_CHARGE_RATES:
            state = {
                ...state,
                adminChargeRate:[]
            };
            return state;
            case masterDataActionTypes.GET_VIEW_ALL_RIGHTS_COMPANIES:
                state = {
                    ...state,
                    viewAllRightsCompanies: data
                };
            return state;
        default:
            return state;
    }
};