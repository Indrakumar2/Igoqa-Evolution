import { supplierActionTypes, commonActionTypes,sideMenu } from '../../constants/actionTypes';
import { supplierAPIConfig, RequestPayload, projectAPIConfig } from '../../apiConfig/apiConfig';
import { FetchData, DeleteData, PostData, CreateData } from '../../services/api/baseApiService';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData, isEmpty, isEmptyReturnDefault,parseValdiationMessage,FilterSave, isUndefined,getNestedObject } from '../../utils/commonUtils';
import { ShowLoader, HideLoader,SetCurrentPageMode,UpdateCurrentModule } from '../../common/commonAction';
import { FetchStateForAddress, FetchCityForAddress } from './supplierDetailAction';
import { SuccessAlert, FailureAlert, WarningAlert, ValidationAlert, DeleteAlert, CreateAlert } from '../../components/viewComponents/customer/alertAction';
import { UpdateSelectedCompany } from '../../components/appLayout/appLayoutActions';
import { required } from '../../utils/validator';
import { StringFormat } from '../../utils/stringUtil';
import { RemoveDocumentsFromDB } from '../../common/commonAction';
import {
    ChangeDataAvailableStatus
} from '../../components/appLayout/appLayoutActions';

const localConstant = getlocalizeData();

const actions = {
    FetchSupplierData: (payload) => ({
        type: supplierActionTypes.FETCH_SUPPLIER_DATA,
        data: payload
    }),
    DeleteSupplierData: (payload) => ({
        type: supplierActionTypes.DELETE_SUPPLIER_DATA,
        data: payload
    }),
    SaveSupplierData: (payload) => ({
        type: supplierActionTypes.SAVE_SUPPLIER_DATA,
        data: payload
    }),
    UpdateSupplierData: (payload) => ({
        type: supplierActionTypes.UPDATE_SUPPLIER_DATA,
        data: payload
    }),
    UpdateInteractionMode: (payload) => ({
        type: commonActionTypes.INTERACTION_MODE,
        data: payload
    }),
    UpdateSupplierBtnEnableStatus: (payload) => ({
        type: supplierActionTypes.UPDATE_SUPPLIER_BTN_ENABLE_STATUS,
        data: payload
    }),
    SaveSelectedSupplier: (payload) => ({
        type: supplierActionTypes.SAVE_SELECTED_SUPPLIER,
        data: payload
    }),
    SupplierDuplicateName: (payload) => ({
        type: supplierActionTypes.SUPPLIER_DUPLICATE_NAME,
        data: payload
    }),
    ClearSupplierDetails: (payload) => ({
        type: sideMenu.CLEAR_SUPPLIER       
    }),
};

/**
 * Fetch supplier data action.
 */

export const FetchSupplierData = (data,isFetchLookUpValues) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    dispatch(actions.FetchSupplierData({}));
    dispatch(actions.UpdateSupplierBtnEnableStatus());
    let selectedCompany = getstate().appLayoutReducer.selectedCompany;
    const selectedCompanyData = isEmptyReturnDefault(getstate().appLayoutReducer.selectedCompanyData);
    const operatingCountry = selectedCompanyData.operatingCountry;
    if (data && data.selectedCompany) {
        selectedCompany = data.selectedCompany;
        dispatch(UpdateSelectedCompany({ companyCode: data.selectedCompany }));
    }
    if (data && data.supplierId) {
        const state = getstate();
        const companyList = state.appLayoutReducer.companyList;
        dispatch(actions.SaveSelectedSupplier(data));
        const supplierUrl = StringFormat(supplierAPIConfig.fetchSupplier, data.supplierId);
        const params = {};
        const requestPayload = new RequestPayload(params);
        const response = await FetchData(supplierUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.validationMessage.SUPPLER_FETCH_VALIDATION, 'warningToast supplierFetchVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
                dispatch(HideLoader());
            });
        if (!isEmpty(response)) {
            
            //646 Fixes
            if (isEmpty(response.SupplierInfo)) {
                dispatch(HideLoader());
                dispatch(ChangeDataAvailableStatus(true));
                return response;
            }

            if (response.SupplierInfo) {
                response.SupplierInfo["oldSupplierName"]=response.SupplierInfo && response.SupplierInfo.supplierName;
            }
            dispatch(actions.FetchSupplierData(response));
            if (response.SupplierInfo) {
                if(isFetchLookUpValues === undefined || isFetchLookUpValues === true){
                    dispatch(FetchStateForAddress(response.SupplierInfo.countryId)); //Changes for D-1076
                    dispatch(FetchCityForAddress(response.SupplierInfo.stateId));   //Changes for D-1076
                }
                dispatch(UpdateCurrentModule(localConstant.moduleName.SUPPLIER));
                // dispatch(actions.UpdateInteractionMode(false));
                // dispatch(SetCurrentPageMode());
            }
            dispatch(HideLoader());
        }
        else {
            IntertekToaster(localConstant.validationMessage.API_WENT_WRONG, 'dangerToast fetchSupplierApiFailure');
        }
    }
    dispatch(HideLoader());
};

