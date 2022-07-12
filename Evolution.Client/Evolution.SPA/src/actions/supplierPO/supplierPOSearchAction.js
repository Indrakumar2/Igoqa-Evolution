import { supplierPOApiConfig, RequestPayload, supplierAPIConfig, projectAPIConfig } from '../../apiConfig/apiConfig';
import { supplierPOActionTypes } from '../../constants/actionTypes';
import { FetchData } from '../../services/api/baseApiService';
import { ShowLoader, HideLoader, UpdateCurrentPage,
    UpdateCurrentModule } from '../../common/commonAction';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { isEmpty, getlocalizeData, isEmptyReturnDefault, parseValdiationMessage } from '../../utils/commonUtils';
import { HandleMenuAction } from '../../components/sideMenu/sideMenuAction';
const localConstant = getlocalizeData();

const actions = {
    FetchSupplierPOSearchResults: (payload) => ({
        type: supplierPOActionTypes.FETCH_SUPPLIER_PO_SEARCH,
        data: payload
    }),
    ClearSupplierpoSearchResults: (payload) => ({
        type: supplierPOActionTypes.CLEAR_SUPPLIER_PO_SEARCH,
        data: payload
    }),
    FetchProjectForSupplierPOCreation: (payload) => ({
        type: supplierPOActionTypes.SAVE_SALECTED_SUPPLIER_PO_ID,
        data: payload
    }),
    FetchSupplierSearchList: (payload) => ({
        type: supplierPOActionTypes.FETCH_SUPPLIER_LIST_FOR_SUPPLIER_PO,
        data: payload
    }),
    ClearSupplierSearchList: () => ({
        type: supplierPOActionTypes.CLEAR_SUPPLIER_LIST_FOR_SUPPLIER_PO
    }),
};
export const FetchSupplierPOSearchResults = (data) => async (dispatch,getstate) => {
    
    dispatch(ShowLoader());
    const state = getstate();
    const params={};
    //const selectedCompany = state.appLayoutReducer.selectedCompany;
    const companyList =state.appLayoutReducer.companyList;
    const supplierPOSearchUrl = supplierPOApiConfig.supplierPOSearch;
    if(data.supplierPOStatus==="all")
    {
        params.supplierPOStatus="";
    }else{
        params.supplierPOStatus = data.supplierPOStatus;
    }    
    if (data.supplierPONumber) {
        params.supplierPONumber = data.supplierPONumber;
    }
    if (data.supplierPODeliveryDate) {
        params.supplierPODeliveryDate = data.supplierPODeliveryDate;
    }
    if (data.supplierPOCompletedDate) {
        params.supplierPOCompletedDate = data.supplierPOCompletedDate;
    }
    if (data.supplierPOContractNumber) {
        params.supplierPOContractNumber = data.supplierPOContractNumber;
    }
    if (data.SupplierPoProjectNumber) {
        params.SupplierPoProjectNumber = data.SupplierPoProjectNumber;
    }
    if (data.supplierPOCustomerProjectName) {
        params.supplierPOCustomerProjectName = data.supplierPOCustomerProjectName;
    }
    if (data.searchDocumentType) {
        params.searchDocumentType = data.searchDocumentType;
    }
    if (data.documentSearchText) {
        params.documentSearchText = data.documentSearchText;
    }
    // Added for Defect 866 Starts
    if(data.customerId){
        // params.SupplierPOCustomerName =data.SupplierPOCustomerName;
        params.SupplierPOCustomerId  =data.customerId ;
    }
    if(data.supplierPOMainSupplierName){
        params.SupplierPOMainSupplierName=data.supplierPOMainSupplierName;
        params.SupplierPOMainSupplierId=data.SupplierPOMainSupplierId;
    }
    if(data.supplierPOSubSupplierName){
        params.SupplierPOSubSupplierName =data.supplierPOSubSupplierName;
        params.SupplierPOSubSupplierId =data.SupplierPOSubSupplierId;
    }
    if (data.supplierPOContractHolderCompany) {
        params.contractCompanyId = data.supplierPOContractHolderCompany;
    }
    if (data.materialDescription) {
        params.supplierPOMaterialDescription = data.materialDescription;
    }
    //search optimization
    if(data.offSet){
        params.OffSet = data.offSet;
    }

    if (data.isExport) //Added for Export to CSV
        params.isExport = data.isExport;
    if (data.supplierPOCompanyId) {
        params.supplierPOCompanyId = data.supplierPOCompanyId;
    }

    //const params = data;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(supplierPOSearchUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        dispatch(actions.FetchSupplierPOSearchResults(error));
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_SUPPLIER_PO_DATA, 'dangerToast FetchSupplierPOSearchResultsErr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });

    if (response && response.code === "1") {
        dispatch(actions.FetchSupplierPOSearchResults(response.result));
        dispatch(HideLoader());
        return response;
    }
    else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast FetchSupplierPOSearchResultsErr');
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_SUPPLIER_PO_DATA, 'dangerToast FetchSupplierPOSearchResultsErr');
    }
    dispatch(HideLoader());
};

