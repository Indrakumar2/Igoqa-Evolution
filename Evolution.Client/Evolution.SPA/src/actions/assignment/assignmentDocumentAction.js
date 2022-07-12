import { assignmentsActionTypes } from '../../constants/actionTypes';
import { assignmentAPIConfig, masterData, RequestPayload } from '../../apiConfig/apiConfig';
import IntertekToaster from '../../common/baseComponents/intertekToaster';
import { FetchData, PostData } from '../../../src/services/api/baseApiService';
import { getlocalizeData, isEmpty, parseValdiationMessage } from '../../utils/commonUtils';
const localConstant = getlocalizeData();
const actions = {
    AddAssignmentDocumentDetails: (payload) => ({
        type: assignmentsActionTypes.ADD_ASSIGNMENT_DOCUMENTS,
        data: payload
    }),
    FetchAssignmentDocumentTypes: (payload) => ({
        type: assignmentsActionTypes.FETCH_DOCUMENT_TYPES,
        data: payload
    }),
    UpdateAssignmentDocumentDetails: (payload) => ({
        type: assignmentsActionTypes.UPDATE_ASSIGNMENT_DOCUMENTS_DETAILS,
        data: payload
    }),
    DeleteAssignmentDocumentDetails: (payload) => ({
        type: assignmentsActionTypes.DELETE_ASSIGNMENT_DOCUMENTS_DETAILS,
        data: payload
    }),
    FetchAssignmentContractDocuments: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_CONTRACT_DOCUMENTS,
        data: payload
    }),
    FetchAssignmentProjectDocuments: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_PROJECT_DOCUMENTS,
        data: payload
    }),
    FetchAssignmentSupplierPODocuments: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_SUPPLIER_PO_DOCUMENTS,
        data: payload
    }),
    FetchAssignmentVisitDocuments: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_VISIT_DOCUMENTS,
        data: payload
    }),
    FetchAssignmentTimesheetDocuments: (payload) => ({
        type: assignmentsActionTypes.FETCH_ASSIGNMENT_TIMESHEET_DOCUMENTS,
        data: payload
    }),
    AddFilesToBeUpload: (payload) => ({
        type: assignmentsActionTypes.ADD_FILES_TO_BE_UPLOADED,
        data: payload
    }),

    ClearFilesToBeUpload: (payload) => ({
        type: assignmentsActionTypes.CLEAR_FILES_TO_BE_UPLOADED,
        data: payload
    }),
};

