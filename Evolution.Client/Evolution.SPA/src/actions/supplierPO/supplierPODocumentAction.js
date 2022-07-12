import { supplierPOActionTypes } from '../../constants/actionTypes';
import { masterData, RequestPayload, supplierPOApiConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData, PostData } from '../../../src/services/api/baseApiService';
import { getlocalizeData, isEmpty, parseValdiationMessage } from '../../utils/commonUtils';
const localConstant = getlocalizeData();
const actions = {
    AddSupplierPODocumentDetails: (payload) => ({
        type: supplierPOActionTypes.ADD_SUPPLIERPO_DOCUMENTS,
        data: payload
    }),
    FetchSupplierPODocumentTypes: (payload) => ({
        type: supplierPOActionTypes.FETCH_SUPPLIERPO_DOCUMENT_TYPES,
        data: payload
    }),
    UpdateSupplierPODocumentDetails: (payload) => ({
        type: supplierPOActionTypes.UPDATE_SUPPLIERPO_DOCUMENTS_DETAILS,
        data: payload
    }),
    DeleteSupplierPODocumentDetails: (payload) => ({
        type: supplierPOActionTypes.DELETE_SUPPLIERPO_DOCUMENTS_DETAILS,
        data: payload
    }),
    FetchVisitDocOfSupplierPo: (payload) => ({
        type: supplierPOActionTypes.FETCH_VISIT_SUPPLIER_PO_DOCUMENTS,
        data: payload
    }),
    AddFilesToBeUpload: (payload) => ({
        type: supplierPOActionTypes.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    ClearFilesToBeUpload: (payload) => ({
        type: supplierPOActionTypes.CLEAR_FILES_TO_BE_UPLOADED,
        data: payload
    }),
};

export const FetchSupplierPODocumentTypes = (data) => async (dispatch, getstate) => {
    const documentTypesMasterDataUrl = masterData.baseUrl + masterData.documentType + "?moduleName=Supplier PO";
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(documentTypesMasterDataUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.supplier.documents.FETCH_DOC_TYPE_FAILED, 'wariningToast supplierPODocumentTypeVal');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchSupplierPODocumentTypes(response.result));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast supplierPODocumentTypeVal');
        }
        else {
            IntertekToaster(localConstant.supplier.documents.FETCH_DOC_TYPE_FAILED, 'dangerToast supplierPODocumentTypeVal');
        }
    }
    else {
        IntertekToaster(localConstant.supplier.documents.FETCH_DOC_TYPE_FAILED, 'dangerToast supplierPODocumentTypeVal');
    }
};
export const AddSupplierPODocumentDetails = (data) => (dispatch) => {
    dispatch(actions.AddSupplierPODocumentDetails(data));
};
export const UpdateSupplierPODocumentDetails = (data, editedData) => (dispatch, getstate) => {
    const state = getstate();
    if (editedData) {
        const editedRow = Object.assign({}, editedData, data);
        const newState = Object.assign([], state.rootSupplierPOReducer.supplierPOData.SupplierPODocuments);
        let index = -1;
        if (editedRow.recordStatus === "N") {
            index = newState.findIndex(
                document => document.documentuniqueIdentifier === editedRow.documentuniqueIdentifier);
        }
        else {
            if (!isEmpty(editedRow.status)) {
                editedRow.status = editedRow.status.trim();
            }
            index = newState.findIndex(document => document.id === editedRow.id);
        }
        newState[index] = editedRow;
        if (index >= 0) {
            dispatch(actions.UpdateSupplierPODocumentDetails(newState));
        }
    }
    else
        dispatch(actions.UpdateSupplierPODocumentDetails(data));
};
export const DeleteSupplierPODocumentDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootSupplierPOReducer.supplierPOData.SupplierPODocuments);
    data && data.forEach(row => {
        newState.forEach((document, i) => {
            if (document.id === row.id) {
                if (row.recordStatus !== "N") {
                    newState[i].recordStatus = "D";
                }
                else {
                    const index = newState.findIndex(value => (value.documentuniqueIdentifier === row.documentuniqueIdentifier));
                    if (index >= 0)
                        newState.splice(index, 1);
                }
            }
        });
    });
    dispatch(actions.DeleteSupplierPODocumentDetails(newState));
};
export const FetchVisitDocOfSupplierPo = (data) => async (dispatch, getstate) => {
    const state = getstate();
    // const visitList = [];
    const supplierPOInfo = Object.assign({}, state.rootSupplierPOReducer.supplierPOData.SupplierPOInfo);
    const visitSupplierPONumber = supplierPOInfo.supplierPOId === null ?
        0 : supplierPOInfo.supplierPOId;
    const supplierPoVisitDocUrl = supplierPOApiConfig.supplierPoBaseUrl + supplierPOApiConfig.visitDocumentsOfSupplierPo;
    const params = {
        'supplierPOId': visitSupplierPONumber
    };
    // visitList.push(visitSupplierPONumber);
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(supplierPoVisitDocUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.validationMessage.FETCH_SUPPLIERPO_VISIT_DOC_FAILED, 'wariningToast supplierPoVisitDocfail');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response)) {
        if (response.code == 1) {
            dispatch(actions.FetchVisitDocOfSupplierPo(response.result));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast supplierPoVisitDocfail');
        }
        else {
            IntertekToaster(localConstant.validationMessage.FETCH_SUPPLIERPO_VISIT_DOC_FAILED, 'wariningToast supplierPoVisitDocfail');
        }
    }
    else {
        IntertekToaster(localConstant.validationMessage.FETCH_SUPPLIERPO_VISIT_DOC_FAILED, 'wariningToast supplierPoVisitDocfail');
    }
};

export const AddFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.AddFilesToBeUpload(data));
};

export const ClearFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.ClearFilesToBeUpload([]));
};