/**
 * Delete Supplier Data
 */
export const DeleteSupplierData = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const supplierInfo = Object.assign({}, state.RootSupplierReducer.SupplierDetailReducers.supplierData);
    supplierInfo.SupplierInfo.recordStatus = 'D';
    const deleteSupplierUrl = StringFormat(supplierAPIConfig.fetchSupplier, data.supplierId);

    const params = {
        data: supplierInfo,
    };

    const requestPayload = new RequestPayload(params);

    const response = await DeleteData(deleteSupplierUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.SUPPLIER_DELETE_VALIDATION, 'warningToast supplierDeleteVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(DeleteAlert(response, "Supplier"));
            dispatch(HideLoader());
            return response;
        }
        else if (response.code == 11 ||response.code == 41 ||response.code ==31) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast Supplier');  //D669 issue-17
            dispatch(HideLoader());
        }
        else {
            dispatch(ValidationAlert(localConstant.validationMessage.API_WENT_WRONG, "Supplier"));
        }
    }
    else {
        dispatch(FailureAlert(response.data, "Supplier"));
    }
    dispatch(HideLoader());
};

/**
 * Save Supplier Data
 */
export const SaveSupplierData = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const supplierData = Object.assign({}, state.RootSupplierReducer.SupplierDetailReducers.supplierData);
    supplierData.SupplierInfo["supplierId"] = data;
    supplierData.SupplierInfo["supplierName"] = supplierData.SupplierInfo["supplierName"].trim();
    supplierData.SupplierInfo["supplierAddress"] = supplierData.SupplierInfo["supplierAddress"].trim();
    const supplierSaveUrl = StringFormat(supplierAPIConfig.fetchSupplier, data);
    const requestPayload = new RequestPayload(supplierData);

    const response = await PostData(supplierSaveUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.SUPPLIER_SAVE_VALIDATION, 'warningToast supplierSaveVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == "1") {
            dispatch(CreateAlert(response, "Supplier"));
            return response;
        }
        else if (response.code == "11" ||response.code == "41" ||response.code == "31") {
            IntertekToaster(parseValdiationMessage(response), 'warningToast Supplier');
        }
        else {
                dispatch(ValidationAlert(localConstant.validationMessage.API_WENT_WRONG, "Supplier"));
        }
    }
    else {
        dispatch(FailureAlert(response, "Supplier"));
    }
    dispatch(HideLoader());
};

/**
 * Update Supplier Data
 */
