import { supplierActionTypes } from '../../constants/actionTypes';
import { masterData, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData } from '../../../src/services/api/baseApiService';
import { getlocalizeData, isEmpty,parseValdiationMessage } from '../../utils/commonUtils';
const localConstant = getlocalizeData();
const actions = {
    AddSupplierDocumentDetails: (payload) => ({
        type: supplierActionTypes.suppler_Documents.ADD_SUPPLIER_DOCUMENTS,
        data: payload
    }),
    FetchSupplierDocumentTypes: (payload) => ({
        type: supplierActionTypes.suppler_Documents.FETCH_SUPPLIER_DOCUMENT_TYPES,
        data: payload
    }),
    UpdateSupplierDocumentDetails: (payload) => ({
        type: supplierActionTypes.suppler_Documents.UPDATE_SUPPLIER_DOCUMENTS_DETAILS,
        data: payload
    }),
    DeleteSupplierDocumentDetails: (payload) => ({
        type: supplierActionTypes.suppler_Documents.DELETE_SUPPLIER_DOCUMENTS_DETAILS,
        data: payload
    }),
    AddFilesToBeUpload: (payload) => ({
        type: supplierActionTypes.suppler_Documents.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    ClearFilesToBeUpload: (payload) => ({
        type: supplierActionTypes.suppler_Documents.CLEAR_FILES_TO_BE_UPLOADED,
        data: payload
    }),
};

export const FetchSupplierDocumentTypes = (data) => async (dispatch, getstate) => {
    const documentTypesMasterDataUrl = masterData.baseUrl + masterData.documentType + "?moduleName=supplier";
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(documentTypesMasterDataUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.supplier.documents.FETCH_DOC_TYPE_FAILED, 'wariningToast supplierDocumentTypeVal');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchSupplierDocumentTypes(response.result));
        }
        else if (response.code == 41 ||response.code == 11||response.code ==31) {
            IntertekToaster(parseValdiationMessage(response), 'warningToast Supplierdocument');
        }
        else {
            IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast Supplierdocument');
        }
    } else{
        IntertekToaster(localConstant.validationMessage.API_WENT_WRONG,'dangerToast Supplierdocument');
    }
};
export const AddSupplierDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.AddSupplierDocumentDetails(data));
};
export const UpdateSupplierDocumentDetails = (data, editedData) => (dispatch, getstate) => {
    const state = getstate();
    if(editedData){
    const editedRow = Object.assign({}, editedData, data);
    const index = state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierDocuments.findIndex(document => document.id === editedRow.id);
    const newState = Object.assign([], state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierDocuments);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateSupplierDocumentDetails(newState));
    }
}else{
    dispatch(actions.UpdateSupplierDocumentDetails(data));
}
};
export const DeleteSupplierDocumentDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.RootSupplierReducer.SupplierDetailReducers.supplierData.SupplierDocuments);
    data.map(row => {
        newState.map(document => {
            if (document.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                newState[index].recordStatus = "D";
            }
        });
    });
    dispatch(actions.DeleteSupplierDocumentDetails(newState));
};

export const AddFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.AddFilesToBeUpload(data));
};

export const ClearFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.ClearFilesToBeUpload([]));
};