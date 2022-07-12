import { FetchData } from '../../services/api/baseApiService';
import { RequestPayload, masterData,customerAPIConfig,userAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../baseComponents/intertekToaster';
import { masterDataActionTypes } from '../../constants/actionTypes';
import { getlocalizeData, isEmpty, isEmptyReturnDefault,isUndefined,isEmptyOrUndefine,parseValdiationMessage } from '../../utils/commonUtils';
import { sortCurrency } from '../../utils/currencyUtil';
import authService from '../../authService';
import { IsGRMMasterDataFetched,IsARSMasterDataLoaded,ShowLoader,HideLoader } from '../../common/commonAction';
import { ChargeRates } from '../../actions/contracts/rateScheduleAction';
import arrayUtil from '../../utils/arrayUtil';
import { ClearRefreshMasterData } from '../../components/header/headerAction';

const localConstant = getlocalizeData();

const actions = {
    FetchCountry: (payload) => ({
        type: masterDataActionTypes.FETCH_COUNTRY_MASTER_DATA,
        data: payload 
    }),
    FetchCurrency: (payload) => ({
        type: masterDataActionTypes.FETCH_CURRENCY_MASTER_DATA,
        data: payload
    }),
    FetchInvoicePaymentTerms: (payload) => ({
        type: masterDataActionTypes.FETCH_INVOICE_PAYMENT_TERMS,
        data: payload
    }),
    FetchReferenceType: (payload) => ({
        type: masterDataActionTypes.FETCH_REFERENCE_TYPE,
        data: payload
    }),

    FetchState: (payload) => ({
        type: masterDataActionTypes.FETCH_STATE_MASTER_DATA,
        data: payload
    }),
    FetchCity: (payload) => ({
        type: masterDataActionTypes.FETCH_CITY_MASTER_DATA,
        data: payload
    }),
    ClearStateCityData: () => ({
        type: masterDataActionTypes.CLEAR_STATE_CITY_DATA
    }),
    ClearCityData: () => ({
        type: masterDataActionTypes.CLEAR_CITY_DATA
    }),
    ClearMasterData: () => ({
        type: masterDataActionTypes.MASTER_CLEAR_DATA
    }),
    FetchAdminSchedule: (payload) => ({
        type: masterDataActionTypes.FETCH_ADMIN_SCHEDULE,
        data: payload
    }),
    FetchAdminInspectionGroup: (payload) => ({
        type: masterDataActionTypes.FETCH_ADMIN_INSPECTION_GROUP,
        data: payload
    }),
    FetchAdminInspectionType: (payload) => ({
        type: masterDataActionTypes.FETCH_ADMIN_INSPECTION_TYPE,
        data: payload
    }),
    FetchAdminChargeRates: (payload) => ({
        type: masterDataActionTypes.FETCH_ADMIN_CHARGE_RATE,
        data: payload
    }),
    FetchEmploymentStatus: (payload) => ({
        type: masterDataActionTypes.FETCH_EMPLOYMENT_STATUS,
        data: payload
    }),
    FetchTechnicalDicipline: (payload) => ({
        type: masterDataActionTypes.FETCH_TECHNICAL_DICIPLINE,
        data: payload
    }),
    FetchTechSpecCategory: (payload) => ({
        type: masterDataActionTypes.FETCH_TECHNICAL_DICIPLINE_CATEGORY,
        data: payload
    }),
    FetchTechSpecSubCategory: (payload) => ({
        type: masterDataActionTypes.FETCH_TECHNICAL_DICIPLINE_SUBCATEGORY,
        data: payload
    }),
    FetchTechSpecServices: (payload) => ({
        type: masterDataActionTypes.FETCH_TECHNICAL_DICIPLINE_SERVICES,
        data: payload
    }),
    FetchTaxonomyInternalTraining: (payload) => ({
        type: masterDataActionTypes.FETCH_TAXONOMY_INTERNAL_TRAINING,
        data: payload
    }),
    FetchTaxonomyCompetency: (payload) => ({
        type: masterDataActionTypes.FETCH_TAXONOMY_COMPETENCY,
        data: payload
    }),
    FetchTaxonomyCustomerApproved: (payload) => ({
        type: masterDataActionTypes.FETCH_TAXONOMY_CUSTOMER_APPROVED,
        data: payload
    }), 
    FetchTaxonomyCustomerApprovedCommodity: (payload) => ({
        type: masterDataActionTypes.FETCH_TAXONOMY_CUSTOMER_APPROVED_COMMODITY,
        data: payload
    }),    
    FetchComputerKnowledge: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_COMPUTER_KNOWLEDGE,
        data: payload
    }),
    FetchLanguages: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_LANGUAGES,
        data: payload
    }),
    FetchCertificates: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_CERTIFICATES,
        data: payload
    }),
    FetchTrainings: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_TRAININGS,
        data: payload
    }),
    FetchEquipment: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_EQUIPMENT,
        data: payload
    }),
    FetchCommodity: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_COMMODITY,
        data: payload
    }),
    FetchCodeStandard: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_CODESTANDARD,
        data: payload
    }),
    FetchSubDivision: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_SUBDIVISION,
        data: payload
    }),
    FetchProfilestatus: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_PROFILESTATUS,
        data: payload
    }),
    FetchCompanies: (payload) => ({
        type: masterDataActionTypes.FAILED_FETCH_COMPANIES,
        data: payload
    }),
    fetchBusinessUnitSuccess: (payload) => ({
        type: masterDataActionTypes.FETCH_BUSINESS_UNIT_SUCCESS,
        data: payload
    }),
    fetchBusinessUnitError: (error) => ({
        type: masterDataActionTypes.FETCH_BUSINESS_UNIT_ERROR,
        error: error
    }),
    fetchMarginTypeSuccess: (payload) => ({
        type: masterDataActionTypes.FETCH_MARGIN_TYPE_SUCCESS,
        data: payload
    }),
    fetchMarginTypeError: (error) => ({
        type: masterDataActionTypes.FETCH_MARGIN_TYPE_ERROR,
        error: error
    }),
    FetchIndustrySectorsSuccess: (payload) => ({
        type: masterDataActionTypes.FETCH_INDUSTRY_SECTORS_SUCCESS,
        data: payload
    }),
    FetchIndustrySectorsError: (error) => ({
        type: masterDataActionTypes.FETCH_INDUSTRY_SECTORS_ERROR,
        error: error
    }),
    FetchManagedServices: (payload) => ({
        type: masterDataActionTypes.FETCH_MANAGED_SERVICES,
        data: payload
    }),
    FetchDocumentTypeMasterData: (payload) => ({
        type: masterDataActionTypes.FETCH_DOCUMENT_TYPE_MASTER_DATA,
        data: payload
    }),
    ClearSubCategory: (payload) => ({
        type: masterDataActionTypes.CLEAR_SUB_CATEGORY,
        data: payload
    }),
    ClearServices: (payload) => ({
        type: masterDataActionTypes.CLEAR_SERVICE,
        data: payload
    }),
    FetchLanguage: (payload) => ({
        type: masterDataActionTypes.FETCH_LANGUAGE,
        data: payload
    }),
    FetchCompetency: (payload) => ({
        type: masterDataActionTypes.FETCH_COMPETENCY,
        data: payload
    }),
    FetchInternaltrainings: (payload) => ({
        type: masterDataActionTypes.FETCH_INTERNALTRANINING,
        data: payload
    }),
    FetchCustomerCommodities: (payload) => ({
        type: masterDataActionTypes.FETCH_CUSTOMERCOMMODITIES,
        data: payload
    }),
    FetchTechnicalSpecialistCustomers: (payload) => ({
        type: masterDataActionTypes.FETCH_TECHNICALSPECIALISTCUSTOMER,
        data: payload
    }),
    FetchPayrollTypes: (payload) => ({
        type: masterDataActionTypes.FETCH_PAYROLLTYPES,
        data: payload
    }),
    FetchExpenseType: (payload) => ({
        type: masterDataActionTypes.FETCH_EXPENSETYPE,
        data: payload
    }),
    FetchPayRollName: (payload) => ({
        type: masterDataActionTypes.FETCH_PAYROLLNAME,
        data: payload
    }),
    
    FetchAttachmentTypeData: (payload) => ({
        type: masterDataActionTypes.FETCH_ATTACHMENT_TYPE_DATA,
        data: payload
    }),
    FetchLogoSuccess: (payload) => ({
        type: masterDataActionTypes.FETCH_LOGO_SUCCESS,
        data: payload
    }),
    FetchLogoError: (error) => ({
        type: masterDataActionTypes.FETCH_LOGO_ERROR,
        error: error
    }),
    FetchCoordinatorSuccess: (payload) => ({
        type: masterDataActionTypes.FETCH_COORDINATOR_SUCCESS,
        data: payload
    }),
    FetchCoordinatorError: (error) => ({
        type: masterDataActionTypes.FETCH_COORDINATOR_ERROR,
        data: error
    }),
    FetchSalutation: (payload) => ({
        type: masterDataActionTypes.SALUTATION,
        data: payload
    }),
    FetchProfileAction: (payload) => ({
        type: masterDataActionTypes.PROFILE_ACTION,
        data: payload
    }),
    FetchTechnicalManager: (payload) => ({
        type: masterDataActionTypes.TECHNICAL_MANAGER,
        data: payload
    }),
    FetchResourceCoordinator: (payload) => ({  // Changes for D496 CR 
        type: masterDataActionTypes.FETCH_RESOURCE_COORDINATOR,
        data: payload
    }),
    FetchCustomerList: (payload) => ({
        type: masterDataActionTypes.FETCH_CUSTOMER,
        data: payload
    }),
    ClearCustomerList: (payload) => ({
        type: masterDataActionTypes.CLEAR_CUSTOMER,
        data: payload
    }),
    ClearAdminChargeRates:() =>({
        type:masterDataActionTypes.CLEAR_ADMIN_CHARGE_RATES,
    }),
    getViewAllRightsCompanies:(payload) => ({
        type: masterDataActionTypes.GET_VIEW_ALL_RIGHTS_COMPANIES,
        data: payload
    }),  
};

