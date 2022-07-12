import { supplierActionTypes } from '../../constants/actionTypes';
import { masterData,RequestPayload } from '../../apiConfig/apiConfig';
import { FetchData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData,isEmpty,parseValdiationMessage } from '../../utils/commonUtils';

const localConstant = getlocalizeData();

const actions={
    AddSupplierDetails:(payload)=>({
        type:supplierActionTypes.ADD_SUPPLIER_DETAILS,
        data:payload
    }),
    AddSupplierContact:(payload)=>({
        type:supplierActionTypes.ADD_SUPPLIER_CONTACT,
        data:payload
    }),
    UpdateSupplierContact:(payload)=>({
        type:supplierActionTypes.UPDATE_SUPPLIER_CONTACT,
        data:payload
    }),
    DeleteSupplierContact:(payload)=>({
        type:supplierActionTypes.DELETE_SUPPLIER_CONTACT,
        data:payload
    }),
    FetchStateForAddress:(payload)=>({
        type:supplierActionTypes.FETCH_STATE_FOR_ADDRESS,
        data:payload
    }),
    FetchCityForAddress:(payload)=>({
        type:supplierActionTypes.FETCH_CITY_FOR_ADDRESS,
        data:payload
    })
};

/**
 * Action to add supplier details.
 */
export const AddSupplierDetails = (data) => (dispatch,getstate)=>{
    dispatch(actions.AddSupplierDetails(data));
};

/**
 * Add new row of supplier contact data
 * @param {*Object} data 
 */
export const AddSupplierContact = (data) => (dispatch,getstate) => {
    dispatch(actions.AddSupplierContact(data));
};

/**
 * Update the edited record to supplier contact
 * @param {*Object} data 
 */
export const UpdateSupplierContact = (data) => (dispatch,getstate) => {
    let index = -1;
    const state = getstate();
    const supplierContact = Object.assign([],state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierContacts);
    index = state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierContacts.findIndex(iteratedValue => iteratedValue.supplierContactId == data.supplierContactId);
    supplierContact[index] = data;
    if(index >= 0){
        dispatch(actions.UpdateSupplierContact(supplierContact));
    }
};

/**
 * Delete selected records from supplier contact
 * @param {*Object} data 
 */
export const DeleteSupplierContact = (data) => (dispatch,getstate) => {
    const state = getstate();
    const supplierContact = Object.assign([],state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierContacts);
    data.map(row=>{
        supplierContact.map((iteratedValue, index) => {
            if (iteratedValue.supplierContactId === row.supplierContactId) {
                if (iteratedValue.recordStatus !== "N") {
                    supplierContact[index].recordStatus = "D";
                }
                else {
                    supplierContact.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteSupplierContact(supplierContact));
};

/**
 * Fetch state for address section based on selected country
 * @param {*String} data 
 */
export const FetchStateForAddress = (data) => async(dispatch,getstate) => {
    dispatch(actions.FetchStateForAddress([]));
    dispatch(actions.FetchCityForAddress([]));
    const state = getstate();
    const stateUrl = masterData.baseUrl + masterData.state;
    const params = {
        'countryId': data   //Changes for D-1076
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(stateUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.STATE_FETCH_VALIDATION, 'warningToast supplierStateFetchAddressVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchStateForAddress(response.result));
        }
        else if (response.code == 11 ||response.code == 41 ||response.code ==31) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast Supplierdetail');
        }
        else {
                IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchStateAddressCode11Fail');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchStateAddressFailure');
    }
};

/**
 * Fetch city for address section based on selected state
 * @param {*String} data 
 */
export const FetchCityForAddress = (data) => async(dispatch,getstate) => {
    dispatch(actions.FetchCityForAddress([]));
    const state = getstate();
    const cityUrl = masterData.baseUrl + masterData.city;
    const params = {
        'stateId': data  //Changes for D-1076
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(cityUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.CITY_FETCH_VALIDATION, 'warningToast supplierCityFetchAddressVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchCityForAddress(response.result));
        }
        else if (response.code == 11||response.code == 41 ||response.code ==31) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast Supplierdetail');
        }
        else {
            IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchCityAddressCode11Fail');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast fetchCityAddressFailure');
    }
};