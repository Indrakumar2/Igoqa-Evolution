import { customerAPIConfig, masterData, contractAPIConfig, RequestPayload } from '../../../apiConfig/apiConfig';
import { processApiRequest } from '../../../services/api/defaultServiceApi';
import { customerCountrySearch, supplierActionTypes } from '../../../constants/actionTypes';
import { FetchData } from '../../../services/api/baseApiService';
import IntertekToaster from '../../../common/baseComponents/intertekToaster';
import { isUndefined,parseValdiationMessage } from '../../../utils/commonUtils';

const actions = {
    FetchContractData: (payload) => (
        {
            type: customerCountrySearch.FETCH_CONTRACT_DATA,
            data: payload
        }
    ),   
    FetchCustomerList: (payload) => ({
        type: customerCountrySearch.FETCH_CUSTOMER_COUNTRY_LIST,
        data: payload
    }),
    OnSubmitCustomerName: (payload) => ({
        type: customerCountrySearch.SELECTED_CUSTOMER_NAME,
        data: payload
    }),
   
    ClearData:()=>({
        type:customerCountrySearch.CLEAR_DATA
    }),

    ClearCustomerData: () =>({
        type: supplierActionTypes.CLEAR_CUSTOMER_DATA,

     }),
     ClearReportsData: () =>(
         {
         type: customerCountrySearch.CLEAR_REPORTS_DATA,
         }),

    GetSelectedCustomerName: (payload) => ({
        type: customerCountrySearch.COUNTRY_SELECTED_CUSTOMER_CODE,
        data: payload
    }),
    SetDefaultCustomerName: (payload) => ({
        type: customerCountrySearch.SET_DEFAULT_CUSTOMER_NAME,
        data: payload
    }),
    SetDefaultReportCustomerName : (payload) => ({
        type: customerCountrySearch.SET_DEFAULT_REPORT_CUSTOMER_NAME,
        data: payload
    }),
    SetSelectedCountryName : (payload) => ({
        type: customerCountrySearch.SET_SELECTED_COUNTRY_NAME,
        data: payload
    }),
    SetReportSelectedCountryName: (payload) =>({
        type: customerCountrySearch.SET_REPORT_SELECTED_COUNTRY_NAME,
        data: payload
    }),
    UpdateReportCustomer: (payload) => ({
        type: customerCountrySearch.UPDATE_REPORT_CUSTOMER,
        data: payload

    }),
};
export const SetDefaultCustomerName = (payload) => (dispatch) => {
    dispatch(actions.SetDefaultCustomerName(payload));
};

export const SetDefaultReportCustomerName = (payload) => (dispatch) => {
    dispatch(actions.SetDefaultReportCustomerName(payload));
};

export const OnSubmitCustomerName = (data) => (dispatch) => {
    dispatch(actions.OnSubmitCustomerName(data));
};

export const UpdateReportCustomer =(data)=> async(dispatch,getstate) =>
{
    dispatch(actions.UpdateReportCustomer(data));
};
export const ClearCustomerData=()=>async(dispatch)=> {
    dispatch(actions.ClearCustomerData());
};

export const ClearReportsData =() =>async(dispatch)=>
{
      dispatch(actions.ClearReportsData());
};

export const ClearData = () => (dispatch) => {
    dispatch(actions.ClearData());
};
export const GetSelectedCustomerName = (data) => (dispatch, getstate) => {
    dispatch(actions.GetSelectedCustomerName(data));
};

export const FetchCustomerList = (data) => (dispatch, getstate) => {
    dispatch(actions.FetchCustomerList(null));
    let apiUrl = customerAPIConfig.custBaseUrl + customerAPIConfig.customerDetails + '?';
    if (data.customerName !== undefined && data.customerName !== '') {        
        data.customerName = data.customerName.replace(/&/g, "%26");
        apiUrl += "customerName=" + data.customerName + '&';
    }
    if (data.operatingCountry !== undefined && data.operatingCountry !== '') {
        apiUrl += "operatingCountry=" + data.operatingCountry + '&';
    }
    apiUrl = apiUrl.slice(0, -1);
    processApiRequest(apiUrl, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data)&&!isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchCustomerList(response.data.result));
        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast contSearchActCustomerList');
        }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast errorUnique1');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
};

export const SetSelectedCountryName = (data) => (dispatch) =>{
    dispatch(actions.SetSelectedCountryName(data));
};

export const SetReportSelectedCountryName = (data) => (dispatch) =>{
    dispatch(actions.SetReportSelectedCountryName(data));
};