//On refresh click
export const ReloadMasterData = () => (dispatch) => {
    //dispatch(ClearData());
    dispatch(ClearRefreshMasterData()) ;
    dispatch(FetchCountry({ isFromRefresh : true })); 
    dispatch(FetchSalutation()); 
    dispatch(FetchDocumentTypeMasterData());
    dispatch(FetchInvoicePaymentTerms({ isFromRefresh : true }));
    dispatch(FetchCurrency());
    dispatch(FetchMarginType());
    dispatch(FetchBusinessUnit());
    dispatch(FetchIndustrySectors());
    dispatch(FetchManagedServices());
    dispatch(FetchTechSpecCategory());
    dispatch(FetchAdminSchedule(true));
    dispatch(FetchExpenseType()); 
    dispatch(FetchReferenceType({ isFromRefresh : true }));   
};

//To Fetch all master data at once.
export const FetchAllMasterData = () => (dispatch) => {
    dispatch(FetchCountry()); 
    dispatch(FetchCurrency());
    dispatch(FetchReferenceType());
    dispatch(FetchLanguages());
    dispatch(FetchInvoicePaymentTerms());
    dispatch(FetchBusinessUnit());
    dispatch(FetchMarginType());
    dispatch(FetchIndustrySectors());
    dispatch(FetchManagedServices());
    dispatch(FetchDocumentTypeMasterData());
    //dispatch(FetchAttachmentTypeData());
    dispatch(FetchSalutation()); 
};
//Grm Master data 
export const grmDetailsMasterData = () => (dispatch) => {
    //GRM MasterData
    dispatch(FetchInvoicePaymentTerms());
    dispatch(FetchEmploymentStatus());   
    dispatch(FetchTaxonomyInternalTraining());
    dispatch(FetchTaxonomyCompetency());
    dispatch(FetchTaxonomyCustomerApproved());    //used in Quick Search Module
    dispatch(FetchComputerKnowledge());   
    dispatch(FetchCertificates()); //used in Quick Search Module
    dispatch(FetchTrainings());
  //  dispatch(FetchEquipment()); //used in Quick Search,PreAssigment and TechnicalSpecialist Module Itself
    dispatch(FetchCommodity());
    dispatch(FetchCodeStandard());
    dispatch(FetchSubDivision());
    dispatch(FetchProfilestatus());
   // dispatch(FetchCompanies());    // Not used in TS Module,Quick Search and PreAssignment
    //dispatch(FetchPayRollName());
    dispatch(FetchExpenseType());    
    dispatch(FetchSalutation()); 
   // dispatch(FetchProfileAction()); //Changes For Sanity Def 116
   // dispatch(FetchTechnicalManager()); // Used in TS Module, Sent To DropDown Change Event
    dispatch(FetchTechSpecCategory()); //used in Quick Search Module
    /**
     * isGrmMasterDataFeteched method which will be set as true once the
     * GRM master data is loaded. For subsequent GRM related menus we check for this flag to load master data
     * If user goes out of GRM/Resource module NEED set the flag to false to reload masterdata 
     */
    dispatch(IsGRMMasterDataFetched(true));
};