export const ClearSupplierSearchList = (data) => (dispatch, getstate) => {
    dispatch(actions.ClearSupplierSearchList());
};

export const ClearSupplierpoSearchResults = (data) => (dispatch, getstate) => {
    dispatch(actions.ClearSupplierpoSearchResults());
};

/**
 * Fetch Project Info for creation of Supplier PO
 */
export const FetchProjectForSupplierPOCreation = (proNumber) => async (dispatch, getstate) => { 
    dispatch(ShowLoader());
    const state = getstate().RootProjectReducer.ProjectDetailReducer;
    const projectNo = state.selectedProjectNo !== '' ? state.selectedProjectNo : proNumber;   
    const projectInfoUrl = projectAPIConfig.projectSearch;
    const params = {
        'projectNumber': projectNo
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(projectInfoUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_PROJECT_FOR_SUPPLIER_PO_CREATION, 'dangerToast projectForSupplierPOFailure');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (response && response.code === "1") {
        if (response.result && response.result.length > 0) {
            
            dispatch(UpdateCurrentPage(localConstant.supplierpo.ADD_SUPPLIER_PO_CURRENTPAGE));
            dispatch(UpdateCurrentModule('supplierpo'));

            dispatch(FetchSupplierPOProjectInfo(response.result[0]));
        }
    }
    else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
        IntertekToaster(parseValdiationMessage(response), 'dangerToast projectForSupplierPOFailure');
    }
    else {
        IntertekToaster(localConstant.errorMessages.FETCH_PROJECT_FOR_SUPPLIER_PO_CREATION, 'dangerToast projectForSupplierPOFailure');
    }
    dispatch(HideLoader());
};

export const FetchSupplierPOProjectInfo = (data) => (dispatch, getstate) => {   
    const supplierPOData = Object.assign({}, getstate().rootSupplierPOReducer.supplierPOData);
    if (data) {
        supplierPOData.SupplierPOInfo = {
            "supplierPOCurrency": data.projectBudgetCurrency,
            "supplierPOId": null,
            "supplierPOProjectNumber": data.projectNumber,
            "supplierPOCustomerCode": data.contractCustomerCode,
            "supplierPOCustomerName": data.contractCustomerName,
            "supplierPOCustomerProjectName": data.customerProjectName,
            "supplierPOStatus": "O",
            "supplierPOBudget": '00.00', //uncommented for D-623 issue 1
            "supplierPOBudgetHours": '00.00',
            "supplierPOBudgetHoursWarning":data.projectBudgetHoursWarning,
            "supplierPOBudgetWarning":data.projectBudgetWarning,
            "oldSupplierPOBudgetHoursWarning":data.projectBudgetHoursWarning,
            "oldSupplierPOBudgetWarning":data.projectBudgetWarning,
            "recordStatus": "N"
        };
        supplierPOData.SupplierPOSubSupplier = null;
        supplierPOData.SupplierPODocuments = null;
        supplierPOData.SupplierPONotes = null;
        dispatch(actions.FetchProjectForSupplierPOCreation(supplierPOData));
    }
};

export const FetchSupplierSearchList = (data) => async (dispatch) => {
    dispatch(ShowLoader());
    const supplierSearchUrl = supplierAPIConfig.supplierBaseUrl + supplierAPIConfig.supplierSearch;
    const params = data;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(supplierSearchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.SUPPLIER_SEARCH_LIST_VALIDATION, 'warningToast supplierSearchFetchVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code === "1") {
            dispatch(actions.FetchSupplierSearchList(response.result));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast supplierSearchFetchVal');
        }
        else {
            IntertekToaster(localConstant.validationMessage.SUPPLIER_SEARCH_LIST_VALIDATION, 'dangerToast fetchSupplierSearchFailure');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG, 'dangerToast fetchSupplierSearchFailure');
    }
    dispatch(HideLoader());
};
