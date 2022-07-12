import { customerAPIConfig,  projectAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import { projectActionTypes } from '../../constants/actionTypes';
import { FetchData } from '../../services/api/baseApiService';  
import { ShowLoader, HideLoader } from '../../common/commonAction';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { isEmpty, getlocalizeData,isEmptyReturnDefault,parseValdiationMessage } from '../../utils/commonUtils';
import { ClearData } from '../../components/applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { ReplaceString } from '../../utils/stringUtil';
const localConstant = getlocalizeData();
const actions = {
    FetchProjectListSuccess: (payload) => ({
        type: projectActionTypes.FETCH_PROJECT_LIST_SUCCESS,
        data: payload
    }),
    FetchProjectListError: (error) => ({
        type: projectActionTypes.FETCH_PROJECT_LIST_ERROR,
        data: error
    }),
    FetchCustomerListSuccess: (payload) => ({
        type: projectActionTypes.FETCH_CUSTOMER_LIST_SUCCESS,
        data: payload
    }),
    FetchCustomerListError: (payload) => ({
        type: projectActionTypes.FETCH_CUSTOMER_LIST_ERROR,
        data: payload
    }),
    ClearSearchCustomerList: () => ({
        type: projectActionTypes.CLEAR_SEARCH_CUSTOMER_LIST
    }),
    ClearSearchProjectList: (payload) => ({
        type: projectActionTypes.CLEAR_SEARCH_PROJECT_LIST
    }),
    ClearGridFormSearchData:(payload)=>({
        type:projectActionTypes.CLEAR_GRID_FORM_SEARCH_DATAS
    })
};

export const FetchProjectList = (data) => async (dispatch, getstate) => { 
    dispatch(ShowLoader());
    dispatch(actions.FetchProjectListSuccess(null));
    
    const projectSearchUrl = projectAPIConfig.baseUrl + projectAPIConfig.projectSearch + projectAPIConfig.getSelectiveProjectData;

    const params = {};
    if (!isEmpty(data.customerName)) {
        params.contractCustomerName = data.customerName;
    }
    if (!isEmpty(data.contractCustomerCode)) {
        params.contractCustomerCode = data.contractCustomerCode;
    }
    if(!isEmpty(data.contractNumber)){
        params.contractNumber = data.contractNumber;
    }
    if(!isEmpty(data.customerContractNumber)){
        params.customerContractNumber = data.customerContractNumber;
    } 
    if(!isEmpty(data.projectNumber)){
        params.projectNumber = data.projectNumber;
    }
    if(!isEmpty(data.customerProjectNumber)){
        params.customerProjectNumber = data.customerProjectNumber;
    }
    if(!isEmpty(data.customerProjectName)){
        params.customerProjectName = data.customerProjectName;
    }
    if(!isEmpty(data.contractHoldingCompanyCode)){
        params.contractHoldingCompanyCode = data.contractHoldingCompanyCode;
    }
    if(!isEmpty(data.division)){
        params.companyDivision = data.division;
    }

    if(!isEmpty(data.office)){
        params.companyOffice = data.office;
    }

    if(!isEmpty(data.businessUnit)){
        params.projectType = data.businessUnit;
    }

    if(!isEmpty(data.coordinatorName)){
        params.projectCoordinatorName = data.coordinatorName;
    }

    if(!isEmpty(data.projectStatus)){
        params.projectStatus = data.projectStatus;
    }
    if(!isEmpty(data.projectDocumentId)){
        params.projectDocumentId = data.projectDocumentId;
    }
    if(!isEmpty(data.searchText)){
        params.searchText = data.searchText;
    }
    if(!isEmpty(data.workFlowTypeIn)){
        params.workFlowTypeIn = data.workFlowTypeIn;
    }  
    if (!isEmpty(data.searchDocumentType )) {
        params.searchDocumentType= data.searchDocumentType;
    }
    if (!isEmpty(data.documentSearchText )) {
        params.documentSearchText= data.documentSearchText;
    }

    const requestPayload = new RequestPayload(params);
    const response = await FetchData(projectSearchUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        dispatch(actions.FetchProjectListError(error));
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast contSearchActCustomerList');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });

    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code === "1") {
            dispatch(actions.FetchProjectListSuccess(response.result));
            dispatch(HideLoader());
            return response.result;
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast PrjctListWentWrong');
        }
        else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast FetchCustomerList');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast FetchCustomerList');
    }
    dispatch(HideLoader());
    return response;
};