/** Loading ARS Master Data */
export const arsMasterData = () => (dispatch) => {
    dispatch(FetchCertificates());
    dispatch(FetchTaxonomyCustomerApproved());    
    dispatch(FetchEquipment());
    dispatch(IsARSMasterDataLoaded(true));
};

export const grmFetchCustomerData=()=>(dispatch)=>{
    dispatch(FetchCustomerList());  
};

export const grmSearchMatsterData = () => (dispatch) => {
    dispatch(FetchTechSpecCategory());  
    dispatch(FetchEmploymentStatus());   
};
export const clearCustomerList = () => (dispatch) => {
    dispatch(actions.ClearCustomerList());  
};

export const FetchCustomerList = (data) => async (dispatch) => {
    dispatch(actions.FetchCustomerList(null));
    let apiUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails + '?';
    apiUrl += "Active=YES ";
    apiUrl = apiUrl.slice(0, -1);
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(apiUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast genDetActCustLst');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchCustomerList(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast genDetActCustLstSmtingWrong');
    }
};

export const FetchSalutation = () => async (dispatch, getstate) => {  
    if (!isEmptyOrUndefine(getstate().masterDataReducer.salutationMasterData)) {
        return false;
    }
    const isActive=true;  
    const salutationMasterData = masterData.baseUrl + masterData.salutation;
    const params = {
        isActive:isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(salutationMasterData, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.SALUTATION_MASTER_DATA, 'dangerToast SalutationMstrDt');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchSalutation(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast SalutationMstrDtSmtWrng');
    }
};

/**
 * Fetch Assignment Type Data
 */
export const FetchAttachmentTypeData = () => async (dispatch,getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.attachmentTypeData)) {
        return false;
    }
        const documentTypeMasterData = masterData.baseUrl + masterData.documentType;
        const params = {
            'moduleName': 'Visit'
        };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(documentTypeMasterData, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster('Attachment Type Data not fetched', 'dangerToast invDefActAttachTypeMstrDt');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchAttachmentTypeData(response.result));
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast invDefActAttachTypeMstrDtSmtWrng');
        }
    };

/**
 * Fetch Document Type Master Data
 */
export const FetchDocumentTypeMasterData = () => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.documentTypeMasterData)) {
        return false;
    }
    const documentTypeMasterData = masterData.baseUrl + masterData.documentType;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(documentTypeMasterData, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.DOCUMENT_TYPE_MASTER_DATA, 'dangerToast DocTypeMstrDt');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchDocumentTypeMasterData(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast DocTypeMstrDtSmtWrng');
    }
};

export const ClearMasterData = () => (dispatch) => {
   dispatch(actions.ClearMasterData());
};

//Country Master Data
export const FetchCountry = (data) => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.countryMasterData)) {
        return false;
    }
    const countryMasterData = masterData.baseUrl + masterData.country;
    const params = {};
    if (data) {
        params.isFromRefresh = data.isFromRefresh;
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(countryMasterData, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Fetch country master data went wrong', 'dangerToast mstDtActCntry');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchCountry(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast mstDtActCntrySmtWrng');
    }
};

