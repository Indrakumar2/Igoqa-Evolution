import { processApiRequest } from '../../../src/services/api/defaultServiceApi';
import { FetchData, PostData } from '../../services/api/baseApiService';
import { contractAPIConfig, customerAPIConfig, masterData, RequestPayload, projectAPIConfig } from '../../apiConfig/apiConfig';
import { contractActionTypes } from '../../constants/actionTypes';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { getlocalizeData, isEmpty, isUndefined, parseValdiationMessage } from '../../utils/commonUtils';
const localConstant = getlocalizeData();
const actions = {
    FetchContractDocumentTypes: (payload) => ({
        type: contractActionTypes.FETCH_DOCUMENT_TYPES,
        data: payload
    }),
    AddDocumentDetails: (payload) => ({
        type: contractActionTypes.ADD_CONTRACT_DOCUMENTS_DETAILS,
        data: payload
    }),
    CopyDocumentDetails: (payload) => ({
        type: contractActionTypes.COPY_DOCUMENTS_DETAILS,
        data: payload
    }),
    DeleteContractDocumentDetails: (payload) => ({
        type: contractActionTypes.DELETE_CONTRACT_DOCUMENTS_DETAILS,
        data: payload
    }),
    UpdateDocumentDetails: (payload) => ({
        type: contractActionTypes.UPDATE_CONTRACT_DOCUMENTS_DETAILS,
        data: payload
    }),
    FetchCustomerDocumentsofContracts: (payload) => ({
        type: contractActionTypes.FETCH_CONTRACT_CUSTOMER_DOCUMENTS,
        data: payload
    }),
    FetchProjectDocumentsofContracts: (payload) => ({
        type: contractActionTypes.FETCH_CONTRACT_PROJECT_DOCUMENTS,
        data: payload
    }),
    FetchContractDocuments: (payload) => ({
        type: contractActionTypes.FETCH_CONTRACT_DOCUMENTS,
        data: payload
    }),
    AddFilesToBeUpload: (payload) => ({
        type: contractActionTypes.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    ClearFilesToBeUpload: (payload) => ({
        type: contractActionTypes.CLEAR_FILES_TO_BE_UPLOADED,
        data: payload
    }),
};

export const AddDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.AddDocumentDetails(data));
};
export const CopyDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.CopyDocumentDetails(data));
};
export const DeleteContractDocumentDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractDocuments);
    data.map(row => {
        newState.map(document => {
            if (document.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                newState[index].recordStatus = "D";
            }
        });
    });
    dispatch(actions.DeleteContractDocumentDetails(newState));
};
export const FetchContractDocumentTypes = (data) => (dispatch, getstate) => {
    const documentTypesMasterDataUrl = masterData.baseUrl + masterData.documentType + masterData.moduleName + masterData.contract;
    processApiRequest(documentTypesMasterDataUrl, {
        method: "GET"
    }).then(response => {
        if (!isUndefined(response) && !isUndefined(response.data) && !isUndefined(response.data.code) && response.data.code == "1") {
            dispatch(actions.FetchContractDocumentTypes(response.data.result));
        } else {
            IntertekToaster(parseValdiationMessage(response.data), 'dangerToast DocActContDocTypes');
        }
    }).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(error, 'dangerToast DocActContDocTypesError');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
};
export const FetchCustomerDocumentsofContracts = () => async (dispatch, getstate) => {
    const customerData = getstate().RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo;
    const customerCode = customerData && customerData.contractCustomerCode;
    if (!isEmpty(customerCode) && !isUndefined(customerCode)) {
        const apiUrl = contractAPIConfig.contractBaseUrl + customerAPIConfig.customerDocuments + customerCode;
        const requestPayload = new RequestPayload();
        const response = await FetchData(apiUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(error, 'dangerToast CustActCustDeterr');
            });
        if (!isEmpty(response)) {
            if (response.code == 1) {
                dispatch(actions.FetchCustomerDocumentsofContracts(response.result));
            }
            else if (response.code == 41 || response.code == 11) {
                if (response.validationMessages.length > 0) {
                    response.validationMessages.map((result, index) => {
                        if (result.messages.length > 0) {
                            result.messages.map(valMessage => {
                                IntertekToaster(valMessage, 'warningToast');
                            });
                        }
                        else {
                            IntertekToaster(localConstant.contract.Documents.FETCH_CONTR_CUST_DOC_FAILED, 'dangerToast CustActCustDet');
                        }
                    });
                }
            }
            else {
                if (response.messages.length > 0) {
                    response.messages.map(result => {
                        if (result.message.length > 0) {
                            IntertekToaster(result.message, 'warningToast');
                        }
                    });
                }
            }
        }
    }
};
export const FetchProjectDocumentsofContracts = () => async (dispatch, getstate) => {
    const state = getstate();
    const contractInfo = Object.assign({}, state.RootContractReducer.ContractDetailReducer.contractDetail.ContractInfo);
    const contractNumber = contractInfo.contractNumber;
    const projectContractDocumentURL = contractAPIConfig.contractBaseUrl + projectAPIConfig.projectDocumentsofContracts + contractNumber;
    const requestPayload = new RequestPayload();
    if (contractNumber) {
        const response = await FetchData(projectContractDocumentURL, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(error, 'dangerToast DocActCustDocError');
            });
        if (!isEmpty(response)) {
            let result = [];   //Changes for Defect 987
            if (!isEmpty(response) && response.code == 1) {
                //Changes for Defect 987 Starts
                if (!isEmpty(response.result)) {
                    result = response.result;
                }
                //Changes for Defect 987 End
                dispatch(actions.FetchProjectDocumentsofContracts(result));
            }
            else if (response.code == 41 || response.code == 11) {
                if (response.validationMessages.length > 0) {
                    response.validationMessages.map((result, index) => {
                        if (result.messages.length > 0) {
                            result.messages.map(valMessage => {
                                IntertekToaster(valMessage, 'warningToast');
                            });
                        }
                        else {
                            IntertekToaster(localConstant.contract.Documents.FECTH_CONTR_PRO_DOC_FAILED, 'dangerToast DocActCustDoc');
                        }
                    });
                }
            }
            else {
                if (response.messages.length > 0) {
                    response.messages.map(result => {
                        if (result.message.length > 0) {
                            IntertekToaster(result.message, 'warningToast');
                        }
                    });
                }
            }
        }
    }
};
export const UpdateDocumentDetails = (data, editedData) => (dispatch, getstate) => {
    const state = getstate();
    if (editedData) {
        const editedRow = Object.assign({}, editedData, data);
        const index = state.RootContractReducer.ContractDetailReducer.contractDetail.ContractDocuments.findIndex(document => document.id === editedRow.id);
        const newState = Object.assign([], state.RootContractReducer.ContractDetailReducer.contractDetail.ContractDocuments);
        newState[index] = editedRow;
        if (index >= 0) {
            dispatch(actions.UpdateDocumentDetails(newState));
        }
    }
    else {
        dispatch(actions.UpdateDocumentDetails(data));
    }
};