export const FetchAssignmentDocumentTypes = (data) => async (dispatch, getstate) => {
    const documentTypesMasterDataUrl = masterData.baseUrl + masterData.documentType + "?moduleName=assignment";
    const isActive=true;
    const param = {
        isActive :isActive//Added for showing only isActive true records in UI, for Admin module isActive filter should be changed to null if needed ,to get all Data .
    };
    const requestPayload = new RequestPayload(param);
    const response = await FetchData(documentTypesMasterDataUrl, requestPayload).catch(error => {
        // console.error(error); // To show the error details in console
        // IntertekToaster(localConstant.assignments.documents.FETCH_DOC_TYPE_FAILED, 'wariningToast assignmentDocumentTypeVal');
        IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
    });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchAssignmentDocumentTypes(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast assignmentDocumentTypeVal');
        }
        else{
            IntertekToaster(localConstant.assignments.documents.FETCH_DOC_TYPE_FAILED, 'wariningToast assignmentDocumentTypeVal');
        }
    }
    else{
        IntertekToaster(localConstant.assignments.documents.FETCH_DOC_TYPE_FAILED, 'wariningToast assignmentDocumentTypeVal');
    }
};
export const AddAssignmentDocumentDetails = (data) => (dispatch) => {
    dispatch(actions.AddAssignmentDocumentDetails(data));
};
export const UpdateAssignmentDocumentDetails = (data, editedData) => (dispatch, getstate) => {
    const state = getstate();
    if(editedData){
    const editedRow = Object.assign({}, editedData, data);
    const index = state.rootAssignmentReducer.assignmentDetail.AssignmentDocuments.findIndex(document => document.id === editedRow.id);
    const newState = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentDocuments);
    newState[index] = editedRow;
    if (index >= 0) {
        dispatch(actions.UpdateAssignmentDocumentDetails(newState));
    }
}
else{
    dispatch(actions.UpdateAssignmentDocumentDetails(data));
}
};
export const DeleteAssignmentDocumentDetails = (data) => (dispatch, getstate) => {
    const state = getstate();
    const newState = Object.assign([], state.rootAssignmentReducer.assignmentDetail.AssignmentDocuments);
    data.map(row => {
        newState.map(document => {
            if (document.id === row.id) {
                const index = newState.findIndex(value => (value.id === row.id));
                newState[index].recordStatus = "D";
            }
        });
    });
    dispatch(actions.DeleteAssignmentDocumentDetails(newState));
};
export const FetchAssignmentContractDocuments = () => async (dispatch, getstate) => {
    const contractList = [];
    const state=getstate();
    const assignmentContractNumber =state.rootAssignmentReducer.assignmentDetail.AssignmentInfo&& state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentContractNumber;
    const assignmentContractDocUrl = assignmentAPIConfig.contractDocumentsOfAssignment;
    contractList.push(assignmentContractNumber);
    const requestPayload = new RequestPayload(contractList);
    if(assignmentContractNumber){
        const response = await PostData(assignmentContractDocUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_CONTRACT_DOC_FAILED, 'wariningToast fetchAssignContractVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
        if (!isEmpty(response) && !isEmpty(response.code)) {
            if (response.code == 1) {
                dispatch(actions.FetchAssignmentContractDocuments(response.result));
            }
            else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
                IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchAssignContractVal');
            }
            else{
                IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_CONTRACT_DOC_FAILED, 'wariningToast fetchAssignContractVal');
            }
        }
        else{
            IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_CONTRACT_DOC_FAILED, 'wariningToast fetchAssignContractVal');
        }
    }
};
export const FetchAssignmentVisitDocuments = () => async (dispatch, getstate) => {
    const state = getstate();
    const assignmentInfo = Object.assign({}, state.rootAssignmentReducer.assignmentDetail.AssignmentInfo);
    const assignmentId = assignmentInfo && assignmentInfo.assignmentId;
    const assignmentVisitDocUrl = assignmentAPIConfig.visitDocumentsOfAssignment;
    const params = {
        'assignmentId': assignmentId
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentVisitDocUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_VISIT_DOC_FAILED, 'wariningToast fetchAssignVisitVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchAssignmentVisitDocuments(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchAssignVisitVal');
        }
        else{
            IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_VISIT_DOC_FAILED, 'wariningToast fetchAssignVisitVal');
        }
    }
    else{
        IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_VISIT_DOC_FAILED, 'wariningToast fetchAssignVisitVal');
    }
};
export const FetchAssignmentTimesheetDocuments = () => async (dispatch, getstate) => {
    const state = getstate();
    const assignmentInfo = Object.assign({}, state.rootAssignmentReducer.assignmentDetail.AssignmentInfo);
    const assignmentId = assignmentInfo && assignmentInfo.assignmentId;
    const assignmentTimesheetDocUrl = assignmentAPIConfig.timesheetDocumentsOfAssignment;
    const params = {
        'assignmentId': assignmentId
    };
    const requestPayload = new RequestPayload(params);
    const response = await FetchData(assignmentTimesheetDocUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_TIMESHEET_DOC_FAILED, 'wariningToast fetchAssignVisitVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchAssignmentTimesheetDocuments(response.result));
        }
        else if (response && (response.code === "11" || response.code === "41" || response.code === "31")) {
            IntertekToaster(parseValdiationMessage(response), 'dangerToast fetchAssignTimesheetVal');
        }
        else{
            IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_TIMESHEET_DOC_FAILED, 'wariningToast fetchAssignTimesheetVal');
        }
    }
    else{
        IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_TIMESHEET_DOC_FAILED, 'wariningToast fetchAssignTimesheetVal');
    }
};
export const FetchAssignmentProjectDocuments = () => async (dispatch, getstate) => {
    const projectList = [];
    const state=getstate();
    const assignmentProjectNumber = state.rootAssignmentReducer.assignmentDetail.AssignmentInfo&&state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentProjectNumber;
    const assignmentProjectDocUrl = assignmentAPIConfig.projectDocumentsOfAssignment;
    projectList.push(assignmentProjectNumber);
    const requestPayload = new RequestPayload(projectList);
    if(assignmentProjectNumber){
        const response = await PostData(assignmentProjectDocUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_PROJECT_DOC_FAILED, 'wariningToast fetchAssignProjVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchAssignmentProjectDocuments(response.result));
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
                        IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_PROJECT_DOC_FAILED, 'dangerToast assignmentProjectrDocumentsSWWVal');
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
export const FetchAssignmentSupplierPODocuments = () => async (dispatch, getstate) => {
    const supplierPOList = [];
    const state=getstate();
    const assignmentSupplierPurchaseOrderId=state.rootAssignmentReducer.assignmentDetail.AssignmentInfo &&state.rootAssignmentReducer.assignmentDetail.AssignmentInfo.assignmentSupplierPurchaseOrderId;
    supplierPOList.push(assignmentSupplierPurchaseOrderId);
    const assignmentSupplierPODocUrl = assignmentAPIConfig.supplierPoDocumentsOfAssignment;
    const requestPayload = new RequestPayload(supplierPOList);
    if(assignmentSupplierPurchaseOrderId){
        const response = await PostData(assignmentSupplierPODocUrl, requestPayload)
        .catch(error => {
            // console.error(error); // To show the error details in console
            // IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_SUPPLIER_PO_DOC_FAILED, 'wariningToast fetchAssignSupPOVal');
            IntertekToaster(localConstant.errorMessages.SOMETHING_WENT_WRONG, 'dangerToast commonError');
        });
    if (!isEmpty(response) && !isEmpty(response.code)) {
        if (response.code == 1) {
            dispatch(actions.FetchAssignmentSupplierPODocuments(response.result));
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
                        IntertekToaster(localConstant.errorMessages.FETCH_ASSIGNMENT_SUPPLIER_PO_DOC_FAILED, 'dangerToast assignmentSupplierPOrDocumentsSWWVal');
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