//State Master Data
export const FetchState = (data) => async (dispatch, getstate) => {
    const stateMasterData = masterData.baseUrl + masterData.state;
    let response = {};
    if(isEmpty(data)){
        dispatch(actions.FetchState());
    } // D-367
     else {
    const params = { 'country': data };
    const requestPayload = new RequestPayload(params);
     response = await FetchData(stateMasterData, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Fetch state master data went wrong', 'dangerToast FetchState');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchState(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchState');
    }
  }
  return response;
};

// Added For ITK DEf 1536
export const FetchStateId = (data) => async(dispatch,getstate) => {
    const state = getstate();
    const stateMasterData = masterData.baseUrl + masterData.state;
    let response = {};
    if(data){
        const params = {
            'countryId': data  
        };
        const requestPayload = new RequestPayload(params);
         response = await FetchData(stateMasterData, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.STATE_FETCH_VALIDATION, 'warningToast supplierStateFetchAddressVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
            if (response && response.code === "1") {
                dispatch(actions.FetchState(response.result));
            }
            else {
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchState');
            }
    } else {
        dispatch(actions.FetchState());
    }
    return response;
};

//City Master Data
export const FetchCity = (data) => async (dispatch, getstate) => {
    const cityMasterData = masterData.baseUrl + masterData.city;
    let response = {};
    if(isEmpty(data)){
        dispatch(actions.FetchCity());
    } // D-367
    else {
    const params = { 'state':data };
    const requestPayload = new RequestPayload(params);
    response = await FetchData(cityMasterData, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Fetch city master data went wrong', 'dangerToast FetchCity');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchCity(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchCity');
    }
  }
  return response;
};
// Added For ITK DEf 1536
export const FetchCityId = (data) => async(dispatch,getstate) => {
    const state = getstate();
    const cityMasterData = masterData.baseUrl + masterData.city;
    let response = {};
    if(data){
        const params = { 'stateId':data };
        const requestPayload = new RequestPayload(params);
        response = await FetchData(cityMasterData, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster('Fetch city master data went wrong', 'dangerToast FetchCity');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchCity(response.result));
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast FetchCity');
        }
    } 
    else { 
        dispatch(actions.FetchCity());
    }
    return response;
};

//Clear state and city data
export const ClearStateCityData = () => (dispatch) => {
    dispatch(actions.ClearStateCityData());
};
//Clear city data
export const ClearCityData = () => (dispatch) => {
    dispatch(actions.ClearCityData());
};
//Currency
export const FetchCurrency = () => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.currencyMasterData)) {
        return false;
    }
    const currency = masterData.baseUrl + masterData.currencies;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(currency, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Fetch Currency Master data went wrong', 'dangerToast mstDtActCurrncy');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const filteredCurrency =sortCurrency(response.result);
        dispatch(actions.FetchCurrency(filteredCurrency));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast mstDtActCurrncySmtWrng');
    }
};

/** Update Master Data Currency */
export const UpdateMasterCurrency = () => (dispatch,getstate) => {
    const state = getstate();
    const currency = isEmptyReturnDefault(state.masterDataReducer.currencyMasterData);
    const filteredCurrency = sortCurrency(currency);
    dispatch(actions.FetchCurrency(filteredCurrency));
};

//Invoice Payment Terms Master Data
export const FetchInvoicePaymentTerms = (data) => async (dispatch,getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.invoicePaymentTerms)) {
        return false;
    }
    const isActive=true;
    const invoicePaymentTerms = masterData.baseUrl + masterData.paymentterms;
    const params = {
        isActive:isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    if (data) {
        params.isFromRefresh = data.isFromRefresh;
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(invoicePaymentTerms, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Invoice payment Terms not fetched', 'dangerToast mstDtActInvPayTrms');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchInvoicePaymentTerms(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast mstDtActInvPayTrmsSmtWrng');
    }
};

//Reference Type Master Data
export const FetchReferenceType = (data) => async (dispatch,getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.referenceType)) {
        return false;
    }
    const isActive=true;
    const referenceType = masterData.baseUrl + masterData.assignmentReference;
    const params = {
        isActive:isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    if (data) {
        params.isFromRefresh = data.isFromRefresh;
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(referenceType, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Reference Type not fetched', 'dangerToast mstDtActRefType');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchReferenceType(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast mstDtActRefTypeSmtWrng');
    }
};

/**
 * Fetch all admin rate schedules.
 */
export const FetchAdminSchedule = (isActive) => async (dispatch, getstate) => {
    const state = getstate();
    const selectedCompanyCode = state.appLayoutReducer.selectedCompany;
    dispatch(actions.FetchAdminSchedule([]));
    const adminScheduleUrl = masterData.baseUrl + masterData.adminSchedule;
    const params = {
        companyCode:selectedCompanyCode,
        isActive: isActive   //Changes for Defect 112
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(adminScheduleUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Admin Schedule API not responding', 'dangerToast mstDtActAdmSch');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchAdminSchedule(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast mstDtActAdmSchSmtWrng');
    }
};

/**
 * Fetch inspection group based on selected admin schedule name. - For Rate Schedule
 * Fetch all admin inspection group - For Admin
 * @param {*Object / String} data 
 */