export const UpdateSupplierData = (data) => async (dispatch, getstate) => {
    dispatch(ShowLoader());
    const state = getstate();
    const supplierData = Object.assign({}, state.RootSupplierReducer.SupplierDetailReducers.supplierData);
    const supplierUpdateUrl = StringFormat(supplierAPIConfig.fetchSupplier, data);
    supplierData.SupplierInfo.recordStatus = "M";
    supplierData.SupplierInfo["supplierName"] = supplierData.SupplierInfo["supplierName"].trim();
    supplierData.SupplierInfo["supplierAddress"] = supplierData.SupplierInfo["supplierAddress"].trim();
    if (supplierData.SupplierDocuments) {
        const doc = supplierData.SupplierDocuments;
        for (let i = 0; i < doc.length; i++) {
            if (!isEmpty(doc[i].status)) {
                doc[i].status = doc[i].status.trim();
            }
            if (doc[i].recordStatus == "N") {
                doc[i].id = 0;
            }
        }
    };
    const requestPayload = new RequestPayload(FilterSave(supplierData));

    const response = await CreateData(supplierUpdateUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.SUPPLIER_UPDATE_VALIDATION, 'warningToast supplierUpdateVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            dispatch(HideLoader());
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == "1") {
            dispatch(SuccessAlert(response, "Supplier"));
            dispatch(FetchSupplierData(supplierData.SupplierInfo,false));
        }
        else if (response.code == "11" ||response.code == "41" || response.code == "31") {
            IntertekToaster(parseValdiationMessage(response), 'warningToast Supplier');
        }
        else {
                dispatch(ValidationAlert(localConstant.validationMessage.API_WENT_WRONG, "Supplier"));
        }
    }
    else {
        dispatch(FailureAlert(response, "Supplier"));
    }
    dispatch(HideLoader());
};
export const SupplierDuplicateName = (data) => async (dispatch,getstate) => {
    const state = getstate();
    dispatch(ShowLoader()); // D-170
    const { SupplierInfo = {} } = Object.assign({},state.RootSupplierReducer.SupplierDetailReducers.supplierData);
    const duplicateSupplier = [];
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
        if (!isEmpty(response.result) && (response.code == 1)) {
            if (response.result && response.result.length > 0) {
                response.result.forEach(iteratedValue => {
                    if(SupplierInfo.postalCode){
                        if (iteratedValue.country === SupplierInfo.country && iteratedValue.state === SupplierInfo.state && iteratedValue.postalCode === SupplierInfo.postalCode) {
                            duplicateSupplier.push(iteratedValue);
                        }
                    }
                    else{
                        if (iteratedValue.country === SupplierInfo.country && iteratedValue.state === SupplierInfo.state && iteratedValue.city === SupplierInfo.city) {
                            duplicateSupplier.push(iteratedValue);
                        }
                    }
                    
                });
                dispatch(actions.SupplierDuplicateName(response.result));
                dispatch(HideLoader()); // D-170
                return duplicateSupplier;
            }
            else{
                dispatch(HideLoader()); // D-170
                return false;
            }
        } else if (isEmpty(response.result) && (response.code == 1)) {
            dispatch(HideLoader()); // D-170
            return false;
        }
        else if (response.code == 11||response.code == 41 ||response.code == 31) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast Supplierduplicate');
        }
        else {
                IntertekToaster(localConstant.validationMessage.API_WENT_WRONG, 'dangerToast fetchSupplierSearchCode11Fail');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG, 'dangerToast fetchSupplierSearchFailure');
    }
    dispatch(HideLoader());
};
export const CancelEditSupplierDetailsDocument = () => async (dispatch, getstate) => { 
    const state = getstate();
    const supplierInfo = Object.assign({},state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierInfo);
    const documentData = state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierDocuments;
    const supplierId = supplierInfo.supplierId;
    const deleteUrl = StringFormat(supplierAPIConfig.supplierDocumentsDelete, supplierId);
    let response = null;
    if (!isEmpty(documentData) && !isUndefined(supplierId)) {
        response = await RemoveDocumentsFromDB(documentData, deleteUrl);
    }
    return response;   
};
export const ClearSupplierDetails = () => async (dispatch, getstate) => { 
    dispatch(actions.ClearSupplierDetails());
};