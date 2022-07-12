import MaterializeComponent from 'materialize-css';
import { customerAPIConfig, masterData, contractAPIConfig, RequestPayload } from '../../apiConfig/apiConfig';
import { processApiRequest } from '../../services/api/defaultServiceApi';
import { companyActionTypes, contractActionTypes } from '../../constants/actionTypes';
import { FetchData } from '../../services/api/baseApiService';
import { GetCurrentPage } from '../../actions/contracts/contractAction';
import { UpdateInteractionMode } from '../../actions/contracts/contractAction';
import { ShowLoader, HideLoader,SetCurrentPageMode } from '../../common/commonAction';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { ClearData } from '../../components/applicationComponents/customerAndCountrySearch/cutomerAndCountrySearchAction';
import { getlocalizeData,parseValdiationMessage,isUndefined } from '../../utils/commonUtils';
import { ReplaceString } from '../../utils/stringUtil';
const localConstant = getlocalizeData();
const countryMasterData = masterData.baseUrl + masterData.country;
const actions = {
    FetchContractData: (payload) => (
        {
            type: contractActionTypes.commonActionTypes.FETCH_CONTRACT_DATA,
            data: payload
        }
    ),
    ShowHidePanel: (payload) => ({
        type: companyActionTypes.SHOW_HIDE_PANEL,
         //data: payload
    }),
   
    FetchCustomerContract: (payload) => ({
        type: contractActionTypes.commonActionTypes.CONTRACT_FETCH_CUSTOMER,
        data: payload
    }),
    ClearSearchData: (payload) => ({
        type: contractActionTypes.CLEAR_SEARCH_DATA,
        data:payload
    }),
    GetSelectedCustomerName: (payload) => ({
        type: contractActionTypes.CONTRACT_SELECTED_CUSTOMER_CODE,
        data: payload
    }),
    ClearGridFormSearchData :(payload) => ({
        type: contractActionTypes.CLEAR_GRID_FORM_SEARCH_DATAS,
        data: payload
    })

};

export const ShowHidePanel = () => (dispatch) => {
    dispatch(actions.ShowHidePanel());
};
export const ClearSearchData = () => (dispatch, setstate) => {
    // For resetting the contract search criteria but not the grid
    dispatch(actions.ClearSearchData());
    dispatch(ClearData());
};
export const GetSelectedCustomerName = (data) => (dispatch, getstate) => {  
    dispatch(actions.GetSelectedCustomerName(data));
};

export const ClearGridFormSearchData=()=>(dispatch,setstate)=>{
    dispatch(actions.ClearGridFormSearchData());
    };

export const FetchCountry = () => (dispatch) => {
    processApiRequest(countryMasterData, {
        method: "GET"
    }).then(response => {
            if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchCountry(response.data.result));
            }
            else {
                IntertekToaster(parseValdiationMessage(response.data), 'dangerToast contSearchActFetchCountry');
            }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast contSearchActError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
};

export const FetchCustomerContract = (data) => (dispatch, getstate) => {
    dispatch(ShowLoader());
    let apiUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.searchContracts + '?';
    let contractCustomerName = '';
    if (data.customerName) {
        contractCustomerName = data.customerName;
    }
    if (contractCustomerName) {
        apiUrl += "contractCustomerName=" + contractCustomerName + '&';
    }  
    if (data.contractStatus !== undefined && data.contractStatus !== '') {
        if (data.contractStatus === "C")
            apiUrl += "contractStatus=" + data.contractStatus + '&';
        else if (data.contractStatus === "O")
            apiUrl += "contractStatus=" + data.contractStatus + '&';
    }
    if (data.contractNumber) {
        apiUrl += "contractNumber=" + data.contractNumber + '&';
    }
    if (data.customerContractNumber) {
        apiUrl += "customerContractNumber=" + data.customerContractNumber + '&';
    }
    if (data.contractHoldingCompany) {
        apiUrl += "contractHoldingCompanyCode=" + data.contractHoldingCompany + '&';
    } 
    if (data.searchDocumentType) {
        apiUrl += "searchDocumentType=" + data.searchDocumentType + '&';
    }
    if (data.documentSearchText) {
        const searchText = ReplaceString(data.documentSearchText, '+', '%2B');
        apiUrl += "documentSearchText=" + searchText + '&';
    }
    // else {
    //     apiUrl += "contractHoldingCompanyCode=" + selectedCompany + '&';
    // }
    if (data.isFromSearchContract) {
        apiUrl += "ContractTypeNotIn=FRW&";
    }
    apiUrl = apiUrl.slice(0, -1);
    processApiRequest(apiUrl, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchCustomerContract(response.data.result));
            dispatch(HideLoader());
        } else {
            IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT_DATA, 'dangerToast contSearchActCustomerContract');
        }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast contSearchActCustomerContracterr');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
};

export const GetSelectedContractData = (payload) => (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.GetSelectedCustomerName({}));
    let apiUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contractSearch + '?';
    let contractCustomerName = '';
 
    if (payload.customerName !== '' && payload.customerName !== undefined) {
        contractCustomerName = payload.customerName;
    }
    if (contractCustomerName !== undefined && contractCustomerName !== '') {
        apiUrl += "contractCustomerName=" + contractCustomerName + '&';
    }
    if (payload.contractStatus !== undefined && payload.contractStatus !== '') {
        if (payload.contractStatus === "C")
            apiUrl += "contractStatus=" + payload.contractStatus + '&';
        else
            apiUrl += "contractStatus=" + payload.contractStatus + '&';
    }
    if (payload.contractNumber !== undefined && payload.contractNumber !== '') {
        apiUrl += "contractNumber=" + payload.contractNumber + '&';
    }
    if (payload.customerContractNumber !== undefined && payload.customerContractNumber !== '') {
        apiUrl += "customerContractNumber=" + payload.customerContractNumber + '&';
    }
    if (payload.contractHoldingCompany !== undefined && payload.contractHoldingCompany !== '') {
        apiUrl += "contractHoldingCompanyCode=" + payload.contractHoldingCompany + '&';
    }
    apiUrl = apiUrl.slice(0, -1);
    processApiRequest(apiUrl, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.GetSelectedCustomerName(response.data.result[0]));
            const company = getstate().appLayoutReducer.selectedCompany;
            if (response.data.result[0].contractHoldingCompanyCode == company) {
                dispatch(GetCurrentPage('Edit Contract'));
                dispatch(UpdateInteractionMode(false));
                dispatch(SetCurrentPageMode());
            }
            else {
                dispatch(GetCurrentPage('View Contract'));
                dispatch(UpdateInteractionMode(true));
                dispatch(SetCurrentPageMode("View"));
            }
            dispatch(HideLoader());
        }
        else {
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast contSearchActCustomerContract');
        }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast contSearchActcontractDataerror');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
};

export const FetchContractForDashboard = (payload) => async (dispatch, getstate) => {
    dispatch(actions.FetchContractData({}));
    const fetchUrl = contractAPIConfig.contractBaseUrl + contractAPIConfig.contracts + contractAPIConfig.detail;
    const params = {
        'contractNumber': payload.contractNumber
    };
    const requestPayload = new RequestPayload(params);
    const fetchResponse = await FetchData(fetchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FAILED_FETCH_CONTRACT_DATA, 'dangerToast contSearchActContractDashboard');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (fetchResponse) {
        dispatch(actions.FetchContractData(fetchResponse));
    }
    else {
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast contSearchActContractDashboardError');
    }
};