export const FetchAdminInspectionGroup = (data,isActive) => async (dispatch, getstate) => {
    let params = {};
    const state=getstate();
    const selectedCompanyCode = state.appLayoutReducer.selectedCompany;
    dispatch(actions.FetchAdminInspectionGroup([]));
    if (data) {
        dispatch(actions.FetchAdminInspectionType([]));
        dispatch(actions.FetchAdminChargeRates([]));
        params = {
            'companyChargeScheduleName': data,
            'companyCode':selectedCompanyCode,     //For Fetching Particular Company Inspection Groups
            'isActive': isActive //Changes for Defect 112
        };
    }
    if (data === "") {
        dispatch(actions.FetchAdminInspectionType([]));
        dispatch(actions.FetchAdminInspectionGroup([]));
        dispatch(ChargeRates([]));  
    }
    else {
        const adminInspectionGroupUrl = masterData.baseUrl + masterData.adminSchedule + masterData.adminInspectionGroup;
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(adminInspectionGroupUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster('Admin Inspection Group API not responding', 'dangerToast mstDtActAdmInsGrp');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchAdminInspectionGroup(response.result));
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast mstDtActAdmInsGrpSmtWrng');
        }
    }
};

/**
 * Fetch inspection type base on selected inspection group and schedule name.
 * Fetch all inspection type - For Admin
 * @param {*Object} grpName 
 * @param {*Object} schName 
 */
export const FetchAdminInspectionType = (grpName, schName,isActive) => async (dispatch, getstate) => {
    let params = {};
    const state=getstate();
    const selectedCompanyCode = state.appLayoutReducer.selectedCompany;
    dispatch(actions.FetchAdminInspectionType([]));
    if (grpName) {
        dispatch(actions.FetchAdminChargeRates([]));
        dispatch(ChargeRates([]));  
        params = {
            'companyChgSchInspectionGroupName': grpName,
            'companyChgSchName': schName,
            'isActive': isActive, //Changes for Defect 112
            'companyCode':selectedCompanyCode    //For Fetching Particular Company Inspection Types
        };
    }
    if (grpName === "") {
        dispatch(actions.FetchAdminInspectionType([]));
        dispatch(ChargeRates([]));  
    }
    else {
        const adminInspectionTypeUrl = masterData.baseUrl + masterData.adminSchedule + masterData.adminInspectionType;
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(adminInspectionTypeUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster('Admin Inspection Type not responding', 'dangerToast mstDtActAdmInsType');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (response && response.code === "1") {
            dispatch(actions.FetchAdminInspectionType(response.result));
        }
        else {
            IntertekToaster('Admin Inspection Type not responding', 'dangerToast mstDtActAdmInsTypeSmtWrng');
        }
    }
};

/**
 * Fetch Admin charge rates based on selected inspection type.
 * Fetch all admin charge rates - For Admin
 * @param {*Object} data 
 */
export const FetchAdminChargeRates = (data) => async (dispatch, getstate) => {
    let params = {};
    const state=getstate();
    const selectedCompanyCode = state.appLayoutReducer.selectedCompany;
    dispatch(ShowLoader());
    dispatch(actions.FetchAdminChargeRates([]));
    if (data) {
        if (data.companyChgSchInspGrpInspectionTypeName === "") {
            dispatch(ChargeRates([]));  
            dispatch(HideLoader());
            return null;
        }

        params = {
            'companyChgSchInspGrpInspectionTypeName': data.companyChgSchInspGrpInspectionTypeName,
            'companyChgSchInspGroupName':data.companyChgSchInspGroupName,
            'companyChargeScheduleName':data.companyChargeScheduleName,
            'companyCode':selectedCompanyCode    //For Fetching Particular Company Admin Charge Rates
        };
    }
    const adminChargeRatesUrl = masterData.baseUrl + masterData.adminChargeRates;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(adminChargeRatesUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster('Admin Charge Rate API not responding', 'dangerToast mstDtActAdmChrRt');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        }); 
    if (response && response.code === "1") {
        dispatch(actions.FetchAdminChargeRates(response.result));
        dispatch(ChargeRates(response.result));   
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast mstDtActAdmChrRtSmtWrng');
    }
    dispatch(HideLoader());
    return response; 
};

/**
 * Fetch Business Units
 */
export const FetchBusinessUnit = (reqParams) => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.businessUnit)) {
        return false;
    }
    const isActive=true;
    const businessUnitUrl = masterData.baseUrl + masterData.businessUnit;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(businessUnitUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            dispatch(actions.fetchBusinessUnitError(error));
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_BUSINESS_UNITS, 'dangerToast FetchBusinessUnit');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code === "1") {
            dispatch(actions.fetchBusinessUnitSuccess(response.result));
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {

        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_BUSINESS_UNITS, 'dangerToast FetchCustomerList');
    }
};

/**
 * Fetch Margin Type
 */
export const FetchMarginType = (reqParams) => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.marginType)) {
        return false;
    }
    const isActive=true;
    const marginTypeUrl = masterData.baseUrl + masterData.marginType;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(marginTypeUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            dispatch(actions.fetchMarginTypeError(error));
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_MARGIN_TYPE, 'dangerToast FetchMarginType');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });

    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code === "1") {
            dispatch(actions.fetchMarginTypeSuccess(response.result));
        }
        else if (response.code === "41") {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {

        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_MARGIN_TYPE, 'dangerToast FetchMarginType');
    }
};

