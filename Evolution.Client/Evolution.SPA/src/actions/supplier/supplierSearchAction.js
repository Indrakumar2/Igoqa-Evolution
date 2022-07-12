import { supplierActionTypes } from '../../constants/actionTypes';
import { masterData,RequestPayload, supplierAPIConfig } from '../../apiConfig/apiConfig';
import { ShowLoader, HideLoader,SetCurrentPageMode,UpdateCurrentPage } from '../../common/commonAction';
import { FetchData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData,isEmpty,parseValdiationMessage } from '../../utils/commonUtils';
import { FetchSupplierData } from './supplierAction';

const localConstant = getlocalizeData();

const actions = {
    SupplierFetchState: (payload) => ({
        type: supplierActionTypes.SUPPLIER_FETCH_STATE,
        data: payload
    }),
    SupplierFetchCity: (payload) => ({
        type: supplierActionTypes.SUPPLIER_FETCH_CITY,
        data: payload
    }),
    FetchSupplierSearchList: (payload) => ({
        type: supplierActionTypes.FETCH_SUPPLIER_SEARCH_LIST,
        data: payload
    }),
    ClearSupplierSearchList: (payload) => ({
        type: supplierActionTypes.CLEAR_SUPPLIER_SEARCH_LIST,
        data: payload
    }),
    GetSelectedSupplier: (payload) => ({
        type: supplierActionTypes.GET_SELECTED_SUPPLIER,
        data: payload
    }),
    ClearGridFormSearchData:(payload)=>({
        type:supplierActionTypes.CLEAR_GRID_FORM_SEARCH_DATAS,
        data:payload
    }),
};

/**
 * Fetch state based on the country change
 * @param {*String} data 
 */
export const SupplierFetchState = (data) => async (dispatch, getstate) => {
    dispatch(actions.SupplierFetchState([]));
    dispatch(actions.SupplierFetchCity([]));
    const state = getstate();
    const stateUrl = masterData.baseUrl + masterData.state;
    const params = {
        'countryId': data
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(stateUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.STATE_FETCH_VALIDATION, 'warningToast supplierStateFetchVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.SupplierFetchState(response.result));
        }
        else if (response.code == 11||response.code == 41||response.code ==31) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast SupplierSearch');
        }
        else {
                IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchStateCode11Fail');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchStateFailure');
    }
};

/**
 * Fetch city based on the state change
 * @param {*String} data 
 */
export const SupplierFetchCity = (data) => async(dispatch, getstate) => {
    dispatch(actions.SupplierFetchCity([]));
    const state = getstate();
    const cityUrl = masterData.baseUrl + masterData.city;
    const params = {
        'stateId': data
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(cityUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.CITY_FETCH_VALIDATION, 'warningToast supplierCityFetchVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.SupplierFetchCity(response.result));
        }
        else if (response.code == 11||response.code == 41||response.code == 31) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast SupplierSearch');
        }
        else {
            IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchCityCode11Fail');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchCityFailure');
    }
};

/**
 * Search Data
 * @param {*Object} data 
 */
export const FetchSupplierSearchList = (data) => async(dispatch,getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const supplierSearchUrl = supplierAPIConfig.supplierBaseUrl + supplierAPIConfig.supplierSearch;
    const params = data;
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(supplierSearchUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.SUPPLIER_SEARCH_LIST_VALIDATION, 'warningToast supplierSearchFetchVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchSupplierSearchList(response.result));
        }
        else if (response.code == 11||response.code == 41||response.code ==31) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast SupplierSearch');
        }
        else {
                IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchSupplierSearchCode11Fail');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchSupplierSearchFailure');
    }
    dispatch(HideLoader());
};

export const ClearSupplierSearchList = () => (dispatch,getstate) => {
    dispatch(actions.ClearSupplierSearchList());
};

export const GetSelectedSupplier = (data) => (dispatch, getstate) => {
    dispatch(FetchSupplierData(data,true));
    dispatch(SetCurrentPageMode());
    dispatch(UpdateCurrentPage(localConstant.supplier.EDIT_VIEW_SUPPLIER)); //D305
};

export const ClearGridFormSearchData =()=>(dispatch,getstate)=>
{
    dispatch(actions.ClearGridFormSearchData());
};