/** Fetch Contract Documents */
export const FetchContractDocuments = (data) => async (dispatch, getstate) => {
    dispatch(actions.FetchContractDocuments([]));
    if (data) {
        const contractDocumentUrl = contractAPIConfig.contractDocuments + data;
        const requestPayload = new RequestPayload();
        const response = await FetchData(contractDocumentUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(error, 'dangerToast ContActCustDeterr');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response)) {
            if (response.code == 1) {
                dispatch(actions.FetchContractDocuments(response.result));
            }
            else if (response.code == 41 || response.code == 11) {
                if (response.validationMessages.length > 0) {
                    response.validationMessages.map((result, index) => {
                        if (result.messages.length > 0) {
                            result.messages.map(valMessage => {
                                IntertekToaster(valMessage, 'warningToast');
                            });
                        }
                        else {
                            IntertekToaster(localConstant.contract.Documents.FETCH_CONTR_DOC_FAILED, 'dangerToast ContActCustDet');
                        }
                    });
                }
            }
            else {
                if (response.messages.length > 0) {
                    response.messages.map(result => {
                        if (result.message.length > 0) {
                            IntertekToaster(result.message, 'warningToast');
                        }
                    });
                }
            }
        }
    }
};

export const AddFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.AddFilesToBeUpload(data));
};

export const ClearFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.ClearFilesToBeUpload([]));
};