//Fetch GRM
//todo
export const FetchComputerKnowledge = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    if (!isEmptyOrUndefine(getstate().masterDataReducer.computerKnowledge)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.computerKnowledge;
    const isActive=true;
    const params = {
        isActive:isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_COMPUTER_KNOWLEDGE, 'dangerToast computerKnowledge');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchComputerKnowledge(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_COMPUTER_KNOWLEDGE, 'dangerToast computerKnowledge');
    }
};
export const FetchLanguages = () => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.languages)) {
        return false;
    }
    const isActive=true;
    const Url = masterData.baseUrl + masterData.languages;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CERTIFICATES, 'dangerToast certificates');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const languages = response.result.sort(function(a,b){
            if(a.name > b.name )
                return 1;
            else if(a.name < b.name)
                return -1;
            else 
                return 0;    
        });
        dispatch(actions.FetchLanguages(languages));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CERTIFICATES, 'dangerToast certificates');
    }
};
export const FetchCertificates = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    if (!isEmptyOrUndefine(getstate().masterDataReducer.certificates)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.certificates;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CERTIFICATES, 'dangerToast certificates');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const certificates = arrayUtil.sort(response.result,'name','asc');
        dispatch(actions.FetchCertificates(certificates));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CERTIFICATES, 'dangerToast certificates');
    }
};
export const FetchTrainings = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    if (!isEmptyOrUndefine(getstate().masterDataReducer.training)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.trainings;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TRAININGS, 'dangerToast trainings');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const trainings = arrayUtil.sort(response.result,'name','asc');
        dispatch(actions.FetchTrainings(trainings));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TRAININGS, 'dangerToast trainings');
    }
};
export const FetchEquipment = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    // const Url = masterData.baseUrl + masterData.equipment;
    // if (!isEmptyOrUndefine(getstate().masterDataReducer.equipment)) {
    //     return false; //Changes for ITK - D1501
    // }
    const Url = masterData.baseUrl +masterData.commodityEquipment;
    const isActive=true;
    const params = {
        "commodity":data,
        isActive:isActive //Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_EQUIPMENT, 'dangerToast equipment');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        const equipment = arrayUtil.sort(response.result,'equipment','asc'); //Changes For D1240
        dispatch(actions.FetchEquipment(equipment));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_EQUIPMENT, 'dangerToast equipment');
    }
};
export const FetchCommodity = (data) => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.commodity)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.commodity;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_COMMODITY, 'dangerToast commodity');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
          dispatch(actions.FetchCommodity(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_COMMODITY, 'dangerToast commodity');
    }
};
export const FetchCodeStandard = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    if (!isEmptyOrUndefine(getstate().masterDataReducer.codeStandard)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.codeStandard;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url,  requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CODESTANDARD, 'dangerToast codeStandard');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchCodeStandard(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CODESTANDARD, 'dangerToast codeStandard');
    }
};
export const FetchSubDivision = (data) => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.subDivision)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.subdivision;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, 'GET', requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PROFILESTATUS, 'dangerToast profilestatus');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchSubDivision(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PROFILESTATUS, 'dangerToast profilestatus');
    }
};
export const FetchProfilestatus = (data) => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.profileStatus)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.profilestatus;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PROFILESTATUS, 'dangerToast profilestatus');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchProfilestatus(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PROFILESTATUS, 'dangerToast profilestatus');
    }
};
export const FetchCompanies = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    const Url = masterData.baseUrl + masterData.companies;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_COMPANIES, 'dangerToast companies');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        // const selectedCompanyCode = configuration.selectedCompany;
        // const selectedCompanyCurrency = response.result.filter(item => item.companyCode === selectedCompanyCode)[0].currency;
        // localStorage.setItem(applicationConstants.Authentication.COMPANY_CURRENCY, selectedCompanyCurrency);
        dispatch(actions.FetchCompanies(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_COMPANIES, 'dangerToast companies');
    }
};
export const FetchEmploymentStatus = (data) => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.employmentStatus)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.employmenttype;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_EMPLOYMENT_STATUS, 'dangerToast employmentStatus');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
          
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchEmploymentStatus(response.result));
    }
    else {
         IntertekToaster(localConstant.errorMessages.FAILED_FETCH_EMPLOYMENT_STATUS, 'dangerToast employmentStatus');
    }
};
export const FetchTechnicalDicipline = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    const Url = 'https://demo2784692.mockable.io/tecnicalDiscipline';
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECHNICAL_DICIPLINE, 'dangerToast technicalDicipline');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.data.code === "1") {
        dispatch(actions.FetchTechnicalDicipline(response.data.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECHNICAL_DICIPLINE, 'dangerToast technicalDicipline');
    }
};
export const FetchTechSpecCategory = (data) => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.techSpecCategory)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.taxonomyCategory;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_CATEGORY, 'dangerToast TechSpecCategory');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchTechSpecCategory(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_CATEGORY, 'dangerToast TechSpecCategory');
    }
};

export const FetchTechSpecSubCategory = (data,isId) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    const Url = masterData.baseUrl + masterData.taxonomySubCategory;
    const params = {};
    if (isId)
        params.taxonomyCategoryId = data;
    else
        params.taxonomyCategory = data;
        
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_SUBCATEGORY, 'dangerToast TechSpecSubCategory');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        
    if (response && response.code === "1") {
        dispatch(actions.FetchTechSpecSubCategory(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_SUBCATEGORY, 'dangerToast TechSpecSubCategory');
    }
};
export const FetchTechSpecServices = (category,subCategory,isId) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    const Url = masterData.baseUrl + masterData.taxonomyServices;
    const params = {};
    if (isId){
        params.taxonomyCategoryId = category;
        params.taxonomySubCategoryId = subCategory;
    }
    else{
        params.taxonomyCategory = category;
        params.taxonomySubCategory = subCategory;
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_SERVICES, 'dangerToast TechSpecServices');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchTechSpecServices(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECH_DICIPLINE_SERVICES, 'dangerToast TechSpecServices');
    }
};