export const FetchProjectListSearch = (data) => async (dispatch, getstate) => { 
    dispatch(ShowLoader());
    dispatch(actions.FetchProjectListSuccess(null));
    
    const projectSearchUrl = projectAPIConfig.baseUrl + projectAPIConfig.projectSearch + projectAPIConfig.getSelectiveProjectData;

    const params = {};
    if (!isEmpty(data.customerName)) {
        // params.contractCustomerName = data.customerName;
        params.contractCustomerId  = data.customerId;
    }
    if (!isEmpty(data.contractCustomerCode)) {
        params.contractCustomerCode = data.contractCustomerCode;
    }
    if(!isEmpty(data.contractNumber)){
        params.contractNumber = data.contractNumber;
    }
    if(!isEmpty(data.customerContractNumber)){
        params.customerContractNumber = data.customerContractNumber;
    } 
    if(!isEmpty(data.projectNumber)){
        params.projectNumber = data.projectNumber;
    }
    if(!isEmpty(data.customerProjectNumber)){
        params.customerProjectNumber = data.customerProjectNumber;
    }
    if(!isEmpty(data.customerProjectName)){
        params.customerProjectName = data.customerProjectName;
    }
    if(!isEmpty(data.contractHoldingCompanyCode)){
        params.contractHoldingCompanyCode = data.contractHoldingCompanyCode;
    }
    if(!isEmpty(data.division)){
        params.companyDivision = data.division;
    }

    if(!isEmpty(data.office)){
        params.companyOffice = data.office;
    }

    if(!isEmpty(data.businessUnit)){
        params.projectType = data.businessUnit;
        params.projectTypeId = data.businessUnit;
    }

    if(!isEmpty(data.coordinatorName)){
        params.projectCoordinatorName = data.coordinatorName;
    }

    if(!isEmpty(data.projectStatus)){
        params.projectStatus = data.projectStatus;
    }
    if(!isEmpty(data.projectDocumentId)){
        params.projectDocumentId = data.projectDocumentId;
    }
    if(!isEmpty(data.searchText)){
        params.searchText = data.searchText;
    }
    if(!isEmpty(data.workFlowTypeIn)){
        params.workFlowTypeIn = data.workFlowTypeIn;
    }  
    if (!isEmpty(data.searchDocumentType )) {
        params.searchDocumentType= data.searchDocumentType;
    }
    if (!isEmpty(data.documentSearchText )) {
        params.documentSearchText= data.documentSearchText;
    }

    //search optimization
    if(data.OffSet){
        params.OffSet = data.OffSet;
    }
    //search optimization
    if (data.projectSearchTotalCount) {
        params.TotalCount = data.projectSearchTotalCount;
    }
    if (data.isExport) //Added for Export to CSV
        params.isExport = data.isExport;

    const requestPayload = new RequestPayload(params);
    const response = await FetchData(projectSearchUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        dispatch(actions.FetchProjectListError(error));
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast contSearchActCustomerList');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        dispatch(HideLoader());
    });

    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code === "1") {
            dispatch(actions.FetchProjectListSuccess(response.result));
            dispatch(HideLoader());
            return response;
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast PrjctListWentWrong');
        }
        else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast FetchCustomerList');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast FetchCustomerList');
    }
    dispatch(HideLoader());
    return response;
};
//To-Do: Need to be removed, when SearchCustomerName component is ready
export const FetchCustomerList = (data) =>async (dispatch, getstate) => {
    dispatch(actions.FetchCustomerListSuccess(null));
    const FetchCustomerListUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails;
    const params = {};
    if (data.customerName) {
        params["customerName"] = data.customerName;
    }
    if (data.operatingCountry) {
        params["operatingCountry"] = data.operatingCountry;
    }
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(FetchCustomerListUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        dispatch(actions.FetchCustomerListError(error));
        // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast contSearchActCustomerList');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });

    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code === "1") {
            dispatch(actions.FetchCustomerListSuccess(response.result));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast CustListWentWrong');
        }
        else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast FetchCustomerList');
        }
    }
    else {
        IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CUSTOMER_LIST, 'dangerToast FetchCustomerList');
    }
};

export const ClearSearchCustomerList = () => (dispatch) => {
    dispatch(actions.ClearSearchCustomerList());
    
};

export const ClearGridFormSearchData =()=>(dispatch) =>
{
    dispatch(actions.ClearGridFormSearchData());
};
export const ClearSearchProjectList = () => (dispatch, getstate) => {
    dispatch(actions.ClearSearchProjectList());
    dispatch(ClearData());
};