import { projectActionTypes } from '../../constants/actionTypes';
import { projectAPIConfig, masterData, RequestPayload, contractAPIConfig, customerAPIConfig } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { PostData, FetchData } from '../../../src/services/api/baseApiService';
import { getlocalizeData, isEmpty, isUndefined, parseValdiationMessage } from '../../utils/commonUtils';
const localConstant = getlocalizeData();
const actions = {
    AddProjectDocumentDetails: (payload) => ({
        type: projectActionTypes.projectDocuments.ADD_PROJECT_DOCUMENTS,
        data: payload
    }),
    FetchProjectDocumentTypes: (payload) => ({
        type: projectActionTypes.projectDocuments.FETCH_DOCUMENT_TYPES,
        data: payload
    }),
    UpdateProjectDocumentDetails: (payload) => ({
        type: projectActionTypes.projectDocuments.UPDATE_PROJECT_DOCUMENTS_DETAILS,
        data: payload
    }),
    DeleteProjectDocumentDetails: (payload) => ({
        type: projectActionTypes.projectDocuments.DELETE_PROJECT_DOCUMENTS_DETAILS,
        data: payload
    }),
    FetchCustomerDocumentsofProject: (payload) => ({
        type: projectActionTypes.projectDocuments.FETCH_PROJECT_CUSTOMER_DOCUMENTS,
        data: payload
    }),
    FetchContractDocumentsofProject: (payload) => ({
        type: projectActionTypes.projectDocuments.FETCH_PROJECT_CONTRACT_DOCUMENTS,
        data: payload
    }),
    AddFilesToBeUpload: (payload) => ({
        type: projectActionTypes.projectDocuments.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    ClearFilesToBeUpload: (payload) => ({
        type: projectActionTypes.projectDocuments.CLEAR_FILES_TO_BE_UPLOADED,
        data: payload
    }),
};

export const FetchProjectDocumentTypes = (data) => async (dispatch, getstate) => {
    const documentTypesMasterDataUrl = masterData.baseUrl + masterData.documentType + "?moduleName=project";
    const param = {};
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(documentTypesMasterDataUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.project.documents.FETCH_DOC_TYPE_FAILED, 'wariningToast projectDocumentTypeVal');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchProjectDocumentTypes(response.result));
        }
        else if (response.code === "11" || response.code === "41" || response.code === "31") {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast projectDocsWentWrong');
        }
        else {
            IntertekToaster(localConstant.project.documents.FETCH_DOC_TYPE_FAILED, 'dangerToast projectMastedrDocumentsSWWVal');
        }
    }
    else {
        IntertekToaster(localConstant.project.documents.FETCH_DOC_TYPE_FAILED, 'dangerToast projectMastedrDocumentsSWWVal');
    }
};
export const AddProjectDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.AddProjectDocumentDetails(data));
};
export const UpdateProjectDocumentDetails = (data, editedData) => (dispatch, getstate) => {
    const state = getstate();
    if (editedData) {
        const editedRow = Object.assign({}, editedData, data);
        const index = state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectDocuments.findIndex(document => document.id === editedRow.id);
        const newState = Object.assign([], state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectDocuments);
        newState[index] = editedRow;
        if (index >= 0) {
            dispatch(actions.UpdateProjectDocumentDetails(newState));
        }
    }
    else {
        dispatch(actions.UpdateProjectDocumentDetails(data));
    }
};
export const DeleteProjectDocumentDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectDocuments);
    data.map(row => {
        newState.map(document => {
            if (document.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                newState[index].recordStatus = "D";
            }
        });
    });
    dispatch(actions.DeleteProjectDocumentDetails(newState));
};
export const FetchCustomerDocumentsofProject = () => async (dispatch, getstate) => {
    const projectData = getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo;
    const contractCustomerCode = projectData && projectData.contractCustomerCode;
    if (!isEmpty(contractCustomerCode) && !isUndefined(contractCustomerCode)) {
        const projectCustDocUrl = projectAPIConfig.baseUrl + customerAPIConfig.customerDocuments + contractCustomerCode;
        const requestPayload = new RequestPayload();
        const response = await FetchData(projectCustDocUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.project.documents.FETCH_PROJECT_CUSTOMER_DOC_FAILED, 'wariningToast fetchProCustVal');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response) && !isEmpty(response.code)) {
            if (response.code == 1) {
                dispatch(actions.FetchCustomerDocumentsofProject(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast CustDocsWentWrong');
            }
            else {
                IntertekToaster(localConstant.project.documents.FETCH_PROJECT_CUSTOMER_DOC_FAILED, 'dangerToast projectMastedrDocumentsSWWVal');
            }
        }
        else {
            IntertekToaster(localConstant.project.documents.FETCH_PROJECT_CUSTOMER_DOC_FAILED, 'dangerToast projectMastedrDocumentsSWWVal');
        }
    }
};
export const FetchContractDocumentsofProject = () => async (dispatch, getstate) => {
    const contractList = [];
    const projectData = getstate().RootProjectReducer.ProjectDetailReducer.projectDetail.ProjectInfo;
    const selectedContractNumber = projectData && projectData.contractNumber;
    if (selectedContractNumber) {
        const projectContractDocUrl = projectAPIConfig.baseUrl + contractAPIConfig.contractDocumentsOfProject;
        contractList.push(selectedContractNumber);
        const requestPayload = new RequestPayload(contractList);
        const response = await PostData(projectContractDocUrl, requestPayload)
            .catch(error => {
                // console.error(error); // To show the error details in console
                // IntertekToaster(localConstant.project.documents.FETCH_PROJECT_CONTRACT_DOC_FAILED, 'wariningToast projectcontractDocfail');
                IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
            });
        if (!isEmpty(response) && !isEmpty(response.code)) {
            if (response.code == 1) {
                dispatch(actions.FetchContractDocumentsofProject(response.result));
            }
            else if (response.code === "11" || response.code === "41" || response.code === "31") {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast ContractDocsWentWrong');
            }
            else {
                IntertekToaster(localConstant.project.documents.FETCH_DOC_TYPE_FAILED, 'dangerToast projectMastedrDocumentsSWWVal');
            }
        }
        else {
            IntertekToaster(localConstant.project.documents.FETCH_DOC_TYPE_FAILED, 'dangerToast projectMastedrDocumentsSWWVal');
        }
    }
};

export const AddFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.AddFilesToBeUpload(data));
};

export const ClearFilesToBeUpload = (data) => (dispatch) => {
    dispatch(actions.ClearFilesToBeUpload([]));
};