export const FetchLanguage = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    const Url = masterData.baseUrl + masterData.languages;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_LANGUAGE, 'dangerToast TechSpecLanguage');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchLanguage(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_LANGUAGE, 'dangerToast TechSpecLanguage');
    }
};
export const FetchTaxonomyCompetency = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    if (!isEmptyOrUndefine(getstate().masterDataReducer.taxonomyCompetency)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.competency;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_COMPETENCY, 'dangerToast Competency');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchTaxonomyCompetency(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_COMPETENCY, 'dangerToast Competency');
    }
};
export const FetchTaxonomyInternalTraining = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    if (!isEmptyOrUndefine(getstate().masterDataReducer.taxonomyInternalTraining)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.internaltrainings;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_INTERNALTRANINING, 'dangerToast Internaltrainings');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchTaxonomyInternalTraining(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_INTERNALTRANINING, 'dangerToast Internaltrainings');
    }
};
export const FetchTaxonomyCustomerApprovedCommodity = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    const Url = masterData.baseUrl + masterData.customerCommodities;
    const params = { 'customerCode':data };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMERCOMMODITIES, 'dangerToast CustomerCommodities');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchTaxonomyCustomerApprovedCommodity(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMERCOMMODITIES, 'dangerToast CustomerCommodities');
    }
};
export const FetchTaxonomyCustomerApproved = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    if (!isEmptyOrUndefine(getstate().masterDataReducer.taxonomyCustomerApproved)) {
        return false;
    }
    const Url = masterData.baseUrl + masterData.technicalSpecialistCustomers;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECHNICALSPECIALISTCUSTOMER, 'dangerToast TechnicalSpecialistCustomers');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchTaxonomyCustomerApproved(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_TECHNICALSPECIALISTCUSTOMER, 'dangerToast TechnicalSpecialistCustomers');
    }
};
export const FetchPayrollTypes = (data) => async (dispatch, getstate) => {
    //const Url = masterData.baseUrl + masterData.currencies;
    const Url = masterData.baseUrl + masterData.payrollTypes;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PAYROLLTYPES, 'dangerToast PayrollTypes');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
       
    if (response && response.code === "1") {
        dispatch(actions.FetchPayrollTypes(response.result));
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_PAYROLLTYPES, 'dangerToast PayrollTypes');
    }
};

//Clear SubCategory
export const ClearSubCategory = () => (dispatch) => {
    dispatch(actions.ClearSubCategory());
};
//Clear Services
export const ClearServices = () => (dispatch) => {
    dispatch(actions.ClearServices());
};

export const FetchIndustrySectors = () => async(dispatch,getstate) =>{
    if (!isEmptyOrUndefine(getstate().masterDataReducer.industrySector)) {
        return false;
    }
    const isActive=true;
    const industrySectorUrl = masterData.industrySector;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(industrySectorUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            dispatch(actions.FetchIndustrySectorsError(error));
            // IntertekToaster(localConstant.validationMessage.INDUSTRY_SECTOR_VALIDATION, 'warningToast industrySectorFetchVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchIndustrySectorsSuccess(response.result));
        }
        else if (response.code == 41) {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {

        }
    }
    else {
        IntertekToaster(`${ localConstant.validationMessage.INDUSTRY_SECTOR_VALIDATION }. ${ localConstant.validationMessage.CONTACT_SYS_ADMIN }`, 'dangerToast industrySectorSWWVal');
    }
};

export const FetchManagedServices = () => async (dispatch, getstate) => {
    if (!isEmptyOrUndefine(getstate().masterDataReducer.managedServiceType)) {
        return false;
    }
    const isActive=true;
    const managedServicesUrl = masterData.managedServiceTypes;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(managedServicesUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.MANAGED_SERVICE_TYPES_VALIDATION, 'warningToast managedServiceTypesFetchVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchManagedServices(response.result));
        }
        else if (response.code == 41) {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {

        }
    }
    else {
        IntertekToaster(`${ localConstant.validationMessage.MANAGED_SERVICE_TYPES_VALIDATION }. ${ localConstant.validationMessage.CONTACT_SYS_ADMIN }`, 'dangerToast managedServiceTypesSWWVal');
    }
};
export const FetchPayRollName = () => async (dispatch) => {
    const payrate = masterData.baseUrl + masterData.payrolls;
    const params = {};
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(payrate, requestPayload)

        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster( localConstant.validationMessage.PAYROLL_NAME_VALIDATION, 'dangerToast ');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        dispatch(actions.FetchPayRollName(response.result));
    }
    else {
       
        IntertekToaster( localConstant.techSpec.common.WENT_WRONG, 'dangerToast ');
    }
};
export const FetchExpenseType = () => async (dispatch,getstate) => {
    const expenseType = isEmptyReturnDefault(getstate().masterDataReducer.expenseType);
    if(expenseType.length === 0 || isUndefined(expenseType)){
        const payrate = masterData.baseUrl + masterData.chargeTypes;
        const isActive=true;
        const params = {
            isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
        };
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(payrate, requestPayload)
    
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster( localConstant.validationMessage.PAYRATE_VALIDATION, 'dangerToast ');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
           
        if (response && response.code === "1") {
            dispatch(actions.FetchExpenseType(response.result));
        }
        else {
            IntertekToaster( localConstant.techSpec.common.WENT_WRONG, 'dangerToast ');
        }
    }
};

export const FetchModuleLogo = (codeName) => async (dispatch) => {
    const logoUrl = masterData.baseUrl + masterData.logo;
    const isActive=true;
    const params = {
        code:codeName,
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(logoUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.LOGO_VALIDATION, 'warningToast ModuleLogoFetchVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(actions.FetchLogoError(error));
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchLogoSuccess(response.result));
        }
        else if (response.code == 41) {
            if (!isEmptyReturnDefault(response.validationMessages)) {
                //To-Do: Common approach to display validationMessages
            }
        }
        else if (response.code === "11") {
            if (!isEmptyReturnDefault(response.messages)) {
                //To-Do: Common approach to display messages
            }
        }
        else {

        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.LOGO_VALIDATION, 'warningToast ModuleLogoFetchVal');
    }
};

/** Fetch Coordinator action */
export const FetchCoordinator = (data) =>async (dispatch,getstate) =>{
    const state = getstate();
    const selectedCompany = getstate().appLayoutReducer.selectedCompany;
    const coordinatorUrl = masterData.user;
    const param = {
        companyCode: selectedCompany
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(coordinatorUrl,requestPayload)
        .catch(error=>{
            // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION,'wariningToast coordinatorVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if(!isEmpty(response)&&!isEmpty(response.code)){
            if(response.code == 1){
                dispatch(actions.FetchCoordinatorSuccess(response.result));
            }
            else if (response.code === "41") {
                if (!isEmptyReturnDefault(response.validationMessages)) {
                    //To-Do: Common approach to display validationMessages
                 }
            }
            else if(response.code === "11"){
                if (!isEmptyReturnDefault(response.messages)) {
                       //To-Do: Common approach to display messages
                }
            }
            else{
    
            }
        }
        else{
            IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast coordinatorSWWVal');
        }
};

//Fetch Profile action
export const FetchProfileAction = () => async (dispatch, getstate) => {
    const profileActionUrl = masterData.baseUrl + masterData.profileActions;
    const isActive=true;
    const params = {
        isActive:isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(profileActionUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.validationMessage.TECHNICAL_MANAGER_VALIDATION, 'dangerToast mstDtActProfileAction');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (response && response.code === "1") {
        dispatch(actions.FetchProfileAction(response.result));
    }
    else {
        IntertekToaster(localConstant.techSpec.common.WENT_WRONG, 'dangerToast mstDtActProfileActionSmtWrng'); 
    }
    return true; //Changes For Sanity Def 116
};

/** Fetch Technical Manager action */
export const FetchTechnicalManager = (data) => async (dispatch, getstate) => {
    const state = getstate();
    const selectedCompany = getstate().appLayoutReducer.selectedCompany;
    const TMUrl = masterData.baseUrl + masterData.technicalManagers;
    const userTypeTM=localConstant.techSpec.userTypes.TM;
    const param = {
        companyCode: selectedCompany,
        userType : userTypeTM,
        isActive : true, //IGO_QC 854 
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(TMUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'wariningToast coordinatorVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code) && response.code == 1) {  
            dispatch(actions.FetchTechnicalManager(response.result)); 
    }
    else {
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast TMListVal');
    }
};
/** Fetch Resource Coordinator action */ // Added for D496 CR 
export const FetchResourceCoordinator = (data) => async (dispatch, getstate) => {
    const state = getstate();
    const selectedCompany = state.appLayoutReducer.selectedCompany;
    const Url = masterData.baseUrl + masterData.technicalManagers;
    const userTypeRC=localConstant.techSpec.userTypes.RC;
    const param = {
        companyCode: selectedCompany,
        userType : userTypeRC
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(Url, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.COORDINATOR_VALIDATION, 'wariningToast coordinatorVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code) && response.code == 1) {  
            dispatch(actions.FetchResourceCoordinator(response.result)); 
    }
    else {
        IntertekToaster(localConstant.validationMessage.SOME_THING_WENT_WRONG, 'dangerToast RCListVal');
    }
};

export const ClearAdminChargeRates = () => (dispatch) =>{
    dispatch(actions.ClearAdminChargeRates());
};

export const ClearAdminContractRates = () => (dispatch) => {
    dispatch(actions.FetchAdminInspectionType([]));
    dispatch(actions.FetchAdminInspectionGroup([]));
    dispatch(ChargeRates([]));
};

export const getViewAllRightsCompanies = () => async (dispatch, getstate) => {
    const state = getstate();
    const userData = authService.getUserDetails();
    const requestURL = userAPIConfig.getViewAllRightsCompanies;
    const params = {
        'samAccountName': userData.unique_name
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(requestURL, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_VIEW_ALL_RIGHTS_COMPANIES_FAILED, 'dangerToast viewallRigthsCompaniesFailed');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
            return false;
        });
    if (!isEmpty(response)) {
        if (response.code === "1") {
           dispatch(actions.getViewAllRightsCompanies(response.result));
           return isEmptyReturnDefault(response.result).length;
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast viewallRigthsCompaniesFailed');
            return false;
        }
        else {
            IntertekToaster(localConstant.errorMessages.FETCH_VIEW_ALL_RIGHTS_COMPANIES_FAILED, 'dangerToast viewallRigthsCompaniesFailed');
            return false;
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_VIEW_ALL_RIGHTS_COMPANIES_FAILED, 'dangerToast viewallRigthsCompaniesFailed');
        return false;
    }
};